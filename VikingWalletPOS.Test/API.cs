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
        public void GetCouponAsync(GetPOSCouponRequest req, Action<GetPOSCouponResult, HttpStatusCode> callback)
        {
            // This is a GET
            RestRequest request = new RestRequest(string.Format("poscoupon/?{0}", req.ToQueryString()), Method.GET);            
            
            apiClient.GetAsync(request, (response, handle) =>
            {
                // Check that we received a status we expect
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.BadRequest ||
                    response.StatusCode == HttpStatusCode.InternalServerError ||
                    response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    // Deserialize JSON into a manageable object and do the callback
                    callback(JsonConvert.DeserializeObject<GetPOSCouponResult>(response.Content), response.StatusCode);
                }
                else
                {
                    // Unexpected status code found. We can't deserialize the response so we send back null with the callback
                    callback(null, response.StatusCode);
                }
            });
        }
        /// <summary>
        /// Redeem a coupon
        /// </summary>
        /// <param name="req">Request parameters to do the request to Viking Spots</param>
        /// <param name="callback">Delegate that handles the callback</param>
        public void RedeemAsync(POSRedeemRequest req, Action<POSRedeemResult, HttpStatusCode> callback)
        {
            // This is a POST
            RestRequest request = new RestRequest(string.Format("posredeemcoupon/"), Method.POST);

            // We're posting a JSON structure in the body.
            request.RequestFormat = DataFormat.Json;
            // Note, RestSharp automatically serializes classes into JSON
            request.AddBody(req);

            apiClient.PostAsync(request, (response, handle) =>
            {
                // Check that we received a status we expect
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.BadRequest ||
                    response.StatusCode == HttpStatusCode.InternalServerError ||
                    response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    // Deserialize JSON into a manageable object and do the callback
                    callback(JsonConvert.DeserializeObject<POSRedeemResult>(response.Content), response.StatusCode);
                }
                else
                {
                    // Unexpected status code found. We can't deserialize the response so we send back null with the callback
                    callback(null, response.StatusCode);
                }
            });
        }
        /// <summary>
        /// Acknowledge a payment
        /// </summary>
        /// <param name="req">Request parameters to do the request to Viking Spots</param>
        /// <param name="callback">Delegate that handles the callback</param>
        public void PaymentAcknowledgeAsync(POSPaymentAcknowledgeRequest req, Action<POSPaymentAcknowledgeResult, HttpStatusCode> callback)
        {
            // This is a POST
            RestRequest request = new RestRequest(string.Format("posacknowledgepayment/"), Method.POST);

            // We're posting a JSON structure in the body.
            request.RequestFormat = DataFormat.Json;
            // Note, RestSharp automatically serializes classes into JSON
            request.AddBody(req);

            apiClient.PostAsync(request, (response, handle) =>
            {
                // Check that we received a status we expect
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.BadRequest ||
                    response.StatusCode == HttpStatusCode.InternalServerError ||
                    response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    // Deserialize JSON into a manageable object and do the callback
                    callback(JsonConvert.DeserializeObject<POSPaymentAcknowledgeResult>(response.Content), response.StatusCode);
                }
                else
                {
                    // Unexpected status code found. We can't deserialize the response so we send back null with the callback
                    callback(null, response.StatusCode);
                }
            });
        }
        #endregion
    }    
}
