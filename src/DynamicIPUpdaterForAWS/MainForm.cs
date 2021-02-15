using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime.Internal;

namespace EnableRdpInAws
{
    public partial class MainForm : Form
    {
        private static string ipServer;
        private static string localIP;
        private static string securityGroupId;
        private static int clientPort;
        private static int serverPort;

        public MainForm()
        {
            InitializeComponent();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;

            ipServer = ConfigurationManager.AppSettings["IPServer"];
            securityGroupId = ConfigurationManager.AppSettings["SecurityGroupId"];

            clientPort = int.Parse(ConfigurationManager.AppSettings["ClientPortToOpen"]);
            serverPort = int.Parse(ConfigurationManager.AppSettings["ServerPortToOpen"]);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            localIP = GetPublicIp();

            lblLocalIP.Text = localIP;

            OpenPort();
        }

        private static string GetPublicIp()
        {
            var req = WebRequest.Create(ipServer);
            var response = req.GetResponse();

            if (response == null) throw new Exception("Response returned is null =(.  No internet connection?");

            var responseStream = response.GetResponseStream();
            if (responseStream == null) throw new Exception("Response stream is null =(.  No internet connection?");


            string returnedText;
            using (responseStream)
            using (var reader = new StreamReader(responseStream))
            {
                returnedText = reader.ReadToEnd().Trim();
            }

            return returnedText;
        }

        private void OpenPort()
        {
            var client = new AmazonEC2Client();

            AuthorizeSecurityGroupIngressResponse res = null;

            try
            {
                res = client.AuthorizeSecurityGroupIngress(new AuthorizeSecurityGroupIngressRequest()
                {
                    IpPermissions = new List<IpPermission>() {GetIpPermissionRule()},
                    GroupId = securityGroupId
                });

            }
            catch (AmazonEC2Exception ex)
            {
                if (ex.Message.Contains("the specified rule") && ex.Message.Contains("already exists"))
                {
                    lblStatus.Text = $"Success: Connection to port {serverPort} is (already) OPEN";
                    lblStatus.ForeColor = Color.DarkGreen;
                    lblStatus.Visible = true;
                    return;
                }
                throw;
            }

            if (res.HttpStatusCode == HttpStatusCode.OK)
            {
                lblStatus.Text = $"Success: Connection to port {serverPort} is OPEN";
                lblStatus.ForeColor = Color.DarkGreen;
                lblStatus.Visible = true;
            }
            else
            {
                lblStatus.Text = $"Error: couldn't open port {serverPort}. Code: " + res.HttpStatusCode;
                lblStatus.ForeColor = Color.DarkRed;
                lblStatus.Visible = true;
            }
        }

        private bool portClosed = false;
        private void ClosePort()
        {
            if (portClosed) return;
            portClosed = true;

            foreach (Control control in this.Controls)
            {
                if (control != lblCloseMessage) control.Visible = false;
            }

            Text = "Closing port...";
            lblCloseMessage.Dock = DockStyle.Fill;
            lblCloseMessage.BackColor = Color.Orange;
            lblCloseMessage.Visible = true;
            Refresh();

            var client = new AmazonEC2Client();

            RevokeSecurityGroupIngressResponse res = null;
            try
            {
                res = client.RevokeSecurityGroupIngress(new RevokeSecurityGroupIngressRequest()
                {
                    IpPermissions = new List<IpPermission>() {GetIpPermissionRule()},
                    GroupId = securityGroupId
                });
            }
            catch (AmazonEC2Exception ex)
            {
                if (ex.Message.Contains("The specified rule does not exist"))
                {
                    lblCloseMessage.Text = $"Success: Connection to port {serverPort} (already) CLOSED";
                    lblCloseMessage.BackColor = Color.DarkGreen;

                    Refresh();
                    Thread.Sleep(2500);
                    return;
                }
                throw;
            }

            if (res.HttpStatusCode == HttpStatusCode.OK)
            {
                lblCloseMessage.Text = $"Success: Connection to port {serverPort} CLOSED";
                lblCloseMessage.BackColor = Color.DarkGreen;

                Refresh();
                Thread.Sleep(2500);
            }
            else
            {
                lblCloseMessage.Text = "Error: couldn't close port. Code: " + res.HttpStatusCode;
                lblCloseMessage.BackColor = Color.DarkRed;

                Refresh();
                Thread.Sleep(5000);
            }
        }

        private static IpPermission GetIpPermissionRule()
        {
            return new IpPermission()
            {
                FromPort = clientPort,
                ToPort = serverPort,
                IpProtocol = "tcp",
                IpRanges = new List<string>(1) {localIP + "/32"}
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ClosePort();
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosePort();
        }
    }
}
