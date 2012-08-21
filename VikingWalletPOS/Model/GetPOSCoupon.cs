namespace VikingWalletPOS.Model
{
    #region GetPOSCouponRequest
    /// <summary>
    /// Used to make the request poscoupon
    /// </summary>
    public class GetPOSCouponRequest : RequestObject
    {
        #region Public Properties
        /// <summary>
        /// Id of the merchant
        /// </summary>
        public int merchant_id { get; set; }
        /// <summary>
        /// The card PAN of the user
        /// </summary>
        public string card_pan { get; set; }
        /// <summary>
        /// The terminal id of the spot
        /// </summary>
        public string terminal_id { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of <see cref="GetPOSCouponRequest"/>
        /// </summary>
        /// <param name="merchant_id">Id of the merchant</param>
        /// <param name="card_pan">The card PAN of the user</param>
        /// <param name="terminal_id">The terminal id of the spot</param>
        public GetPOSCouponRequest(int merchant_id, string card_pan, string terminal_id)
            : base()
        {
            this.merchant_id = merchant_id;
            this.card_pan = card_pan;
            this.terminal_id = terminal_id;
        }
        #endregion
    }
    #endregion

    #region GetPOSCouponResult
    /// <summary>
    /// Result wrapper for poscoupon
    /// </summary>
    public class GetPOSCouponResult : ResultObject
    {
        /// <summary>
        /// The response field
        /// </summary>
        public new GetPOSResponse response { get; set; }
    }
    #endregion

    #region GetPOSResponse
    /// <summary>
    /// Data structure of the response field of <see cref="GetPOSCouponResult"/>
    /// </summary>
    public class GetPOSResponse
    {
        /// <summary>
        /// A list of <see cref="POSCoupon"/>
        /// </summary>
        public POSCoupon[] coupons { get; set; }
    }
    #endregion

    #region POSCoupon
    /// <summary>
    /// Contains information about a coupon
    /// </summary>
    public class POSCoupon
    {
        /// <summary>
        /// Owner of the coupon
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// Id of the coupon
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Name of the deal
        /// </summary>
        public string name { get; set; }
    }
    #endregion
}
