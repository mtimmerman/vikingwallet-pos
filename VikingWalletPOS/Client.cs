using System;
using System.Configuration;
using System.IO;
using Comtech;
using Comtech.Wbxml;
using System.Text;
using System.Net.Sockets;

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
        private TcpClient client;
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

        #region Event Handlers        
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
            try
            {
                client = new TcpClient();
                client.Connect(ConfigurationManager.AppSettings["tcpServer"],
                    Convert.ToInt32(ConfigurationManager.AppSettings["tcpPort"]));

                if (Connected != null)
                {
                    Connected(this, EventArgs.Empty);
                }
            }
            catch (SocketException)
            {                
            }
        }
        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            if (client != null && client.Connected)
            {
                client.Close();

                if (Disconnected != null)
                {
                    // Fire event
                    Disconnected(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="xml">The XML that will be translated to WBXML before being sent to the server</param>
        public void SendMessage(string xml)
        {
            // Convert the XML to WBXML
            RootElement element = Utils.ConvertXmlToWbxml(xml);
            EComMessage outgoingMessage = EComMessage.CreateMessageFromElement(element);
            NetworkStream stream = client.GetStream();
            outgoingMessage.WriteToStream(stream);

            int readCount = 0;
            byte[] data = new byte[client.ReceiveBufferSize];
            using (MemoryStream responseStream = new MemoryStream())
            {
                readCount = stream.Read(data, 0, client.ReceiveBufferSize);

                do
                {
                    if (readCount > 0)
                    {
                        responseStream.Write(data, 0, readCount);
                    }
                } while (readCount == client.ReceiveBufferSize && (readCount = stream.Read(data, 0, client.ReceiveBufferSize)) != 0);

                responseStream.Seek(0, SeekOrigin.Begin);
                MessageReceived(this, new ServerMessageEventArgs(Utils.ConvertWbxmlToXml(responseStream)));
            }                                       
        }
        #endregion
    }    
}
