using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingWalletPOS.Test.Model
{
    public class POSRedeemRequest : RequestObject
    {
        public int merchant_id { get; set; }
        public int deal_id { get; set; }
        public string terminal_id { get; set; }

        public POSRedeemRequest(int merchant_id, int deal_id, string terminal_id)
            : base()
        {
            this.merchant_id = merchant_id;
            this.deal_id = deal_id;
            this.terminal_id = terminal_id;
        }
    }

    public class POSRedeemResult : ResultObject
    {
    }
}
