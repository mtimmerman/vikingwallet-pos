using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Comtech;
using Comtech.Wbxml;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Server;
using VikingWalletPOS.Model;

namespace VikingWalletPOS
{
    /// <summary>
    /// This class sets up a TCP server that listens for incoming requests.
    /// 
    /// Course of action:
    /// 
    /// - Translate the incoming WBXML message to a readable format.
    /// - Determine which Viking Spots API call needs to be done and execute it using the parameters in the XML
    /// - Translate the response returned by the Viking Spots API back to WBXML
    /// - Send the WBXML to the client
    /// 
    /// </summary>
    public class Server
    {
        #region Private Members
        // The TCP server
        private IScsServer server;
        private bool started;
        // API handler
        private API api;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of <see cref="Server"/>
        /// </summary>
        public Server()
        {
            server = ScsServerFactory.CreateServer(new ScsTcpEndPoint(Convert.ToInt32(ConfigurationManager.AppSettings["tcpPort"])));
            server.ClientConnected += new EventHandler<ServerClientEventArgs>(ClientConnected);
            server.ClientDisconnected += new EventHandler<ServerClientEventArgs>(ClientDisconnected);
            
            api = new API();            
        }
        #endregion

        #region Events
        /// <summary>
        /// The server logged a message
        /// </summary>
        public event EventHandler<LogEventArgs> Logged;
        void Log(string msg)
        {
            if (Logged != null)
            {
                Logged(this, new LogEventArgs(msg));
            }
        }
        #endregion

        #region API Responses
        /// <summary>
        /// Build the XML that is to be sent in the response back to the client in WBXML format.
        /// </summary>
        /// <param name="client">The client that made the request</param>
        /// <param name="message">The request message</param>
        /// <param name="code"><see cref="HttpStatusCode"/> returned from the Viking Spots API</param>
        /// <param name="response">Result received from the Viking Spots API</param>
        /// <param name="successStep">If the API request was OK, perform this callback</param>
        void BuildAndSendResponse(IScsServerClient client, ScsMessage message, HttpStatusCode code, ResultObject response, Action<XmlWriter> successStep)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                // Construct the expected WBXML format from the response

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.GetEncoding("iso-8859-1");

                XmlWriter writer = XmlWriter.Create(buffer, settings);
                writer.WriteStartElement("rsp");                
                writer.WriteAttributeString("seq", "");
                writer.WriteAttributeString("code", ((int)code).ToString());

                if (code == HttpStatusCode.OK)
                {        
                    // All of the above and below code is always repeated. 
                    // Except for this callback where the xmlwriter is expanded with custom element.
                    // This code is only done when the API call was successful.
                    // Error handling is done generically below
                    successStep(writer);
                }
                else
                {
                    if (response != null)
                    {
                        // Read the first error given back by the server
                        string msg = response.messages.Length > 0 ? response.messages[0].msg_text : "Something went wrong!";
                        writer.WriteAttributeString("dsp", msg);
                    }
                    else if (code == HttpStatusCode.NotFound)
                    {
                        // 404 found
                        writer.WriteAttributeString("dsp", "This endpoint does not exist!");
                    }
                    writer.WriteAttributeString("prt", "");
                }

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
        /// <summary>
        /// A client has disconnected from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClientDisconnected(object sender, ServerClientEventArgs e)
        {
            Log(string.Format("A client is disconnected. Client Id = {0}", e.Client.ClientId));
        }
        /// <summary>
        /// A client has connected to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClientConnected(object sender, ServerClientEventArgs e)
        {
            Log(string.Format("A new client connected. Client Id = {0}", e.Client.ClientId));

            // Set up client message handling
            e.Client.MessageReceived += new EventHandler<MessageEventArgs>(ClientMessageReceived);            
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

            // Only accept Raw data messages
            if (message == null)
                return;

            IScsServerClient client = (IScsServerClient)sender;

            using (MemoryStream readStream = new MemoryStream(message.MessageData))
            {
                // Decode WBXML to XML
                EComMessage incomingMessage = EComMessage.ReadFromStream(readStream);                
                string inComingXml = incomingMessage.RootElement.ToXmlString();

                Log(string.Format("Request from client {0} with id {1}:\r\n{2}",
                    client.ClientId,
                    message.MessageId,
                    inComingXml));
                
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(inComingXml);
                string id = doc.DocumentElement.GetAttribute("id");

                if (id == "dealByPAN")
                {
                    // Extract parameters from request
                    int merchant_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("mid"));
                    string card_pan = doc.DocumentElement.GetAttribute("pan");
                    string terminal_id = doc.DocumentElement.GetAttribute("tid");

                    GetPOSCouponRequest request = new GetPOSCouponRequest(merchant_id, card_pan, terminal_id);

                    // Call poscoupon
                    api.GetCouponAsync(request, (response, code) =>
                    {
                        BuildAndSendResponse(client, message, code, response, (writer) =>
                        {
                            /*
                             * Expand the response with custom stuff for this kind of request
                             * 
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

                            writer.WriteAttributeString("dsp", "Coupon list has been retrieved");
                            writer.WriteAttributeString("prt", "Please choose the correct one");
                            writer.WriteStartElement("lst");

                            writer.WriteAttributeString("id", "deals");

                            foreach (POSCoupon coupon in response.response.coupons)
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
                        });
                    });
                }
                else if (id == "redeem")
                {           
                    // Extract parameters from request
                    int merchant_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("mid"));
                    int deal_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("deal"));
                    string terminal_id = doc.DocumentElement.GetAttribute("tid");

                    POSRedeemRequest request = new POSRedeemRequest(merchant_id, deal_id, terminal_id);

                    // Call posredeemcoupon
                    api.RedeemAsync(request, (response, code) =>
                    {
                        BuildAndSendResponse(client, message, code, response, (writer) =>
                        {
                            /*
                             * Example:
                             * 
                             * <rsp code="0" seq="" dsp="" prt="" />
                             */

                            writer.WriteAttributeString("dsp", "Redeemed successfully!");
                            writer.WriteAttributeString("prt", "Yay!");
                        });
                    });
                }
            }            
        }        
        #endregion
    }
}
