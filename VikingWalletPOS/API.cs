using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using VikingWalletPOS.Model;

namespace VikingWalletPOS
{
    /// <summary>
    /// Class that is used to contact the Viking Spots API
    /// </summary>
    public class API
    {
        #region Private Members
        private RestClient apiClient;
        #endregion        

        #region Constructor
        /// <summary>
        /// Create a new instance of <see cref="API"/>
        /// </summary>
        public API()
        {
            apiClient = new RestClient(ConfigurationManager.AppSettings["apiBaseUrl"]);
            apiClient.Authenticator = new HttpBasicAuthenticator(
                ConfigurationManager.AppSettings["apiUsername"], 
                ConfigurationManager.AppSettings["apiPassword"]);            
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the coupon list
        /// </summary>
        /// <param name="req">Request parameters needed to do the request to Viking Spots</param>
        /// <param name="callback">Delegate that handles the callback</param>
        public GetPOSCouponResult GetCoupon(GetPOSCouponRequest req, ref HttpStatusCode status)
        {
            // This is a GET
            RestRequest request = new RestRequest(string.Format("poscoupon/?{0}", req.ToQueryString()), Method.GET);

            IRestResponse response = apiClient.Get(request);

            status = response.StatusCode;

            // Check that we received a status we expect
            if (response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.InternalServerError ||
                response.StatusCode == HttpStatusCode.PaymentRequired)
            {
                return JsonConvert.DeserializeObject<GetPOSCouponResult>(response.Content);
            }

            return null;
        }
        /// <summary>
        /// Redeem a coupon
        /// </summary>
        /// <param name="req">Request parameters to do the request to Viking Spots</param>
        /// <param name="callback">Delegate that handles the callback</param>
        public POSRedeemResult Redeem(POSRedeemRequest req, ref HttpStatusCode code)
        {
            // This is a POST
            RestRequest request = new RestRequest(string.Format("posredeemcoupon/"), Method.POST);

            // We're posting a JSON structure in the body.
            request.RequestFormat = DataFormat.Json;

            // Note, RestSharp automatically serializes classes into JSON
            request.AddBody(req);

            IRestResponse response = apiClient.Post(request);

            code = response.StatusCode;

            // Check that we received a status we expect
            if (response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.InternalServerError ||
                response.StatusCode == HttpStatusCode.PaymentRequired)
            {
                // Deserialize JSON into a manageable object
                return JsonConvert.DeserializeObject<POSRedeemResult>(response.Content);
            }

            return null;            
        }
        /// <summary>
        /// Acknowledge a payment
        /// </summary>
        /// <param name="req">Request parameters to do the request to Viking Spots</param>
        /// <param name="callback">Delegate that handles the callback</param>
        public POSPaymentAcknowledgeResult PaymentAcknowledge(POSPaymentAcknowledgeRequest req, ref HttpStatusCode code)
        {
            // This is a POST
            RestRequest request = new RestRequest(string.Format("posacknowledgepayment/"), Method.POST);

            // We're posting a JSON structure in the body.
            request.RequestFormat = DataFormat.Json;
            // Note, RestSharp automatically serializes classes into JSON
            request.AddBody(req);

            IRestResponse response = apiClient.Post(request);
            code = response.StatusCode;

            // Check that we received a status we expect
            if (response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.InternalServerError ||
                response.StatusCode == HttpStatusCode.PaymentRequired)
            {
                // Deserialize JSON into a manageable object
                return JsonConvert.DeserializeObject<POSPaymentAcknowledgeResult>(response.Content);
            }

            return null;            
        }
        #endregion
    }    
}
