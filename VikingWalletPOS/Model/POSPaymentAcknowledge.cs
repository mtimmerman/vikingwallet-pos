using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingWalletPOS.Model
{    
    #region POSPaymentAcknowledgeRequest
    /// <summary>
    /// Used to acknowledge a payment using posacknowledgepayment
    /// </summary>
    public class POSPaymentAcknowledgeRequest : RequestObject
    {
        #region Public Properties
        /// <summary>
        /// The terminal id of the spot
        /// </summary>
        public string terminal_id { get; set; }
        /// <summary>
        /// The Id of the coupon
        /// </summary>
        public int coupon_id { get; set; }
        /// <summary>
        /// The Id of the merchant
        /// </summary>
        public int merchant_id { get; set; }
        /// <summary>
        /// The amount that was paid
        /// </summary>
        public double amount { get; set; }
        /// <summary>
        /// The card PAN of the user
        /// </summary>
        public string card_pan { get; set; }
        /// <summary>
        /// The payment type that was used
        /// </summary>
        public string payment_type { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of <see cref="POSPaymentAcknowledgeRequest"/>
        /// </summary>
        /// <param name="terminal_id">The terminal id of the spot</param>
        /// <param name="coupon_id">The Id of the coupon</param>
        /// <param name="merchant_id">The Id of the merchant</param>
        /// <param name="amount">The amount that was paid</param>
        /// <param name="card_pan">The card PAN of the user</param>
        public POSPaymentAcknowledgeRequest(string terminal_id, int coupon_id, int merchant_id, double amount, string card_pan, string payment_type)
        {
            this.terminal_id = terminal_id;
            this.coupon_id = coupon_id;
            this.merchant_id = merchant_id;
            this.amount = amount;
            this.card_pan = card_pan;
            this.payment_type = payment_type;
        }
        #endregion
    }
    #endregion    

    #region POSPaymentAcknowledgeResult
    /// <summary>
    /// Result wrapper of posacknowledgepayment
    /// </summary>
    public class POSPaymentAcknowledgeResult : ResultObject
    {
    }
    #endregion
}
