namespace VikingWalletPOS
{
    partial class frmMain
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStartClient = new System.Windows.Forms.Button();
            this.gbxServer = new System.Windows.Forms.GroupBox();
            this.btnSendGetCoupons = new System.Windows.Forms.Button();
            this.btnStopClient = new System.Windows.Forms.Button();
            this.gbxClient = new System.Windows.Forms.GroupBox();
            this.tpGetCoupons = new System.Windows.Forms.TabPage();
            this.gbGetCoupons = new System.Windows.Forms.GroupBox();
            this.txtGetCouponsMerchantId = new System.Windows.Forms.TextBox();
            this.lblGetCouponsMerchantId = new System.Windows.Forms.Label();
            this.txtGetCouponsCardPAN = new System.Windows.Forms.TextBox();
            this.lblGetCouponsCardPAN = new System.Windows.Forms.Label();
            this.lblGetCouponsTerminalId = new System.Windows.Forms.Label();
            this.txtGetCouponsTerminalId = new System.Windows.Forms.TextBox();
            this.tcEndpoints = new System.Windows.Forms.TabControl();
            this.tpRedeem = new System.Windows.Forms.TabPage();
            this.gbRedeem = new System.Windows.Forms.GroupBox();
            this.btnSendRedeem = new System.Windows.Forms.Button();
            this.txtRedeemMerchantId = new System.Windows.Forms.TextBox();
            this.lblRedeemMerchantId = new System.Windows.Forms.Label();
            this.lblRedeemDealId = new System.Windows.Forms.Label();
            this.txtRedeemDealId = new System.Windows.Forms.TextBox();
            this.lblRedeemTerminalId = new System.Windows.Forms.Label();
            this.txtRedeemTerminalId = new System.Windows.Forms.TextBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.gbxServer.SuspendLayout();
            this.gbxClient.SuspendLayout();
            this.tpGetCoupons.SuspendLayout();
            this.gbGetCoupons.SuspendLayout();
            this.tcEndpoints.SuspendLayout();
            this.tpRedeem.SuspendLayout();
            this.gbRedeem.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(6, 50);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // btnStartClient
            // 
            this.btnStartClient.Enabled = false;
            this.btnStartClient.Location = new System.Drawing.Point(6, 21);
            this.btnStartClient.Name = "btnStartClient";
            this.btnStartClient.Size = new System.Drawing.Size(75, 23);
            this.btnStartClient.TabIndex = 2;
            this.btnStartClient.Text = "Start";
            this.btnStartClient.UseVisualStyleBackColor = true;
            this.btnStartClient.Click += new System.EventHandler(this.btnStartClient_Click);
            // 
            // gbxServer
            // 
            this.gbxServer.Controls.Add(this.btnStop);
            this.gbxServer.Controls.Add(this.btnStart);
            this.gbxServer.Location = new System.Drawing.Point(3, 3);
            this.gbxServer.Name = "gbxServer";
            this.gbxServer.Size = new System.Drawing.Size(90, 105);
            this.gbxServer.TabIndex = 3;
            this.gbxServer.TabStop = false;
            this.gbxServer.Text = "Server";
            // 
            // btnSendGetCoupons
            // 
            this.btnSendGetCoupons.Enabled = false;
            this.btnSendGetCoupons.Location = new System.Drawing.Point(6, 156);
            this.btnSendGetCoupons.Name = "btnSendGetCoupons";
            this.btnSendGetCoupons.Size = new System.Drawing.Size(75, 23);
            this.btnSendGetCoupons.TabIndex = 6;
            this.btnSendGetCoupons.Text = "Send";
            this.btnSendGetCoupons.UseVisualStyleBackColor = true;
            this.btnSendGetCoupons.Click += new System.EventHandler(this.btnSendGetCoupons_Click);
            // 
            // btnStopClient
            // 
            this.btnStopClient.Enabled = false;
            this.btnStopClient.Location = new System.Drawing.Point(6, 50);
            this.btnStopClient.Name = "btnStopClient";
            this.btnStopClient.Size = new System.Drawing.Size(75, 23);
            this.btnStopClient.TabIndex = 3;
            this.btnStopClient.Text = "Stop";
            this.btnStopClient.UseVisualStyleBackColor = true;
            this.btnStopClient.Click += new System.EventHandler(this.btnStopClient_Click);
            // 
            // gbxClient
            // 
            this.gbxClient.Controls.Add(this.btnStartClient);
            this.gbxClient.Controls.Add(this.btnStopClient);
            this.gbxClient.Location = new System.Drawing.Point(3, 114);
            this.gbxClient.Name = "gbxClient";
            this.gbxClient.Size = new System.Drawing.Size(90, 104);
            this.gbxClient.TabIndex = 5;
            this.gbxClient.TabStop = false;
            this.gbxClient.Text = "Client";
            // 
            // tpGetCoupons
            // 
            this.tpGetCoupons.Controls.Add(this.gbGetCoupons);
            this.tpGetCoupons.Location = new System.Drawing.Point(4, 25);
            this.tpGetCoupons.Name = "tpGetCoupons";
            this.tpGetCoupons.Padding = new System.Windows.Forms.Padding(3);
            this.tpGetCoupons.Size = new System.Drawing.Size(575, 195);
            this.tpGetCoupons.TabIndex = 0;
            this.tpGetCoupons.Text = "Get Coupons";
            this.tpGetCoupons.UseVisualStyleBackColor = true;
            // 
            // gbGetCoupons
            // 
            this.gbGetCoupons.Controls.Add(this.txtGetCouponsMerchantId);
            this.gbGetCoupons.Controls.Add(this.btnSendGetCoupons);
            this.gbGetCoupons.Controls.Add(this.lblGetCouponsMerchantId);
            this.gbGetCoupons.Controls.Add(this.txtGetCouponsCardPAN);
            this.gbGetCoupons.Controls.Add(this.lblGetCouponsCardPAN);
            this.gbGetCoupons.Controls.Add(this.lblGetCouponsTerminalId);
            this.gbGetCoupons.Controls.Add(this.txtGetCouponsTerminalId);
            this.gbGetCoupons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGetCoupons.Location = new System.Drawing.Point(3, 3);
            this.gbGetCoupons.Name = "gbGetCoupons";
            this.gbGetCoupons.Size = new System.Drawing.Size(569, 189);
            this.gbGetCoupons.TabIndex = 0;
            this.gbGetCoupons.TabStop = false;
            this.gbGetCoupons.Text = "Parameters";
            // 
            // txtGetCouponsMerchantId
            // 
            this.txtGetCouponsMerchantId.Location = new System.Drawing.Point(6, 127);
            this.txtGetCouponsMerchantId.Name = "txtGetCouponsMerchantId";
            this.txtGetCouponsMerchantId.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponsMerchantId.TabIndex = 5;
            this.txtGetCouponsMerchantId.Text = "4";
            // 
            // lblGetCouponsMerchantId
            // 
            this.lblGetCouponsMerchantId.AutoSize = true;
            this.lblGetCouponsMerchantId.Location = new System.Drawing.Point(6, 108);
            this.lblGetCouponsMerchantId.Name = "lblGetCouponsMerchantId";
            this.lblGetCouponsMerchantId.Size = new System.Drawing.Size(82, 17);
            this.lblGetCouponsMerchantId.TabIndex = 4;
            this.lblGetCouponsMerchantId.Text = "Merchant Id";
            // 
            // txtGetCouponsCardPAN
            // 
            this.txtGetCouponsCardPAN.Location = new System.Drawing.Point(6, 83);
            this.txtGetCouponsCardPAN.Name = "txtGetCouponsCardPAN";
            this.txtGetCouponsCardPAN.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponsCardPAN.TabIndex = 3;
            this.txtGetCouponsCardPAN.Text = "12345";
            // 
            // lblGetCouponsCardPAN
            // 
            this.lblGetCouponsCardPAN.AutoSize = true;
            this.lblGetCouponsCardPAN.Location = new System.Drawing.Point(6, 63);
            this.lblGetCouponsCardPAN.Name = "lblGetCouponsCardPAN";
            this.lblGetCouponsCardPAN.Size = new System.Drawing.Size(70, 17);
            this.lblGetCouponsCardPAN.TabIndex = 2;
            this.lblGetCouponsCardPAN.Text = "Card PAN";
            // 
            // lblGetCouponsTerminalId
            // 
            this.lblGetCouponsTerminalId.AutoSize = true;
            this.lblGetCouponsTerminalId.Location = new System.Drawing.Point(6, 18);
            this.lblGetCouponsTerminalId.Name = "lblGetCouponsTerminalId";
            this.lblGetCouponsTerminalId.Size = new System.Drawing.Size(78, 17);
            this.lblGetCouponsTerminalId.TabIndex = 1;
            this.lblGetCouponsTerminalId.Text = "Terminal Id";
            // 
            // txtGetCouponsTerminalId
            // 
            this.txtGetCouponsTerminalId.Location = new System.Drawing.Point(6, 38);
            this.txtGetCouponsTerminalId.Name = "txtGetCouponsTerminalId";
            this.txtGetCouponsTerminalId.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponsTerminalId.TabIndex = 0;
            this.txtGetCouponsTerminalId.Text = "12345";
            // 
            // tcEndpoints
            // 
            this.tcEndpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcEndpoints.Controls.Add(this.tpGetCoupons);
            this.tcEndpoints.Controls.Add(this.tpRedeem);
            this.tcEndpoints.Location = new System.Drawing.Point(99, 3);
            this.tcEndpoints.Name = "tcEndpoints";
            this.tcEndpoints.SelectedIndex = 0;
            this.tcEndpoints.Size = new System.Drawing.Size(583, 224);
            this.tcEndpoints.TabIndex = 7;
            // 
            // tpRedeem
            // 
            this.tpRedeem.Controls.Add(this.gbRedeem);
            this.tpRedeem.Location = new System.Drawing.Point(4, 25);
            this.tpRedeem.Name = "tpRedeem";
            this.tpRedeem.Padding = new System.Windows.Forms.Padding(3);
            this.tpRedeem.Size = new System.Drawing.Size(575, 195);
            this.tpRedeem.TabIndex = 1;
            this.tpRedeem.Text = "Redeem";
            this.tpRedeem.UseVisualStyleBackColor = true;
            // 
            // gbRedeem
            // 
            this.gbRedeem.Controls.Add(this.btnSendRedeem);
            this.gbRedeem.Controls.Add(this.txtRedeemMerchantId);
            this.gbRedeem.Controls.Add(this.lblRedeemMerchantId);
            this.gbRedeem.Controls.Add(this.lblRedeemDealId);
            this.gbRedeem.Controls.Add(this.txtRedeemDealId);
            this.gbRedeem.Controls.Add(this.lblRedeemTerminalId);
            this.gbRedeem.Controls.Add(this.txtRedeemTerminalId);
            this.gbRedeem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRedeem.Location = new System.Drawing.Point(3, 3);
            this.gbRedeem.Name = "gbRedeem";
            this.gbRedeem.Size = new System.Drawing.Size(569, 189);
            this.gbRedeem.TabIndex = 0;
            this.gbRedeem.TabStop = false;
            this.gbRedeem.Text = "Parameters";
            // 
            // btnSendRedeem
            // 
            this.btnSendRedeem.Enabled = false;
            this.btnSendRedeem.Location = new System.Drawing.Point(6, 156);
            this.btnSendRedeem.Name = "btnSendRedeem";
            this.btnSendRedeem.Size = new System.Drawing.Size(75, 23);
            this.btnSendRedeem.TabIndex = 8;
            this.btnSendRedeem.Text = "Send";
            this.btnSendRedeem.UseVisualStyleBackColor = true;
            this.btnSendRedeem.Click += new System.EventHandler(this.btnSendRedeem_Click);
            // 
            // txtRedeemMerchantId
            // 
            this.txtRedeemMerchantId.Location = new System.Drawing.Point(6, 127);
            this.txtRedeemMerchantId.Name = "txtRedeemMerchantId";
            this.txtRedeemMerchantId.Size = new System.Drawing.Size(188, 22);
            this.txtRedeemMerchantId.TabIndex = 7;
            this.txtRedeemMerchantId.Text = "4";
            // 
            // lblRedeemMerchantId
            // 
            this.lblRedeemMerchantId.AutoSize = true;
            this.lblRedeemMerchantId.Location = new System.Drawing.Point(6, 108);
            this.lblRedeemMerchantId.Name = "lblRedeemMerchantId";
            this.lblRedeemMerchantId.Size = new System.Drawing.Size(82, 17);
            this.lblRedeemMerchantId.TabIndex = 6;
            this.lblRedeemMerchantId.Text = "Merchant Id";
            // 
            // lblRedeemDealId
            // 
            this.lblRedeemDealId.AutoSize = true;
            this.lblRedeemDealId.Location = new System.Drawing.Point(6, 63);
            this.lblRedeemDealId.Name = "lblRedeemDealId";
            this.lblRedeemDealId.Size = new System.Drawing.Size(52, 17);
            this.lblRedeemDealId.TabIndex = 5;
            this.lblRedeemDealId.Text = "Deal Id";
            // 
            // txtRedeemDealId
            // 
            this.txtRedeemDealId.Location = new System.Drawing.Point(6, 83);
            this.txtRedeemDealId.Name = "txtRedeemDealId";
            this.txtRedeemDealId.Size = new System.Drawing.Size(188, 22);
            this.txtRedeemDealId.TabIndex = 4;
            this.txtRedeemDealId.Text = "12345";
            // 
            // lblRedeemTerminalId
            // 
            this.lblRedeemTerminalId.AutoSize = true;
            this.lblRedeemTerminalId.Location = new System.Drawing.Point(6, 18);
            this.lblRedeemTerminalId.Name = "lblRedeemTerminalId";
            this.lblRedeemTerminalId.Size = new System.Drawing.Size(78, 17);
            this.lblRedeemTerminalId.TabIndex = 3;
            this.lblRedeemTerminalId.Text = "Terminal Id";
            // 
            // txtRedeemTerminalId
            // 
            this.txtRedeemTerminalId.Location = new System.Drawing.Point(6, 38);
            this.txtRedeemTerminalId.Name = "txtRedeemTerminalId";
            this.txtRedeemTerminalId.Size = new System.Drawing.Size(188, 22);
            this.txtRedeemTerminalId.TabIndex = 2;
            this.txtRedeemTerminalId.Text = "12345";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.gbxServer);
            this.pnlTop.Controls.Add(this.gbxClient);
            this.pnlTop.Controls.Add(this.tcEndpoints);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(682, 224);
            this.pnlTop.TabIndex = 9;
            // 
            // txtResponse
            // 
            this.txtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponse.Location = new System.Drawing.Point(0, 224);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(682, 178);
            this.txtResponse.TabIndex = 10;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 402);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(700, 295);
            this.Name = "frmMain";
            this.Text = "Viking Wallet POS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.gbxServer.ResumeLayout(false);
            this.gbxClient.ResumeLayout(false);
            this.tpGetCoupons.ResumeLayout(false);
            this.gbGetCoupons.ResumeLayout(false);
            this.gbGetCoupons.PerformLayout();
            this.tcEndpoints.ResumeLayout(false);
            this.tpRedeem.ResumeLayout(false);
            this.gbRedeem.ResumeLayout(false);
            this.gbRedeem.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStartClient;
        private System.Windows.Forms.GroupBox gbxServer;
        private System.Windows.Forms.Button btnStopClient;
        private System.Windows.Forms.Button btnSendGetCoupons;
        private System.Windows.Forms.GroupBox gbxClient;
        private System.Windows.Forms.TabPage tpGetCoupons;
        private System.Windows.Forms.GroupBox gbGetCoupons;
        private System.Windows.Forms.TextBox txtGetCouponsMerchantId;
        private System.Windows.Forms.Label lblGetCouponsMerchantId;
        private System.Windows.Forms.TextBox txtGetCouponsCardPAN;
        private System.Windows.Forms.Label lblGetCouponsCardPAN;
        private System.Windows.Forms.Label lblGetCouponsTerminalId;
        private System.Windows.Forms.TextBox txtGetCouponsTerminalId;
        private System.Windows.Forms.TabControl tcEndpoints;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.TabPage tpRedeem;
        private System.Windows.Forms.GroupBox gbRedeem;
        private System.Windows.Forms.Button btnSendRedeem;
        private System.Windows.Forms.TextBox txtRedeemMerchantId;
        private System.Windows.Forms.Label lblRedeemMerchantId;
        private System.Windows.Forms.Label lblRedeemDealId;
        private System.Windows.Forms.TextBox txtRedeemDealId;
        private System.Windows.Forms.Label lblRedeemTerminalId;
        private System.Windows.Forms.TextBox txtRedeemTerminalId;
    }
}

