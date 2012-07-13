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
            this.btnSendGetCoupon = new System.Windows.Forms.Button();
            this.btnStopClient = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtGetCouponTerminalId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGetCouponCardPAN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGetCouponMerchantId = new System.Windows.Forms.TextBox();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.txtGetCouponRequest = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
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
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStartClient
            // 
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(90, 83);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // btnSendGetCoupon
            // 
            this.btnSendGetCoupon.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSendGetCoupon.Location = new System.Drawing.Point(119, 157);
            this.btnSendGetCoupon.Name = "btnSendGetCoupon";
            this.btnSendGetCoupon.Size = new System.Drawing.Size(75, 23);
            this.btnSendGetCoupon.TabIndex = 6;
            this.btnSendGetCoupon.Text = "Send";
            this.btnSendGetCoupon.UseVisualStyleBackColor = true;
            this.btnSendGetCoupon.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnStopClient
            // 
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
            this.groupBox3.Location = new System.Drawing.Point(12, 101);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(90, 83);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Client";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtGetCouponRequest);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(554, 195);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Get Coupon";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(108, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(562, 224);
            this.tabControl1.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.txtGetCouponMerchantId);
            this.groupBox2.Controls.Add(this.btnSendGetCoupon);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtGetCouponCardPAN);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtGetCouponTerminalId);
            this.groupBox2.Location = new System.Drawing.Point(6, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 186);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // txtGetCouponTerminalId
            // 
            this.txtGetCouponTerminalId.Location = new System.Drawing.Point(6, 38);
            this.txtGetCouponTerminalId.Name = "txtGetCouponTerminalId";
            this.txtGetCouponTerminalId.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponTerminalId.TabIndex = 0;
            this.txtGetCouponTerminalId.Text = "12345";
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Card PAN";
            // 
            // txtGetCouponCardPAN
            // 
            this.txtGetCouponCardPAN.Location = new System.Drawing.Point(6, 83);
            this.txtGetCouponCardPAN.Name = "txtGetCouponCardPAN";
            this.txtGetCouponCardPAN.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponCardPAN.TabIndex = 3;
            this.txtGetCouponCardPAN.Text = "12345";
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
            // txtGetCouponMerchantId
            // 
            this.txtGetCouponMerchantId.Location = new System.Drawing.Point(6, 128);
            this.txtGetCouponMerchantId.Name = "txtGetCouponMerchantId";
            this.txtGetCouponMerchantId.Size = new System.Drawing.Size(188, 22);
            this.txtGetCouponMerchantId.TabIndex = 5;
            this.txtGetCouponMerchantId.Text = "10";
            // 
            // txtResponse
            // 
            this.txtResponse.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtResponse.Location = new System.Drawing.Point(0, 242);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(682, 160);
            this.txtResponse.TabIndex = 8;
            // 
            // txtGetCouponRequest
            // 
            this.txtGetCouponRequest.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtGetCouponRequest.Location = new System.Drawing.Point(212, 3);
            this.txtGetCouponRequest.Multiline = true;
            this.txtGetCouponRequest.Name = "txtGetCouponRequest";
            this.txtGetCouponRequest.ReadOnly = true;
            this.txtGetCouponRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGetCouponRequest.Size = new System.Drawing.Size(339, 189);
            this.txtGetCouponRequest.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 402);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(700, 295);
            this.Name = "frmMain";
            this.Text = "Viking Wallet POS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStartClient;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStopClient;
        private System.Windows.Forms.Button btnSendGetCoupon;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtGetCouponMerchantId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGetCouponCardPAN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGetCouponTerminalId;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox txtGetCouponRequest;
        private System.Windows.Forms.TextBox txtResponse;
    }
}

