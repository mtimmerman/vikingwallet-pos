using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingWalletPOS.Test.Model
{
    public class GetPOSCouponRequest: RequestObject
    {
        public int merchant_id { get; set; }
        public string card_pan { get; set; }
        public string terminal_id { get; set; }

        public GetPOSCouponRequest(int merchant_id, string card_pan, string terminal_id) : base()
        {
            this.merchant_id = merchant_id;
            this.card_pan = card_pan;
            this.terminal_id = terminal_id;
        }
    }
}
