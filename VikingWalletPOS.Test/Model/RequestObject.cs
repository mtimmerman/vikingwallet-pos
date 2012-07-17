using System.Reflection;

namespace VikingWalletPOS.Model
{
    /// <summary>
    /// Abstract class for Viking Spots API requests
    /// </summary>
    public abstract class RequestObject
    {
        /// <summary>
        /// Convert the properties of this class to a querystring
        /// </summary>
        /// <returns>A querystring representation of the properties of this class</returns>
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
