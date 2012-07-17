using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingWalletPOS.Test.Model
{
    public class POSRedeemRequest : RequestObject
    {
        public int merchant_id { get; set; }
        public int coupon_id { get; set; }
        public string terminal_id { get; set; }

        public POSRedeemRequest(int merchant_id, int coupon_id, string terminal_id)
            : base()
        {
            this.merchant_id = merchant_id;
            this.coupon_id = coupon_id;
            this.terminal_id = terminal_id;
        }
    }

    public class POSRedeemResult : ResultObject
    {
    }
}
