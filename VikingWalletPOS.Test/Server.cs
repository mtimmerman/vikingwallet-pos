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
    public class Server
    {
        private IScsServer server;
        private bool started;
        private API api;
        
        public Server()
        {
            server = ScsServerFactory.CreateServer(new ScsTcpEndPoint(10085));
            
            server.ClientConnected += new EventHandler<ServerClientEventArgs>(server_ClientConnected);
            server.ClientDisconnected += new EventHandler<ServerClientEventArgs>(server_ClientDisconnected);
            api = new API();
            api.CouponListReceived += new ResponseArgs(api_CouponListReceived);
        }

        void api_CouponListReceived(object sender, string response, IScsServerClient client)
        {
            string outGoingXml = "<rsp code='0' dsp='Test passed' prt='Test passed' />";
            EComMessage outGoingMessage = EComMessage.CreateMessageFromElement(Server.ConvertXmlToWbxml(outGoingXml));

            using (MemoryStream writeStream = new MemoryStream())
            {
                outGoingMessage.WriteToStream(writeStream);

                client.SendMessage(new ScsRawDataMessage(writeStream.ToArray()));
            }
        }

        public void Start()
        {
            if (!started)
            {
                server.Start();
                started = true;
                Console.WriteLine("Server is started successfully.");
            }
            else
            {
                Console.WriteLine("Server is already running!");
            }
        }

        public void Stop()
        {
            if (started)
            {
                server.Stop();
                started = false;
                Console.WriteLine("Server is stopped successfully.");
            }
            else
            {
                Console.WriteLine("Server wasn't running!");
            }
        }

        void server_ClientDisconnected(object sender, ServerClientEventArgs e)
        {
            Console.WriteLine(string.Format("A client is disconnected. Client Id = {0}", e.Client.ClientId));
        }

        void server_ClientConnected(object sender, ServerClientEventArgs e)
        {
            Console.WriteLine(string.Format("A new client connected. Client Id = {0}", e.Client.ClientId));
            e.Client.MessageReceived += new EventHandler<MessageEventArgs>(Client_MessageReceived);
            e.Client.MessageSent += messageSentHandler;
        }

        void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            ScsRawDataMessage message = e.Message as ScsRawDataMessage;
            if (message == null)
                return;

            IScsServerClient client = (IScsServerClient)sender;

            using (MemoryStream readStream = new MemoryStream(message.MessageData))
            {                
                EComMessage incomingMessage = EComMessage.ReadFromStream(readStream);

                string inComingXml = incomingMessage.RootElement.ToXmlString();

                Console.WriteLine(string.Format("Received message {0} contents in reply to {1}:",
                    message.MessageId,
                    message.RepliedMessageId));
                Console.WriteLine(inComingXml);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(inComingXml);

                if (doc.DocumentElement.GetAttribute("id") == "dealByPAN")
                {
                    int merchant_id = Convert.ToInt32(doc.DocumentElement.GetAttribute("mid"));
                    string card_pan = doc.DocumentElement.GetAttribute("pan");
                    string terminal_id = doc.DocumentElement.GetAttribute("tid");

                    GetPOSCouponRequest request = new GetPOSCouponRequest(merchant_id, card_pan, terminal_id);
                    api.GetCoupon(request, client);
                }                
            }            
        }
        EventHandler<MessageEventArgs> messageSentHandler = delegate(object sender, MessageEventArgs e)
        {            
            ScsTextMessage message = e.Message as ScsTextMessage;
            if (message == null)
                return;
            Console.WriteLine(string.Format("Sent message {0} contents in reply to {1}:", 
                message.MessageId,
                message.RepliedMessageId));
            Console.WriteLine(message.Text);
        };

        private static RootElement ConvertXmlToWbxml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            RootElement root = RootElement.ReadFromXml(doc.DocumentElement);
            return root;
        }
    }
}
