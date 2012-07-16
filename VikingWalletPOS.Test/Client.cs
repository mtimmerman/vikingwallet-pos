using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.Messengers;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using System.IO;
using Comtech.Wbxml;
using Comtech;

namespace VikingWalletPOS.Test
{
    public delegate void ServerMessageDelegate(string response);

    public class Client
    {
        private IScsClient client;
        private SynchronizedMessenger<IScsClient> synchronizedMessenger;
        public event ServerMessageDelegate MessageReceived;
        public event EventHandler Connected;
        public event EventHandler Disconnected;        

        public Client()
        {
            client = ScsClientFactory.CreateClient(new ScsTcpEndPoint("127.0.0.1", 10085));
            client.MessageReceived += new EventHandler<MessageEventArgs>(ServerMessageReceived);
            client.Connected += new EventHandler(ClientConnected);
            client.Disconnected += new EventHandler(ClientDisconnected);
            synchronizedMessenger = new SynchronizedMessenger<IScsClient>(client);
        }

        void ServerMessageReceived(object sender, MessageEventArgs e)
        {
            ScsRawDataMessage message = e.Message as ScsRawDataMessage;
            if (message == null)
                return;

            using (MemoryStream readStream = new MemoryStream(message.MessageData))
            {
                string text = Encoding.GetEncoding("iso-8859-1").GetString(message.MessageData);

                if (MessageReceived != null)
                    MessageReceived(Utils.ConvertWbxmlToXml(readStream).ToString());
            }
        }

        void ClientConnected(object sender, EventArgs e)
        {
            if (Connected != null)
            {
                Connected(sender, e);
            }
        }

        void ClientDisconnected(object sender, EventArgs e)
        {
            if (Disconnected != null)
            {
                Disconnected(sender, e);
            }
        }

        public void Connect()
        {
            synchronizedMessenger.Start();                
            client.Connect();                  
        }

        public void Disconnect()
        {
            synchronizedMessenger.Stop();                
            client.Disconnect();
        }

        public void SendMessage(string xml)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                RootElement element = Utils.ConvertXmlToWbxml(xml);
                EComMessage outgoingMessage = EComMessage.CreateMessageFromElement(element);
                outgoingMessage.WriteToStream(stream);

                synchronizedMessenger.SendMessage(new ScsRawDataMessage(stream.ToArray()));
            }
        }
    }
}
