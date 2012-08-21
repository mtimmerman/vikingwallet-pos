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

    class POS : ServiceBase
    {
        private static int userCount = 0;
        
        [DllImport("ADVAPI32.DLL", EntryPoint = "SetServiceStatus")]
        public static extern bool SetServiceStatus(
                        IntPtr hServiceStatus,
                        SERVICE_STATUS lpServiceStatus
                        );
        private SERVICE_STATUS myServiceStatus;

        private Server server;
        
        public POS()
        {
            this.CanPauseAndContinue = false;
            this.CanHandleSessionChangeEvent = false;
            this.ServiceName = "Viking Wallet PoS Service";
        }

        static void Main()
        {
#if LOGEVENTS
            EventLog.WriteEntry("POS.Main", DateTime.Now.ToLongTimeString() +
                " - Service main method starting...");
#endif

            ServiceBase.Run(new POS());

#if LOGEVENTS
            EventLog.WriteEntry("POS.Main", DateTime.Now.ToLongTimeString() +
                " - Service main method exiting...");
#endif
        }

        private void InitializeComponent()
        {
            // Initialize the operating properties for the service.
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanHandleSessionChangeEvent = false;
            this.ServiceName = "Viking Wallet PoS Service";
            this.server = new Server();
        }

        // Start the service.
        protected override void OnStart(string[] args)
        {
            IntPtr handle = this.ServiceHandle;
            myServiceStatus.currentState = (int)State.SERVICE_START_PENDING;
            SetServiceStatus(handle, myServiceStatus);

            // Start a separate thread that does the actual work.

            server.Start();
            
            myServiceStatus.currentState = (int)State.SERVICE_RUNNING;
            SetServiceStatus(handle, myServiceStatus);                  
        }

        // Stop this service.
        protected override void OnStop()
        {
            // New in .NET Framework version 2.0.
            this.RequestAdditionalTime(4000);

            server.Stop();

            // Indicate a successful exit.
            this.ExitCode = 0;
        }

        
    }
}
