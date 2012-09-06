using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using Comtech;
using Comtech.Wbxml;
using VikingWalletPOS.Model;

namespace VikingWalletPOS
{
    public enum Codes
    {
        OK = 0,
        InternalError = -1,
        NotFoundError = 1,
        NoEndPoint = 2,
        CannotRedeem = 3
    }

    public class ClientHandler
    {
        #region Private Members
        private TcpClient clientSocket;
        private NetworkStream networkStream;
        bool continueProcess = false;
        private byte[] bytes;        
        private byte[] data = null;
        private API api;
        private Dictionary<Codes, string> displays;
        private Dictionary<string, string> translations;
        #endregion

        #region Constructor
        public ClientHandler(TcpClient clientSocket)
        {
            clientSocket.ReceiveTimeout = 100;
            this.clientSocket = clientSocket;
            networkStream = clientSocket.GetStream();
            bytes = new byte[clientSocket.ReceiveBufferSize];
            continueProcess = true;
            api = new API();

            displays = new Dictionary<Codes, string>();
            displays.Add(Codes.NoEndPoint, "NO ENDPOINT");
            displays.Add(Codes.NotFoundError, "NOT FOUND");
            displays.Add(Codes.CannotRedeem, "CANNOT REDEEM");
            displays.Add(Codes.InternalError, "INTERNAL ERROR");

            translations = new Dictionary<string,string>();
            translations.Add("Coupon", "Deal");
            translations.Add("Deal", "Deal");
            translations.Add("Location", "Terminal Id");            
        }
        #endregion

        #region Events
        public event EventHandler<LogEventArgs> Logged;

        void OnLogged(string message, params object[] args)
        {
            if (Logged != null)
            {
                Logged(this, new LogEventArgs(string.Format(message, args)));
            }
        }
        #endregion

        #region Public Methods
        public void Process()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    int bytesRead = networkStream.Read(bytes, 0, bytes.Length);
                    do
                    {
                        if (bytesRead > 0)
                        {
                            stream.Write(bytes, 0, bytesRead);
                        }
                        else
                        {
                            ProcessDataReceived(stream);
                        }
                    }
                    while ((bytesRead = networkStream.Read(bytes, 0, bytes.Length)) > 0);

                }
                catch (IOException)
                {
                    ProcessDataReceived(stream);
                }
                catch (SocketException)
                {
                    networkStream.Close();
                    clientSocket.Close();
                    continueProcess = false;
                    //OnLogged("Connection with the client is broken!");
                }
            }
        }
        #endregion

        private void ProcessDataReceived(Stream stream)
        {
            if (stream.Length > 0)
            {
                try
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    // Decode WBXML to XML
                    EComMessage incomingMessage = EComMessage.ReadFromStream(stream);
                    string inComingXml = incomingMessage.RootElement.ToXmlString();

                    //OnLogged("Received the following request:\r\n{0}", inComingXml);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(inComingXml);
                    string id = doc.DocumentElement.GetAttribute("id");

                    if (id == "dealByPAN")
                    {
                        DealByPAN(doc);
                    }
                    else if (id == "redeem")
                    {
                        Redeem(doc);
                    }
                    else if (id == "acknowledge")
                    {
                        AcknowledgePayment(doc);
                    }
                }
                catch (Exception ex)
                {
                    BuildAndSendResponse(HttpStatusCode.InternalServerError, new InternalServerError(ex), (writer) => { });
                }
            }
        }

        public bool Alive
        {
            get
            {
                return continueProcess;
            }
        }

        public void Close()
        {
            networkStream.Close();
            clientSocket.Close();
        }

        private string BuildPrintStatement(string message, params object[] args)
        {
            return BuildPrintStatement(message, 48, args);
        }
        private string BuildPrintStatement(string message, int length,params object[] args) 
        {
            message = string.Format(message, args);
            length = Math.Min(message.Length, length);
            return message.Substring(0, length);
        }

        /// <summary>
        /// Build the XML that is to be sent in the response back to the client in WBXML format.
        /// </summary>
        /// <param name="client">The client that made the request</param>
        /// <param name="message">The request message</param>
        /// <param name="code"><see cref="HttpStatusCode"/> returned from the Viking Spots API</param>
        /// <param name="response">Result received from the Viking Spots API</param>
        /// <param name="successStep">If the API request was OK, perform this callback</param>
        private void BuildAndSendResponse(HttpStatusCode code, ResultObject response, Action<XmlWriter> successStep)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                // Construct the expected WBXML format from the response

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = false;
                settings.Encoding = Encoding.GetEncoding("iso-8859-1");

                XmlWriter writer = XmlWriter.Create(buffer, settings);
                writer.WriteStartElement("rsp");
                writer.WriteAttributeString("seq", "");
                
                if (code == HttpStatusCode.OK)
                {
                    writer.WriteAttributeString("code", ((int)Codes.OK).ToString());
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
                        string msg_code = response.messages[0].msg_code;
                        string opt_field_value = response.messages[0].opt_field_value;
                        string msg_text = response.messages[0].msg_text;

                        if (msg_code == "X_002")
                        {
                            // InternalException
                            writer.WriteAttributeString("code", ((int)Codes.InternalError).ToString());
                            writer.WriteAttributeString("dsp", displays[Codes.InternalError]);
                            writer.WriteAttributeString("prt", BuildPrintStatement("{0} - An unexpected exception happened", 
                                displays[Codes.InternalError]));                            
                        }
                        else if (msg_code == "X_003")
                        {
                            // DoesNotExistException
                            writer.WriteAttributeString("code", ((int)Codes.NotFoundError).ToString());
                            writer.WriteAttributeString("dsp", displays[Codes.NotFoundError]);

                            writer.WriteAttributeString("prt", BuildPrintStatement("{0} - This {1} does not exist",
                                    displays[Codes.NotFoundError],
                                    translations[opt_field_value]));                            
                        }
                        else if (msg_code == "X_020")
                        {
                            // CannotRedeemCouponException
                            writer.WriteAttributeString("code", ((int)Codes.CannotRedeem).ToString());
                            writer.WriteAttributeString("dsp", displays[Codes.CannotRedeem]);

                            writer.WriteAttributeString("prt", BuildPrintStatement("{0} - {1}",
                                displays[Codes.CannotRedeem],
                                opt_field_value));
                        }
                    }
                    else if (code == HttpStatusCode.NotFound)
                    {
                        // 404 found
                        writer.WriteAttributeString("code", ((int)Codes.NoEndPoint).ToString());
                        writer.WriteAttributeString("dsp", displays[Codes.NoEndPoint]);
                        writer.WriteAttributeString("prt", BuildPrintStatement("{0} - This API does not exist",
                            displays[Codes.NoEndPoint]));
                    }                    
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
                    byte[] data = writeStream.ToArray();
                    networkStream.Write(data, 0, data.Length);
                }
            }
        }
        /// <summary>
        /// Do API request GetCoupon and send response back to the client
        /// </summary>
        /// <param name="doc">request XML</param>
        /// <param name="client">client that send the request</param>
        /// <param name="message">The message containing the request</param>
        private void DealByPAN(XmlDocument doc)
        {
            // Extract parameters from request
            int merchant_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("mid"));
            string card_pan = doc.DocumentElement.GetAttribute("pan");
            int terminal_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("tid"));

            GetPOSCouponRequest request = new GetPOSCouponRequest(merchant_id, card_pan, terminal_id);
            HttpStatusCode code = HttpStatusCode.NotFound;
            GetPOSCouponResult response = api.GetCoupon(request, ref code);

            if (response != null)
            {
                BuildAndSendResponse(code, response, (writer) =>
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

                    //writer.WriteAttributeString("dsp", "Coupon list has been retrieved");
                    //writer.WriteAttributeString("prt", "Please choose the correct one");
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
                        writer.WriteAttributeString("val", coupon.name.Substring(0, 16));
                        writer.WriteEndElement(); // fld end element

                        writer.WriteStartElement("fld");
                        writer.WriteAttributeString("id", "user");
                        writer.WriteAttributeString("val", coupon.user);
                        writer.WriteEndElement(); // fld end element

                        writer.WriteEndElement(); // row end element
                    }
                    writer.WriteEndElement(); // lst end element  
                });
            }
        }
        /// <summary>
        /// Do API request Redeem and send response back to the client
        /// </summary>
        /// <param name="doc">request XML</param>
        /// <param name="client">client that send the request</param>
        /// <param name="message">The message containing the request</param>
        private void Redeem(XmlDocument doc)
        {
            // Extract parameters from request
            int merchant_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("mid"));
            int coupon_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("deal"));
            string terminal_id = doc.DocumentElement.GetAttribute("tid");

            POSRedeemRequest request = new POSRedeemRequest(merchant_id, coupon_id, terminal_id);

            HttpStatusCode code = HttpStatusCode.NotFound;
            // Call posredeemcoupon
            POSRedeemResult response = api.Redeem(request, ref code);

            BuildAndSendResponse(code, response, (writer) =>
            {
                /*
                 * Example:
                 * 
                 * <rsp code="0" seq="" dsp="" prt="" />
                 */

                //writer.WriteAttributeString("dsp", "Redeemed successfully!");
                //writer.WriteAttributeString("prt", "Yay!");
            });
        }
        /// <summary>
        /// Do API request AcknowledgePayment and send response back to the client
        /// </summary>
        /// <param name="doc">request XML</param>
        /// <param name="client">client that send the request</param>
        /// <param name="message">The message containing the request</param>
        private void AcknowledgePayment(XmlDocument doc)
        {
            // Extract parameters from request
            int merchant_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("mid"));
            int coupon_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("deal"));
            string terminal_id = doc.DocumentElement.GetAttribute("tid");
            string amountString = doc.DocumentElement.GetAttribute("amt");
            amountString = amountString.Insert(amountString.Length - 2, ".");
            IFormatProvider culture = new CultureInfo("en-us");
            double amount = Convert.ToDouble(amountString, culture);
            string card_pan = doc.DocumentElement.GetAttribute("pan");
            string payment_type = doc.DocumentElement.GetAttribute("pmt");

            POSPaymentAcknowledgeRequest request = new POSPaymentAcknowledgeRequest(terminal_id, coupon_id, merchant_id, amount, card_pan, payment_type);

            HttpStatusCode code = HttpStatusCode.NotFound;
            // Call posacknowledgepayment
            POSPaymentAcknowledgeResult response = api.PaymentAcknowledge(request, ref code);

            BuildAndSendResponse(code, response, (writer) =>
            {
                /*
                 * Example:
                 * 
                 * <rsp code="0" seq="" dsp="" prt="" />
                 */

                //writer.WriteAttributeString("dsp", "Successfully acknowledged payment!");
                //writer.WriteAttributeString("prt", "Yay!");
            });
        }
    }
}
