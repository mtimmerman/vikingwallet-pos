using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Comtech.Wbxml;
using Comtech;

namespace VikingWalletPOS.Test
{
    internal static class Utils
    {
        public static string ConvertWbxmlToXml(Stream s)
        {
            EComMessage incomingMessage = EComMessage.ReadFromStream(s);
            string xml = incomingMessage.RootElement.ToXmlString();
            return xml;
        }

        public static RootElement ConvertXmlToWbxml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            RootElement root = RootElement.ReadFromXml(doc.DocumentElement);
            return root;
        }
    }
}
