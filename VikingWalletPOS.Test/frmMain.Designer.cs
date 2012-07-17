namespace VikingWalletPOS.Test
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSendGetCoupons = new System.Windows.Forms.Button();
            this.btnStopClient = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtGetCouponsMerchantId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGetCouponsCardPAN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGetCouponsTerminalId = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRedeemTerminalId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRedeemDealId = new System.Windows.Forms.TextBox();
            this.txtRedeemMerchantId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSendRedeem = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(90, 105);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnStartClient);
            this.groupBox3.Controls.Add(this.btnStopClient);
            this.groupBox3.Location = new System.Drawing.Point(3, 114);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(90, 104);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Client";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(575, 195);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Get Coupons";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtGetCouponsMerchantId);
            this.groupBox2.Controls.Add(this.btnSendGetCoupons);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtGetCouponsCardPAN);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtGetCouponsTerminalId);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(569, 189);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // txtGetCouponsMerchantId
            // 
            this.txtGetCouponsMerchantId.Location = new System.Drawing.Point(6, 127);
            this.txtGetCouponsMerchantId.Name = "txtGetCouponsMerchantId";
            this.txtGetCouponsMerchantId.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponsMerchantId.TabIndex = 5;
            this.txtGetCouponsMerchantId.Text = "4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Merchant Id";
            // 
            // txtGetCouponsCardPAN
            // 
            this.txtGetCouponsCardPAN.Location = new System.Drawing.Point(6, 83);
            this.txtGetCouponsCardPAN.Name = "txtGetCouponsCardPAN";
            this.txtGetCouponsCardPAN.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponsCardPAN.TabIndex = 3;
            this.txtGetCouponsCardPAN.Text = "12345";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Card PAN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Terminal Id";
            // 
            // txtGetCouponsTerminalId
            // 
            this.txtGetCouponsTerminalId.Location = new System.Drawing.Point(6, 38);
            this.txtGetCouponsTerminalId.Name = "txtGetCouponsTerminalId";
            this.txtGetCouponsTerminalId.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponsTerminalId.TabIndex = 0;
            this.txtGetCouponsTerminalId.Text = "12345";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(99, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(583, 224);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(575, 195);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Redeem";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSendRedeem);
            this.groupBox4.Controls.Add(this.txtRedeemMerchantId);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtRedeemDealId);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtRedeemTerminalId);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(569, 189);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Parameters";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(682, 224);
            this.panel1.TabIndex = 9;
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Terminal Id";
            // 
            // txtRedeemTerminalId
            // 
            this.txtRedeemTerminalId.Location = new System.Drawing.Point(6, 38);
            this.txtRedeemTerminalId.Name = "txtRedeemTerminalId";
            this.txtRedeemTerminalId.Size = new System.Drawing.Size(188, 22);
            this.txtRedeemTerminalId.TabIndex = 2;
            this.txtRedeemTerminalId.Text = "12345";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Deal Id";
            // 
            // txtRedeemDealId
            // 
            this.txtRedeemDealId.Location = new System.Drawing.Point(6, 83);
            this.txtRedeemDealId.Name = "txtRedeemDealId";
            this.txtRedeemDealId.Size = new System.Drawing.Size(188, 22);
            this.txtRedeemDealId.TabIndex = 4;
            this.txtRedeemDealId.Text = "12345";
            // 
            // txtRedeemMerchantId
            // 
            this.txtRedeemMerchantId.Location = new System.Drawing.Point(6, 127);
            this.txtRedeemMerchantId.Name = "txtRedeemMerchantId";
            this.txtRedeemMerchantId.Size = new System.Drawing.Size(188, 22);
            this.txtRedeemMerchantId.TabIndex = 7;
            this.txtRedeemMerchantId.Text = "4";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Merchant Id";
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
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 402);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(700, 295);
            this.Name = "frmMain";
            this.Text = "Viking Wallet POS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStartClient;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStopClient;
        private System.Windows.Forms.Button btnSendGetCoupons;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtGetCouponsMerchantId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGetCouponsCardPAN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGetCouponsTerminalId;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSendRedeem;
        private System.Windows.Forms.TextBox txtRedeemMerchantId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRedeemDealId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRedeemTerminalId;
    }
}

