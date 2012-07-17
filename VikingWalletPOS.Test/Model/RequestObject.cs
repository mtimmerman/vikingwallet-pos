using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;

namespace VikingWalletPOS.Test.Model
{
    public class RequestObject
    {
        private JsonSerializerSettings settings;

        public RequestObject()
        {
            settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
        }
        
        public string ToQueryString()
        {
            string result = "";            

            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                object val = p.GetValue(this, null);

                if (val != null)
                {
                    if (result.Length == 0)
                        result = string.Format("{0}={1}", p.Name, val);
                    else
                        result += string.Format("&{0}={1}", p.Name, val);
                }
            }

            return result;
        }
    }
}
