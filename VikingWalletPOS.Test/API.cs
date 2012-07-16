using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingWalletPOS.Test.Model;
using RestSharp;
using Hik.Communication.Scs.Server;
using Newtonsoft.Json;
using Hik.Communication.Scs.Communication.Messages;
using System.Threading.Tasks;

namespace VikingWalletPOS.Test
{
    public class API
    {
        #region Private Members
        private RestClient apiClient;
        #endregion        

        #region Constructor
        public API()
        {
            apiClient = new RestClient("http://beta.vikingspots.com/en/api/3/");
            apiClient.Authenticator = new HttpBasicAuthenticator("mtimmerman", "ca960306de9828be5a43ac1c14ad86abf02a3d71");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the coupon list
        /// </summary>
        /// <param name="req">Request parameters needed to do the request to Viking Spots</param>
        /// <param name="callback">Delegate that handles the callback</param>
        public void GetCouponAsync(GetPOSCouponRequest req, Action<GetPOSCouponResult> callback)
        {
            RestRequest request = new RestRequest(string.Format("poscoupon/?{0}", req.ToQueryString()), Method.GET);
            
            apiClient.GetAsync(request, (response, handle) =>
            {
                callback(JsonConvert.DeserializeObject<GetPOSCouponResult>(response.Content));                
            });
        }
        #endregion
    }    
}
