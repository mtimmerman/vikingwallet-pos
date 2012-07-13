using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingWalletPOS.Test.Model;
using RestSharp;
using Hik.Communication.Scs.Server;

namespace VikingWalletPOS.Test
{
    public delegate void ResponseArgs(object sender, string response, IScsServerClient client);

    public class API
    {
        private RestClient apiClient;
        private string authHeader = "";

        public event ResponseArgs CouponListReceived;
        private void OnCouponListReceived(string response, IScsServerClient client)
        {
            if (CouponListReceived != null)
            {
                CouponListReceived(this, response, client);
            }
        }
        public API()
        {
            apiClient = new RestClient("https://vikingspots.com/en/api/3/");
            authHeader = "Basic bXRpbW1lcm1hbjpjYTk2MDMwNmRlOTgyOGJlNWE0M2FjMWMxNGFkODZhYmYwMmEzZDcx";            
        }

        public void GetCoupon(GetPOSCouponRequest req, IScsServerClient client)
        {
            RestRequest request = new RestRequest(string.Format("poscoupon?{0}", req.ToQueryString()), Method.GET);
            
            request.AddHeader("Authorization", authHeader);

            apiClient.GetAsync(request, (response, handle) =>
            {
                OnCouponListReceived(response.Content, client);
            });
        }
    }
}
