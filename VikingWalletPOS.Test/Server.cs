using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hik.Communication.Scs.Server;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using System.IO;
using Comtech.Utils;
using Comtech;
using Comtech.Wbxml;
using System.Xml;
using System.Threading.Tasks;
using VikingWalletPOS.Test.Model;

namespace VikingWalletPOS.Test
{
    /// <summary>
    /// This class sets up a TCP server that listens for incoming requests.
    /// 
    /// When it does, it will:
    /// 
    /// - Translate the incoming WBXML message to a readable format.
    /// - Determine which Viking Spots API call needs to be done and execute it using the parameters in the XML
    /// - Translate the response returned by the Viking Spots API back to WBXML
    /// - Send the WBXML to the client
    /// </summary>
    public class Server
    {
        #region Private Members
        private IScsServer server;
        private bool started;
        private API api;
        #endregion

        #region Constructor
        public Server()
        {
            server = ScsServerFactory.CreateServer(new ScsTcpEndPoint(10085));

            server.ClientConnected += new EventHandler<ServerClientEventArgs>(ClientConnected);
            server.ClientDisconnected += new EventHandler<ServerClientEventArgs>(ClientDisconnected);
            
            api = new API();            
        }

        #endregion

        #region Events
        public event EventHandler<StringEventArgs> Logged;
        void Log(string msg)
        {
            if (Logged != null)
            {
                Logged(this, new StringEventArgs(msg));
            }
        }
        #endregion

        #region API Responses
        /// <summary>
        /// The list of coupons has been received
        /// </summary>
        /// <param name="data">The result return by Viking Spots</param>
        /// <param name="client">The client that made the request</param>
        /// <param name="message">The message containing the request</param>
        void CouponListReceived(GetPOSCouponResult data, IScsServerClient client, ScsMessage message)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                // Construct the expected WBXML format from the response

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.GetEncoding("iso-8859-1");

                /*
                 * Example:
                 * 
                 * <rsp code="0" seq="" dsp="" prt="">
                 *  <lst id="deals">
                 *      <row>
                 *          <fld id="dealId" val="" />
                 *          <fld id="name" val="" />
                 *          <fld id="user" val="" />
                 *      </row>
                 *  </lst>
                 *</rsp>
                 */

                XmlWriter writer = XmlWriter.Create(buffer, settings);
                writer.WriteStartElement("rsp");
                writer.WriteAttributeString("code", "0");
                writer.WriteAttributeString("seq", "");
                writer.WriteAttributeString("dsp", "Coupon list has been retrieved");
                writer.WriteAttributeString("prt", "Please choose the correct one");
                writer.WriteStartElement("lst");

                writer.WriteAttributeString("id", "deals");

                foreach (POSCoupon coupon in data.response.coupons)
                {
                    writer.WriteStartElement("row");

                    writer.WriteStartElement("fld");
                    writer.WriteAttributeString("id", "dealId");
                    writer.WriteAttributeString("val", coupon.id.ToString());
                    writer.WriteEndElement(); // fld end element

                    writer.WriteStartElement("fld");
                    writer.WriteAttributeString("id", "name");
                    writer.WriteAttributeString("val", coupon.name);
                    writer.WriteEndElement(); // fld end element

                    writer.WriteStartElement("fld");
                    writer.WriteAttributeString("id", "user");
                    writer.WriteAttributeString("val", coupon.user);
                    writer.WriteEndElement(); // fld end element

                    writer.WriteEndElement(); // row end element
                }
                writer.WriteEndElement(); // lst end element
                writer.WriteEndElement(); // rsp end element
                writer.Close();

                // Get the XML to convert to WBXML
                buffer.Seek(0, SeekOrigin.Begin);
                string outGoingXml = Encoding.GetEncoding("iso-8859-1").GetString(buffer.ToArray());
                
                // Convert to WBXML
                RootElement element = Utils.ConvertXmlToWbxml(outGoingXml);                
                EComMessage outGoingMessage = EComMessage.CreateMessageFromElement(element);

                // Write message to buffer
                using (MemoryStream writeStream = new MemoryStream())
                {
                    outGoingMessage.WriteToStream(writeStream);

                    // Sender message to the client
                    client.SendMessage(new ScsRawDataMessage(writeStream.ToArray(), message.MessageId));
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            if (!started)
            {
                server.Start();
                started = true;
                Log("Server is started successfully.");
            }
            else
            {
                Log("Server is already running!");
            }
        }
        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            if (started)
            {
                server.Stop();
                started = false;
                Log("Server is stopped successfully.");
            }
            else
            {
                Log("Server wasn't running!");
            }
        }
        #endregion

        #region Event Handlers
        void ClientDisconnected(object sender, ServerClientEventArgs e)
        {
            Log(string.Format("A client is disconnected. Client Id = {0}", e.Client.ClientId));
        }

        void ClientConnected(object sender, ServerClientEventArgs e)
        {
            Log(string.Format("A new client connected. Client Id = {0}", e.Client.ClientId));

            // Set up client message handling
            e.Client.MessageReceived += new EventHandler<MessageEventArgs>(ClientMessageReceived);
            e.Client.MessageSent += new EventHandler<MessageEventArgs>(ClientMessageSent);
        }
        /// <summary>
        /// A message has been received from a client. 
        /// Depending on the id of the message a different Viking Spots API will be called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClientMessageReceived(object sender, MessageEventArgs e)
        {
            ScsRawDataMessage message = e.Message as ScsRawDataMessage;
            if (message == null)
                return;

            IScsServerClient client = (IScsServerClient)sender;

            using (MemoryStream readStream = new MemoryStream(message.MessageData))
            {                
                EComMessage incomingMessage = EComMessage.ReadFromStream(readStream);
                // Decode WBXML to XML
                string inComingXml = incomingMessage.RootElement.ToXmlString();

                Log(string.Format("Request from client {0} with id {1}:\r\n{2}",
                    client.ClientId,
                    message.MessageId,
                    inComingXml));
                
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(inComingXml);

                if (doc.DocumentElement.GetAttribute("id") == "dealByPAN")
                {                    
                    // Call GetCoupon
                    int merchant_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("mid"));
                    string card_pan = doc.DocumentElement.GetAttribute("pan");
                    string terminal_id = doc.DocumentElement.GetAttribute("tid");

                    GetPOSCouponRequest request = new GetPOSCouponRequest(merchant_id, card_pan, terminal_id);
                    //Log(string.Format("Contacting Viking Spots API for GetCoupon"));
                    api.GetCouponAsync(request, (response) =>
                    {
                        CouponListReceived(response, client, message);
                    });
                }                
            }            
        }
        void ClientMessageSent(object sender, MessageEventArgs e)
        {
            //ScsRawDataMessage message = e.Message as ScsRawDataMessage;
            //if (message == null)
            //    return;

            //Log(string.Format("Sent message {0} contents in reply to {1}:\r\n{2}",
            //    message.MessageId,
            //    message.RepliedMessageId,
            //    BitConverter.ToString(message.MessageData)));            
        }
        #endregion
    }
}
