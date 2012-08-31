#define LOGEVENTS

using System;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using VikingWalletPOS;

namespace VikingWalletPOS.Service
{
    // Define custom commands for the POS.
    public enum POSCustomCommands { StopWorker = 128, RestartWorker, CheckWorker };
    [StructLayout(LayoutKind.Sequential)]
    public struct SERVICE_STATUS
    {
        public int serviceType;
        public int currentState;
        public int controlsAccepted;
        public int win32ExitCode;
        public int serviceSpecificExitCode;
        public int checkPoint;
        public int waitHint;
    }

    public enum State
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    // Define a simple service implementation.
    public class POS : System.ServiceProcess.ServiceBase
    {
        private static int userCount = 0;
        private static ManualResetEvent pause = new ManualResetEvent(false);
        private Server server;

        [DllImport("ADVAPI32.DLL", EntryPoint = "SetServiceStatus")]
        public static extern bool SetServiceStatus(
                        IntPtr hServiceStatus,
                        SERVICE_STATUS lpServiceStatus
                        );
        private SERVICE_STATUS myServiceStatus;

        private Thread workerThread = null;

        public POS()
        {
            CanPauseAndContinue = true;
            CanHandleSessionChangeEvent = true;
            ServiceName = "POS";
            server = new Server();
            server.Logged += new EventHandler<LogEventArgs>(server_Logged);
        }

        void server_Logged(object sender, LogEventArgs e)
        {
            EventLog.WriteEntry(e.Message, EventLogEntryType.Information);
        }

        static void Main()
        {
#if LOGEVENTS
            EventLog.WriteEntry("POS.Main", DateTime.Now.ToLongTimeString() +
                " - Service main method starting...");
#endif

            // Load the service into memory.
            System.ServiceProcess.ServiceBase.Run(new POS());

#if LOGEVENTS
            EventLog.WriteEntry("POS.Main", DateTime.Now.ToLongTimeString() +
                " - Service main method exiting...");
#endif

        }

        private void InitializeComponent()
        {
            // Initialize the operating properties for the service.
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanHandleSessionChangeEvent = true;
            this.ServiceName = "POS";
        }

        // Start the service.
        protected override void OnStart(string[] args)
        {
            IntPtr handle = this.ServiceHandle;
            myServiceStatus.currentState = (int)State.SERVICE_START_PENDING;
            SetServiceStatus(handle, myServiceStatus);

            // Start a separate thread that does the actual work.

            if ((workerThread == null) ||
                ((workerThread.ThreadState &
                 (System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Stopped)) != 0))
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnStart", DateTime.Now.ToLongTimeString() +
                    " - Starting the service worker thread.");
#endif

                server.Start();                
                
                workerThread = new Thread(new ThreadStart(ServiceWorkerMethod));
                workerThread.Start();
            }
            if (workerThread != null)
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnStart", DateTime.Now.ToLongTimeString() +
                    " - Worker thread state = " +
                    workerThread.ThreadState.ToString());
#endif
            }
            myServiceStatus.currentState = (int)State.SERVICE_RUNNING;
            SetServiceStatus(handle, myServiceStatus);

        }

        // Stop this service.
        protected override void OnStop()
        {
            // New in .NET Framework version 2.0.
            this.RequestAdditionalTime(4000);
            // Signal the worker thread to exit.
            if ((workerThread != null) && (workerThread.IsAlive))
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnStop", DateTime.Now.ToLongTimeString() +
                    " - Stopping the service worker thread.");
#endif
                pause.Reset();
                Thread.Sleep(5000);
                workerThread.Abort();

                //server.Stop();
            }
            if (workerThread != null)
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnStop", DateTime.Now.ToLongTimeString() +
                    " - OnStop Worker thread state = " +
                    workerThread.ThreadState.ToString());
#endif
            }
            // Indicate a successful exit.
            this.ExitCode = 0;
        }

        // Pause the service.
        protected override void OnPause()
        {
            // Pause the worker thread.
            if ((workerThread != null) &&
                (workerThread.IsAlive) &&
                ((workerThread.ThreadState &
                 (System.Threading.ThreadState.Suspended | System.Threading.ThreadState.SuspendRequested)) == 0))
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnPause", DateTime.Now.ToLongTimeString() +
                    " - Pausing the service worker thread.");
#endif

                pause.Reset();
                Thread.Sleep(5000);
            }

            if (workerThread != null)
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnPause", DateTime.Now.ToLongTimeString() +
                    " OnPause - Worker thread state = " +
                    workerThread.ThreadState.ToString());
#endif
            }
        }

        // Continue a paused service.
        protected override void OnContinue()
        {

            // Signal the worker thread to continue.
            if ((workerThread != null) &&
                ((workerThread.ThreadState &
                 (System.Threading.ThreadState.Suspended | System.Threading.ThreadState.SuspendRequested)) != 0))
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnContinue", DateTime.Now.ToLongTimeString() +
                    " - Resuming the service worker thread.");

#endif
                pause.Set();
            }
            if (workerThread != null)
            {
#if LOGEVENTS
                EventLog.WriteEntry("POS.OnContinue", DateTime.Now.ToLongTimeString() +
                    " OnContinue - Worker thread state = " +
                    workerThread.ThreadState.ToString());
#endif
            }
        }

        // Handle a custom command.
        protected override void OnCustomCommand(int command)
        {
#if LOGEVENTS
            EventLog.WriteEntry("POS.OnCustomCommand", DateTime.Now.ToLongTimeString() +
                " - Custom command received: " +
                command.ToString());
#endif

            // If the custom command is recognized,
            // signal the worker thread appropriately.

            switch (command)
            {
                case (int)POSCustomCommands.StopWorker:
                    // Signal the worker thread to terminate.
                    // For this custom command, the main service
                    // continues to run without a worker thread.
                    OnStop();
                    break;

                case (int)POSCustomCommands.RestartWorker:

                    // Restart the worker thread if necessary.
                    OnStart(null);
                    break;

                case (int)POSCustomCommands.CheckWorker:
#if LOGEVENTS
                    // Log the current worker thread state.
                    EventLog.WriteEntry("POS.OnCustomCommand", DateTime.Now.ToLongTimeString() +
                        " OnCustomCommand - Worker thread state = " +
                        workerThread.ThreadState.ToString());
#endif

                    break;

                default:
#if LOGEVENTS
                    EventLog.WriteEntry("POS.OnCustomCommand",
                        DateTime.Now.ToLongTimeString());
#endif
                    break;
            }
        }
        // Handle a session change notice
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
#if LOGEVENTS
            EventLog.WriteEntry("POS.OnSessionChange", DateTime.Now.ToLongTimeString() +
                " - Session change notice received: " +
                changeDescription.Reason.ToString() + "  Session ID: " +
                changeDescription.SessionId.ToString());
#endif

            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    userCount += 1;
#if LOGEVENTS
                    EventLog.WriteEntry("POS.OnSessionChange",
                        DateTime.Now.ToLongTimeString() +
                        " SessionLogon, total users: " +
                        userCount.ToString());
#endif
                    break;

                case SessionChangeReason.SessionLogoff:

                    userCount -= 1;
#if LOGEVENTS
                    EventLog.WriteEntry("POS.OnSessionChange",
                        DateTime.Now.ToLongTimeString() +
                        " SessionLogoff, total users: " +
                        userCount.ToString());
#endif
                    break;
                case SessionChangeReason.RemoteConnect:
                    userCount += 1;
#if LOGEVENTS
                    EventLog.WriteEntry("POS.OnSessionChange",
                        DateTime.Now.ToLongTimeString() +
                        " RemoteConnect, total users: " +
                        userCount.ToString());
#endif
                    break;

                case SessionChangeReason.RemoteDisconnect:

                    userCount -= 1;
#if LOGEVENTS
                    EventLog.WriteEntry("POS.OnSessionChange",
                        DateTime.Now.ToLongTimeString() +
                        " RemoteDisconnect, total users: " +
                        userCount.ToString());
#endif
                    break;
                case SessionChangeReason.SessionLock:
#if LOGEVENTS
                    EventLog.WriteEntry("POS.OnSessionChange",
                        DateTime.Now.ToLongTimeString() +
                        " SessionLock");
#endif
                    break;

                case SessionChangeReason.SessionUnlock:
#if LOGEVENTS
                    EventLog.WriteEntry("POS.OnSessionChange",
                        DateTime.Now.ToLongTimeString() +
                        " SessionUnlock");
#endif
                    break;

                default:

                    break;
            }
        }
        // Define a simple method that runs as the worker thread for 
        // the service.  
        public void ServiceWorkerMethod()
        {
#if LOGEVENTS
            EventLog.WriteEntry("POS.WorkerThread", DateTime.Now.ToLongTimeString() +
                " - Starting the service worker thread.");
#endif

            try
            {
                do
                {
                    // Simulate 4 seconds of work.
                    Thread.Sleep(4000);
                    // Block if the service is paused or is shutting down.
                    pause.WaitOne();
#if LOGEVENTS
                    EventLog.WriteEntry("POS.WorkerThread", DateTime.Now.ToLongTimeString() +
                        " - heartbeat cycle.");
#endif
                }
                while (true);
            }
            catch (ThreadAbortException)
            {
                // Another thread has signalled that this worker
                // thread must terminate.  Typically, this occurs when
                // the main service thread receives a service stop 
                // command.

                // Write a trace line indicating that the worker thread
                // is exiting.  Notice that this simple thread does
                // not have any local objects or data to clean up.
#if LOGEVENTS
                EventLog.WriteEntry("POS.WorkerThread", DateTime.Now.ToLongTimeString() +
                    " - Thread abort signaled.");
#endif
            }
#if LOGEVENTS

            EventLog.WriteEntry("POS.WorkerThread", DateTime.Now.ToLongTimeString() +
                " - Exiting the service worker thread.");
#endif

        }
    }
}