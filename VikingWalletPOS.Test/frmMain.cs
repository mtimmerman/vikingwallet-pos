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
        private IScsClient client;
        private SynchronizedMessenger<IScsClient> synchronizedMessenger;
        private bool clientStarted;

        public frmMain()
        {
            InitializeComponent();
            server = new Server();
            client = ScsClientFactory.CreateClient(new ScsTcpEndPoint("127.0.0.1", 10085));
            client.MessageReceived += new EventHandler<MessageEventArgs>(client_MessageReceived);
            synchronizedMessenger = new SynchronizedMessenger<IScsClient>(client);
        }

        void client_MessageReceived(object sender, MessageEventArgs e)
        {
            ScsRawDataMessage message = e.Message as ScsRawDataMessage;
            if (message == null)
                return;

            using (MemoryStream readStream = new MemoryStream(message.MessageData))
            {
                EComMessage incomingMessage = EComMessage.ReadFromStream(readStream);
                this.Invoke(new SetTextDelegate(SetText), incomingMessage.RootElement.ToXmlString());
            }
        }

        private void SetText(string text)
        {
            txtResponse.Text = text;
        }

        private delegate void SetTextDelegate(string text);

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Stop();
            client.Disconnect();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnStartClient_Click(object sender, EventArgs e)
        {
            if (!clientStarted)
            {
                synchronizedMessenger.Start();
                client.Connect();
                clientStarted = true;
            }            
        }

        private void btnStopClient_Click(object sender, EventArgs e)
        {
            if (clientStarted)
            {
                synchronizedMessenger.Stop();
                client.Disconnect();
                clientStarted = false;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                EComMessage outgoingMessage = EComMessage.CreateMessageFromElement(ConvertXmlToWbxml(txtRequest.Text));
                outgoingMessage.WriteToStream(stream);

                synchronizedMessenger.SendMessage(new ScsRawDataMessage(stream.ToArray()));
            }
        }

        RootElement ConvertXmlToWbxml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            RootElement root = RootElement.ReadFromXml(doc.DocumentElement);
            return root;
        }
    }
}
