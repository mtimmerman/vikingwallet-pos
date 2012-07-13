using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Comtech.Wbxml
{
	public class RootElement : Element
	{
		public static RootElement ReadFromStreamWithHeader(Stream s, Encoding encoding)
		{
			ReadHeader(s);
			return (RootElement)ReadFromStream(s, encoding, new RootElement());
		}

		private static void ReadHeader(Stream s)
		{
			byte[] buffer = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				buffer[i] = ReadByte(s, "WBXML header");
			}
			int pos = 0;
			bool ok =
				(buffer[pos++] == 0x03) &&
				(buffer[pos++] == 0x01) &&
				(buffer[pos++] == 0x03) &&
				(buffer[pos++] == 0x00);
			if (!ok)
				throw new ApplicationException("invalid WBXML header: expected 03 01 03 00, received " + Utils.HexHelper.ToHexString(buffer));
		}

		public static new RootElement ReadFromXml(XmlElement xmlElement)
		{
			return (RootElement)ReadFromXml(xmlElement, new RootElement());
		}

		//public static RootElement Clone(Element element)
		//{
		//    MemoryStream ms = new MemoryStream();
		//    WbxmlWriter w = new WbxmlWriter(ms);
		//    w.WriteElement(element);
		//    ms.Position = 0;
		//    return (RootElement)ReadFromStream(ms, new RootElement());
		//}

		public static string ConvertWbxmlBytesToXmlText(byte[] bytes, Encoding encoding)
		{
			return ConvertWbxmlStreamToXmlText(new MemoryStream(bytes), encoding);
		}

		public static string ConvertWbxmlStreamToXmlText(Stream s, Encoding encoding)
		{
			// read and create XML
			RootElement root = ReadFromStreamWithHeader(s, encoding);
			XmlDocument doc = new XmlDocument();
			XmlElement docRoot = root.WriteToXml(doc);
			doc.AppendChild(docRoot);
			// write XML to text
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.OmitXmlDeclaration = true;
			StringBuilder sb = new StringBuilder();
			using (XmlWriter xw = XmlWriter.Create(sb, settings))
			{
				doc.WriteTo(xw);
			}
			return sb.ToString();
		}
	}
}
