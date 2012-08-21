using System.IO;
using System.Xml;
using Comtech;
using Comtech.Wbxml;

namespace VikingWalletPOS
{
    /// <summary>
    /// Helper class with utilities
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Convert WBXML to XML
        /// </summary>
        /// <param name="s">A stream containing WBXML</param>
        /// <returns></returns>
        public static string ConvertWbxmlToXml(Stream s)
        {
            EComMessage incomingMessage = EComMessage.ReadFromStream(s);
            string xml = incomingMessage.RootElement.ToXmlString();
            return xml;
        }
        /// <summary>
        /// Convert XML to WBXML
        /// </summary>
        /// <param name="xml">xml to be converted</param>
        /// <returns></returns>
        public static RootElement ConvertXmlToWbxml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            RootElement root = RootElement.ReadFromXml(doc.DocumentElement);
            return root;
        }
    }
}
