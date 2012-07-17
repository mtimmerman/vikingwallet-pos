using System;
using System.Configuration;
using System.IO;
using Comtech;
using Comtech.Wbxml;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Communication.Messengers;

namespace VikingWalletPOS
{
    /// <summary>
    /// This class is a wrapper around the Client-side logic. 
    /// Third parties will communicate with the Server-side logic in a similar way as described in this class
    /// It's purpose is for testing only.
    /// </summary>
    public class Client
    {
        #region Private Members
        // TCP client that connects to our POS server
        private IScsClient client;
        // Messenger that handles the communication between client and server
        private RequestReplyMessenger<IScsClient> messenger;        
        #endregion

        #region Events
        /// <summary>
        /// A message has been received from the Server
        /// </summary>
        public event EventHandler<ServerMessageEventArgs> MessageReceived;
        /// <summary>
        /// The client has successfully connected to the server
        /// </summary>
        public event EventHandler Connected;
        /// <summary>
        /// The client has disconnected from the server
        /// </summary>
        public event EventHandler Disconnected;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of <see cref="Client"/>
        /// </summary>
        public Client()
        {
            // Set up the client
            client = ScsClientFactory.CreateClient(new ScsTcpEndPoint(ConfigurationManager.AppSettings["tcpServer"],
                                                                      Convert.ToInt32(ConfigurationManager.AppSettings["tcpPort"])));
            client.MessageReceived += new EventHandler<MessageEventArgs>(ServerMessageReceived);
            client.Connected += new EventHandler(ClientConnected);
            client.Disconnected += new EventHandler(ClientDisconnected);

            // Set up the messenger
            messenger = new RequestReplyMessenger<IScsClient>(client);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// A WBXML message has been received from the server.
        /// This method will translate the message to XML and fire the MessageReceived event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServerMessageReceived(object sender, MessageEventArgs e)
        {
            ScsRawDataMessage message = e.Message as ScsRawDataMessage;

            // Only accept Raw data and only when proper events are hooked
            if (message == null || MessageReceived == null)
                return;

            using (MemoryStream readStream = new MemoryStream(message.MessageData))
            {
                // Fire event
                MessageReceived(this, new ServerMessageEventArgs(Utils.ConvertWbxmlToXml(readStream)));
            }
        }
        /// <summary>
        /// The client has successfully connected to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClientConnected(object sender, EventArgs e)
        {
            if (Connected != null)
            {
                // Fire event
                Connected(sender, e);
            }
        }
        /// <summary>
        /// The client has been disconnected from the server, either by choice or because the server went away.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClientDisconnected(object sender, EventArgs e)
        {
            if (Disconnected != null)
            {
                // Fire event
                Disconnected(sender, e);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect()
        {
            messenger.Start();                
            client.Connect();                  
        }
        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            messenger.Stop();                
            client.Disconnect();
        }
        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="xml">The XML that will be translated to WBXML before being sent to the server</param>
        public void SendMessage(string xml)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Convert the XML to WBXML
                RootElement element = Utils.ConvertXmlToWbxml(xml);
                EComMessage outgoingMessage = EComMessage.CreateMessageFromElement(element);
                outgoingMessage.WriteToStream(stream);

                // Sent the WBXML to the server
                messenger.SendMessage(new ScsRawDataMessage(stream.ToArray()));
            }
        }
        #endregion
    }    
}
