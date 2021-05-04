using System;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace DynamicIPUpdaterForAWS
{
    public partial class MainForm : Form
    {
        private static string ipServer;
        private static string localIP;

        private Configs configs;
        private FirewallManager firewallManager;

        public MainForm()
        {
            InitializeComponent();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;

            ipServer = ConfigurationManager.AppSettings["IPServer"];
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var loadedConfigs = new Configs();
            loadedConfigs.DeviceName = ConfigurationManager.AppSettings["DeviceName"];

            if (string.IsNullOrWhiteSpace(loadedConfigs.DeviceName))
            {
                MessageBox.Show("Device name not set. Set the 'DeviceName' in the config file.", "Error");
                Close();
            }

            int index = 1;
            do
            {
                var securityGroupId = ConfigurationManager.AppSettings[$"Rule{index}.SecurityGroupId"];
                if (string.IsNullOrWhiteSpace(securityGroupId) ||
                    !int.TryParse(ConfigurationManager.AppSettings[$"Rule{index}.PortToOpen"], out int port) ||
                    port <= 0)
                {
                    break;
                }

                loadedConfigs.Rules.Add(
                    new Configs.Rule()
                    {
                        Port = port,
                        SecurityGroupId = securityGroupId
                    }
                );

                index++;
            } while (true);

            configs = loadedConfigs;
            firewallManager = new FirewallManager(ipServer, configs);
            firewallManager.LoadPublicIp();

            lblLocalIP.Text = configs.PublicIp;

            OpenPorts();
        }

        private void OpenPorts()
        {
            var results = firewallManager.OpenPorts();

            pnlMessages.Controls.Clear();
            foreach (var result in results)
            {
                AddMessage(result.Message, result.Color);
            }
        }

        private bool portClosed = false;

        private void ClosePorts()
        {
            if (portClosed || firewallManager == null) return;
            portClosed = true;

            pnlMessages.Controls.Clear();
            AddMessage("Closing ports...", Color.Orange, DockStyle.Fill);
            Refresh();

            var results = firewallManager.ClosePorts();


            pnlMessages.Controls.Clear();
            foreach (var result in results)
            {
                AddMessage(result.Message, result.Color);
            }

            Refresh();
            Thread.Sleep(2500);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ClosePorts();
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosePorts();
        }

        private void AddMessage(string message, Color color, DockStyle fill = DockStyle.None)
        {
            pnlMessages.Controls.Add(NewLabel(message, color, fill));
        }

        private Label NewLabel(string text, Color color, DockStyle fill = DockStyle.None)
        {
            return new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Microsoft Sans Serif", 22F, FontStyle.Regular, GraphicsUnit.Point, ((byte) (0))),
                ForeColor = SystemColors.ButtonHighlight,
                Location = new Point(12, 289),
                Size = new Size(726, 44),
                TabIndex = 4,
                Text = text,
                BackColor = color,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }
    }
}
