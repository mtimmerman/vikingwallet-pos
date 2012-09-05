using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VikingWalletPOS
{
    public class ClientConnectionPool
    {
        private Queue syncdQ = Queue.Synchronized(new Queue());

        public void Enqueue(ClientHandler client)
        {
            syncdQ.Enqueue(client);
        }

        public ClientHandler Dequeue()
        {
            return (ClientHandler)syncdQ.Dequeue();
        }

        public int Count
        {
            get
            {
                return syncdQ.Count;
            }
        }

        public object SyncRoot
        {
            get
            {
                return syncdQ.SyncRoot;
            }
        }
    }
}
