using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingWalletPOS.Test.Model;
using RestSharp;
using Hik.Communication.Scs.Server;
using Newtonsoft.Json;
using Hik.Communication.Scs.Communication.Messages;

namespace VikingWalletPOS.Test
{
    public delegate void ResponseArgs(object sender, ResultObject data, IScsServerClient client, ScsMessage message);

    public class API
    {
        private RestClient apiClient;
        private string authHeader = "";

        public event ResponseArgs CouponListReceived;
        private void OnCouponListReceived(GetPOSCouponResult data, IScsServerClient client, ScsMessage message)
        {
            if (CouponListReceived != null)
            {
                CouponListReceived(this, data, client, message);
            }
        }
        public API()
        {
            apiClient = new RestClient("http://beta.vikingspots.com/en/api/3/");
            apiClient.Authenticator = new HttpBasicAuthenticator("mtimmerman", "ca960306de9828be5a43ac1c14ad86abf02a3d71");
        }

        public void GetCoupon(GetPOSCouponRequest req, IScsServerClient client, ScsMessage message)
        {
            RestRequest request = new RestRequest(string.Format("poscoupon/?{0}", req.ToQueryString()), Method.GET);
            
            apiClient.GetAsync(request, (response, handle) =>
            {
                GetPOSCouponResult data = JsonConvert.DeserializeObject<GetPOSCouponResult>(response.Content);
                OnCouponListReceived(data, client, message);
            });
        }
    }
}
