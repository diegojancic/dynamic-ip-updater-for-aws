namespace EnableRdpInAws
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblLocalIP = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCloseMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(726, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Your IP Address:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.Location = new System.Drawing.Point(307, 297);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 41);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close Port";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblLocalIP
            // 
            this.lblLocalIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLocalIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalIP.Location = new System.Drawing.Point(12, 44);
            this.lblLocalIP.Name = "lblLocalIP";
            this.lblLocalIP.Size = new System.Drawing.Size(726, 26);
            this.lblLocalIP.TabIndex = 2;
            this.lblLocalIP.Text = "(loading...)";
            this.lblLocalIP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 84);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(726, 26);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "[status]";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblStatus.Visible = false;
            // 
            // lblCloseMessage
            // 
            this.lblCloseMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCloseMessage.BackColor = System.Drawing.Color.Red;
            this.lblCloseMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCloseMessage.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblCloseMessage.Location = new System.Drawing.Point(12, 165);
            this.lblCloseMessage.Name = "lblCloseMessage";
            this.lblCloseMessage.Size = new System.Drawing.Size(726, 44);
            this.lblCloseMessage.TabIndex = 4;
            this.lblCloseMessage.Text = "Closing port...";
            this.lblCloseMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCloseMessage.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 350);
            this.Controls.Add(this.lblCloseMessage);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblLocalIP);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dynamic IP Updater For AWS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblLocalIP;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCloseMessage;
    }
}

