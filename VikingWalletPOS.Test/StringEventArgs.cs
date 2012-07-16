using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingWalletPOS.Test
{
    public class StringEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string MessageId { get; set; }

        public StringEventArgs(string message, string messageId="")
        {
            this.Message = message;
            this.MessageId = messageId;
        }
    }
}
