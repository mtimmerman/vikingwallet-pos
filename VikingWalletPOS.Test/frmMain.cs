using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using Newtonsoft.Json;

namespace VikingWalletPOS.Test
{
    /// <summary>
    /// Front end form
    /// </summary>
    public partial class frmMain : Form
    {
        #region Private Members
        private Server server;
        private Client client;
        #endregion

        #region Constructor
        public frmMain()
        {
            InitializeComponent();

            server = new Server();
            client = new Client();

            server.Logged += new EventHandler<LogEventArgs>(ServerMessageLogged);

            client.MessageReceived += new EventHandler<ServerMessageEventArgs>(MessageReceived);
            client.Connected += new EventHandler(ClientConnected);
            client.Disconnected += new EventHandler(ClientDisconnected);            
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// A log message has been received from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServerMessageLogged(object sender, LogEventArgs e) 
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new EventHandler<LogEventArgs>(ServerMessageLogged), sender, e);
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                txtResponse.AppendText(string.Format("\r\n{0}\r\n", e.Message));
            }
        }
        /// <summary>
        /// The client has disconnected from the server, either by choice or because the server went away
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
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
                btnSendPaymentAcknowledge.Enabled = false;
            }
        }
        /// <summary>
        /// The client has successfully connected to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                btnSendPaymentAcknowledge.Enabled = true;
            }
        }
        /// <summary>
        /// A message has been received from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MessageReceived(object sender, ServerMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<ServerMessageEventArgs>(MessageReceived), sender, e);
            }
            else
            {
                txtResponse.AppendText(string.Format("\r\nResponse from server:\r\n{0}\r\n", e.Message));                
            }
        }
        /// <summary>
        /// Start the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            server.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnStartClient.Enabled = true;
        }
        /// <summary>
        /// Form is closing. shut down stuff
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Disconnect();
            server.Stop();            
        }
        /// <summary>
        /// Stop the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopServer_Click(object sender, EventArgs e)
        {
            server.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
        /// <summary>
        /// Start the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartClient_Click(object sender, EventArgs e)
        {
            client.Connect();
        }
        /// <summary>
        /// Stop the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopClient_Click(object sender, EventArgs e)
        {
            client.Disconnect();
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
            SendMessage(writer =>
            {
                writer.WriteAttributeString("id", "dealByPAN");
                writer.WriteAttributeString("tid", txtGetCouponsTerminalId.Text);
                writer.WriteAttributeString("pan", txtGetCouponsCardPAN.Text);
                writer.WriteAttributeString("mid", txtGetCouponsMerchantId.Text);
            });                      
        }
        /// <summary>
        /// Send sample request. Normally our third party would do this request.
        /// 
        /// For testing purposes ONLY!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendRedeem_Click(object sender, EventArgs e)
        {
            SendMessage(writer =>
            {
                writer.WriteAttributeString("id", "redeem");
                writer.WriteAttributeString("tid", txtRedeemTerminalId.Text);
                writer.WriteAttributeString("deal", txtRedeemDealId.Text);
                writer.WriteAttributeString("mid", txtRedeemMerchantId.Text);
            });    
        }
        /// <summary>
        /// Send sample request. Normally our third party would do this request.
        /// 
        /// For testing purposes ONLY!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendPaymentAcknowledge_Click(object sender, EventArgs e)
        {
            SendMessage(writer =>
            {                
                writer.WriteAttributeString("id", "acknowledge");
                writer.WriteAttributeString("tid", txtPaymentAcknowledgeTerminalId.Text);
                writer.WriteAttributeString("deal", txtPaymentAcknowledgeDealId.Text);
                writer.WriteAttributeString("mid", txtPaymentAcknowledgeMerchantId.Text);
                writer.WriteAttributeString("amt", txtPaymentAcknowledgeAmount.Text.Replace(".", ""));
                writer.WriteAttributeString("pmt", txtPaymentAcknowledgePaymentType.Text);
                writer.WriteAttributeString("pan", txtPaymentAcknowledgeCardPAN.Text);
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generic function that will build the XML and send it to the server
        /// </summary>
        /// <param name="writeRequest">Action that expands the XML with the actual request parameters</param>
        private void SendMessage(Action<XmlWriter> writeRequest)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                // Boring stuff like make the XML
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.GetEncoding("iso-8859-1");
                
                XmlWriter writer = XmlWriter.Create(buffer, settings);
                writer.WriteStartElement("req");
                writer.WriteAttributeString("app", "9A");
                writer.WriteAttributeString("ver", "123");
                writer.WriteAttributeString("dt", DateTime.Now.ToString("yyyyMMddHHmmtt"));

                // Do the custom stuff for the request
                writeRequest(writer);

                writer.WriteEndElement();
                writer.Close();

                buffer.Seek(0, SeekOrigin.Begin);
                string xml = Encoding.GetEncoding("iso-8859-1").GetString(buffer.ToArray());

                // Send the XML to the server
                client.SendMessage(xml);
            }
        }
        #endregion                
    }
}
