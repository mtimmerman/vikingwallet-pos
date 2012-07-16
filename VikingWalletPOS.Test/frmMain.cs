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

            server.MessageLogged += new ServerMessageDelegate(ServerMessageLogged);

            client.MessageReceived += new ServerMessageDelegate(MessageReceived);
            client.Connected += new EventHandler(ClientConnected);
            client.Disconnected += new EventHandler(ClientDisconnected);            
        }

        void ServerMessageLogged(string response)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ServerMessageDelegate(ServerMessageLogged), response);
            }
            else
            {
                txtResponse.AppendText(string.Format("\r\n{0}\r\n", response));
            }
        }

        void ClientDisconnected(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(ClientDisconnected), sender, e);
            }
            else
            {
                btnStartClient.Enabled = true;
                btnStopClient.Enabled = false;
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
                clientStarted = true;
            }
        }

        private void MessageReceived(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ServerMessageDelegate(MessageReceived), text);
            }
            else
            {
                txtResponse.AppendText(string.Format("\r\nResponse from server:\r\n{0}\r\n", text));                
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
            server.Stop();
            client.Disconnect();
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

        private void btnSend_Click(object sender, EventArgs e)
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
                writer.WriteAttributeString("ver", "1");
                writer.WriteAttributeString("dt", DateTime.Now.ToString("yyyyMMddHHmmtt"));
                writer.WriteAttributeString("tid", txtGetCouponTerminalId.Text);
                writer.WriteAttributeString("pan", txtGetCouponCardPAN.Text);
                writer.WriteAttributeString("mid", txtGetCouponMerchantId.Text);
                writer.WriteEndElement();
                writer.Close();

                buffer.Seek(0, SeekOrigin.Begin);
                string xml = Encoding.UTF8.GetString(buffer.ToArray());

                txtGetCouponRequest.Text = xml;

                client.SendMessage(xml);
            }
        }        
    }
}
