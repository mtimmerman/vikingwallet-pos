namespace VikingWalletPOS.Model
{
    #region POSRedeemRequest
    /// <summary>
    /// Used to redeem a coupon using posredeemcoupon
    /// </summary>
    public class POSRedeemRequest : RequestObject
    {
        #region Public Properties
        /// <summary>
        /// The Id of the merchant
        /// </summary>
        public int merchant_id { get; set; }
        /// <summary>
        /// The Id of the coupon
        /// </summary>
        public int coupon_id { get; set; }
        /// <summary>
        /// The terminal id of the spot
        /// </summary>
        public string terminal_id { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of <see cref="POSRedeemRequest"/>
        /// </summary>
        /// <param name="merchant_id">The Id of the merchant</param>
        /// <param name="coupon_id">The Id of the coupon</param>
        /// <param name="terminal_id">The terminal id of the spot</param>
        public POSRedeemRequest(int merchant_id, int coupon_id, string terminal_id)
            : base()
        {
            this.merchant_id = merchant_id;
            this.coupon_id = coupon_id;
            this.terminal_id = terminal_id;
        }
        #endregion
    }
    #endregion

    #region POSRedeemResult
    /// <summary>
    /// Result wrapper of posredeemcoupoon 
    /// </summary>
    public class POSRedeemResult : ResultObject
    {
    }
    #endregion
}
