using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VikingWalletPOS;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messengers;
using Comtech.Wbxml;
using System.Xml;
using System.IO;
using Comtech;
using Hik.Communication.Scs.Communication.Messages;

namespace VikingWalletPOS.Test
{
    public partial class frmMain : Form
    {
        private Server server;
        private Client client;
        private bool clientStarted;
        private bool serverStarted;

        public frmMain()
        {
            InitializeComponent();
            server = new Server();
            client = new Client();

            server.Logged += new EventHandler<StringEventArgs>(ServerMessageLogged);

            client.MessageReceived += new EventHandler<StringEventArgs>(MessageReceived);
            client.Connected += new EventHandler(ClientConnected);
            client.Disconnected += new EventHandler(ClientDisconnected);            
        }

        void ServerMessageLogged(object sender, StringEventArgs e) 
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new EventHandler<StringEventArgs>(ServerMessageLogged), sender, e);
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                txtResponse.AppendText(string.Format("\r\n{0}\r\n", e.Message));
            }
        }

        void ClientDisconnected(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new EventHandler(ClientDisconnected), sender, e);
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                btnStartClient.Enabled = true;
                btnStopClient.Enabled = false;
                btnSendGetCoupons.Enabled = false;
                btnSendRedeem.Enabled = false;
                clientStarted = false;
            }
        }

        void ClientConnected(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(ClientConnected), sender, e);
            }
            else
            {
                btnStartClient.Enabled = false;
                btnStopClient.Enabled = true;
                btnSendGetCoupons.Enabled = true;
                btnSendRedeem.Enabled = true;
                clientStarted = true;
            }
        }

        private void MessageReceived(object sender, StringEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<StringEventArgs>(MessageReceived), sender, e);
            }
            else
            {
                txtResponse.AppendText(string.Format("\r\nResponse from server:\r\n{0}\r\n", e.Message));                
            }
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            server.Start();
            serverStarted = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnStartClient.Enabled = true;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Disconnect();
            server.Stop();            
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            server.Stop();
            serverStarted = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnStartClient_Click(object sender, EventArgs e)
        {
            if (!clientStarted && serverStarted)
            {
                client.Connect();                
            }            
        }

        private void btnStopClient_Click(object sender, EventArgs e)
        {
            if (clientStarted)
            {
                client.Disconnect();                
            }
        }

        /// <summary>
        /// Send sample request. Normally our third party would do this request.
        /// 
        /// For testing purposes ONLY!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendGetCoupons_Click(object sender, EventArgs e)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = UTF8Encoding.Default;

                XmlWriter writer = XmlWriter.Create(buffer, settings);
                writer.WriteStartElement("req");
                writer.WriteAttributeString("app", "??");
                writer.WriteAttributeString("id", "dealByPAN");
                writer.WriteAttributeString("ver", "123");
                writer.WriteAttributeString("dt", DateTime.Now.ToString("yyyyMMddHHmmtt"));
                writer.WriteAttributeString("tid", txtGetCouponsTerminalId.Text);
                writer.WriteAttributeString("pan", txtGetCouponsCardPAN.Text);
                writer.WriteAttributeString("mid", txtGetCouponsMerchantId.Text);
                writer.WriteEndElement();
                writer.Close();

                buffer.Seek(0, SeekOrigin.Begin);
                string xml = Encoding.UTF8.GetString(buffer.ToArray());

                client.SendMessage(xml);
            }            
        }

        private void btnSendRedeem_Click(object sender, EventArgs e)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.GetEncoding("iso-8859-1");

                XmlWriter writer = XmlWriter.Create(buffer, settings);
                writer.WriteStartElement("req");
                writer.WriteAttributeString("app", "??");
                writer.WriteAttributeString("id", "redeem");
                writer.WriteAttributeString("ver", "123");
                writer.WriteAttributeString("dt", DateTime.Now.ToString("yyyyMMddHHmmtt"));
                writer.WriteAttributeString("tid", txtRedeemTerminalId.Text);
                writer.WriteAttributeString("deal", txtRedeemDealId.Text);
                writer.WriteAttributeString("mid", txtRedeemMerchantId.Text);
                writer.WriteEndElement();
                writer.Close();

                buffer.Seek(0, SeekOrigin.Begin);
                string xml = Encoding.GetEncoding("iso-8859-1").GetString(buffer.ToArray());

                client.SendMessage(xml);
            }  
        }        
    }
}
