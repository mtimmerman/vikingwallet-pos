using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Configuration;
using System.Net;

namespace VikingWalletPOS
{
    public class ClientService
    {
        const int NUM_OF_THREADS = 10;

        private ClientConnectionPool connectionPool;
        private bool continueProcess = false;
        private Thread[] threadTask = new Thread[NUM_OF_THREADS];
        private Thread listenThread;

        public ClientService(ClientConnectionPool connectionPool)
        {
            this.connectionPool = connectionPool;
            listenThread = new Thread(new ThreadStart(Listen));
        }

        public event EventHandler<LogEventArgs> Logged;

        void OnLogged(string message, params object[] args)
        {
            if (Logged != null)
            {
                Logged(this, new LogEventArgs(string.Format(message, args)));
            }
        }

        public void Start()
        {
            continueProcess = true;

            for (int i = 0; i < threadTask.Length; i++)
            {
                threadTask[i] = new Thread(new ThreadStart(this.Process));
                threadTask[i].Start();
            }

            listenThread = new Thread(new ThreadStart(Listen));
            listenThread.Start();
        }

        public void Listen()
        {
            TcpListener listener = new TcpListener(IPAddress.Any,
                Convert.ToInt32(ConfigurationManager.AppSettings["tcpPort"]));
            try
            {
                listener.Start();
                int clientNr = 0;
                OnLogged("Waiting for a connection...");
                while (continueProcess)
                {
                    if (listener.Pending())
                    {
                        TcpClient handler = listener.AcceptTcpClient();

                        if (handler != null)
                        {
                            OnLogged("Client #{0} accepted", ++clientNr);

                            ClientHandler client = new ClientHandler(handler);
                            client.Logged += Logged;
                            connectionPool.Enqueue(client);
                        }
                    }

                    Thread.Sleep(100);
                }                
            }
            finally
            {
                listener.Stop();                
            }
        }

        private void Process()
        {
            while (continueProcess)
            {
                ClientHandler client = null;

                lock (connectionPool.SyncRoot)
                {
                    if (connectionPool.Count > 0)
                    {
                        client = connectionPool.Dequeue();
                    }
                }

                if (client != null)
                {
                    client.Process();

                    if (client.Alive)
                    {
                        connectionPool.Enqueue(client);
                    }

                    Thread.Sleep(100);
                }
            }
        }

        public void Stop()
        {
            continueProcess = false;

            if (listenThread.IsAlive)
            {
                listenThread.Abort();
            }

            for (int i = 0; i < threadTask.Length; i++)
            {
                if (threadTask[i] != null && threadTask[i].IsAlive)
                {
                    threadTask[i].Join();
                }
            }

            while (connectionPool.Count > 0)
            {
                ClientHandler client = connectionPool.Dequeue();
                client.Close();
            }
            OnLogged("Stopping server...");
        }
    }
}
