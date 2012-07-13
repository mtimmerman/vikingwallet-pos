using System;
using System.IO;
using System.Text;
using log4net;
using Comtech.Utils;
using Comtech.Wbxml;

namespace Comtech
{
	public abstract class Message
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public const string DateTimeFormat = "yyyyMMddHHmmss";

		private Tag type;

		private WbxmlWriter.ElementFlags flags;

		// authentication data for MAC calculations
		protected byte authType;
		private byte[] rawData; // received message body - incoming messages only
		private byte[] mac; // received MAC - incoming messages only
		// outgoing only
		private byte[] serialNo;
		private byte[] sapNo;
		private byte[] imsi;
		private byte[] imei;
		private string terminalId;

		#region Outgoing messages / writing
		public Message(Tag type, WbxmlWriter.ElementFlags flags)
		{
			this.type = type;
			this.flags = flags;
		}

		public void WriteToStream(Stream s)
		{
			// create the message body in a buffer
			MemoryStream ms = new MemoryStream();
			WbxmlWriter w = new WbxmlWriter(ms, AppConfig.TerminalEncoding);
			w.WriteHeader();
			w.WriteElement(type, flags);
			if ((flags & WbxmlWriter.ElementFlags.HasAttributes) != 0)
			{
				WriteAttributes(w);
				w.WriteTerminator();
			}
			if ((flags & WbxmlWriter.ElementFlags.HasChildren) != 0)
			{
				WriteChildElements(w);
				w.WriteTerminator();
			}

			byte[] buf = ms.ToArray();
			int len = buf.Length;

			// calculate MAC if needed
			if (authType > 0)
			{
				//mac = CalculateMac(authType, serialNo, sapNo, imsi, imei, buf, terminalId);
			}
			log.Debug("sending message: length: " + len + "\r\n" +
				HexHelper.ToHexDump(buf) +
				((authType > 0) ? "\r\n" + "MAC: " + HexHelper.ToHexString(mac) : ""));

			// write message into the TCP/IP stream (length + body + MAC if needed)
			BinaryWriter bw = new BinaryWriter(s);
			bw.Write(BinaryHelper.SwapByteOrder(len));
			bw.Write(buf);
			if (authType > 0)
			{
				bw.Write(mac);
			}
		}

		protected abstract void WriteAttributes(WbxmlWriter w);

		protected virtual void WriteChildElements(WbxmlWriter w)
		{
		}
		#endregion

		#region Incoming messages / reading
		protected Message(Tag type)
		{
			this.type = type;
		}

		protected static RootElement ReadElementFromStream(Stream s, out byte authType, out byte[] rawData, out byte[] mac)
		{
			log.Debug("receiving message");
			BinaryReader br = new BinaryReader(s);
			// read the header (length)
			int len = BinaryHelper.SwapByteOrder(br.ReadInt32());
			if (len < 5 || len > 1024 * 1024)
				throw new ApplicationException("invalid message length: " + len.ToString("X4"));
			// read the message
			byte[] body = br.ReadBytes(len);
			log.Debug("length: " + len + "\r\n" + HexHelper.ToHexDump(body, 0, len));
			WbxmlParser parser = new WbxmlParser(body, AppConfig.TerminalEncoding);
			RootElement root = parser.RootElement;
			// check the authentication type and read the MAC if auth != 0
			authType = 0;
			rawData = body;
			mac = null;
			string auth = "";
            //if (root.Attributes.TryGetValue(Comtech.Wbxml.Attribute.Aut, out auth))
            //{
            //    authType = Convert.ToByte(auth);
            //}
			if (authType > 0)
			{
				mac = br.ReadBytes(4);
			}
			log.Debug(root.ToXmlString());
			return root;
		}

		protected virtual void ConstructFromElement(Element element)
		{
			if (element.HasAttributes)
			{
				foreach (Comtech.Wbxml.Attribute attr in element.Attributes.Keys)
				{
					SetAttribute(attr, element.Attributes[attr]);
				}
			}
			if (element.HasChildren)
			{
				foreach (Element child in element.Children)
				{
					AddChildElement(child);
				}
			}
		}

		protected virtual void SetAttribute(Comtech.Wbxml.Attribute attr, string value)
		{
		}

		protected virtual void AddChildElement(Element element)
		{
		}
		#endregion

		#region Authentication
		public byte AuthType
		{
			get { return authType; }
		}

		public void SetOutgoingAuthData(byte authType, string serialNo, string sapNo, string imsi, string imei, string terminalId)
		{
			this.authType = authType;
			this.serialNo = Encoding.ASCII.GetBytes(serialNo ?? "");
			this.sapNo = Encoding.ASCII.GetBytes(sapNo ?? "");
			this.imsi = Encoding.ASCII.GetBytes(imsi ?? "");
			this.imei = Encoding.ASCII.GetBytes(imei ?? "");
			this.terminalId = terminalId;
		}

		public void SetIncomingAuthData(byte authType, byte[] rawData, byte[] mac)
		{
			this.authType = authType;
			this.rawData = rawData;
			this.mac = mac;
		}
		#endregion

		#region Tag helpers
		public static Tag GetTagByName(string tagName)
		{
			try
			{
				return (Tag)Enum.Parse(typeof(Tag), tagName, true);
			}
			catch (ArgumentException)
			{
				// check shortcuts/synonyms
				switch (tagName)
				{
					default:
						throw new ApplicationException("unknown tag name: " + tagName);
				}
			}
		}

		public static Comtech.Wbxml.Attribute GetAttributeByName(string attrName)
		{
			try
			{
				return (Comtech.Wbxml.Attribute)Enum.Parse(typeof(Comtech.Wbxml.Attribute), attrName, true);
			}
			catch (ArgumentException)
			{
				// check shortcuts/synonyms
				switch (attrName)
				{
					//case "dt":
					//    return Comtech.Wbxml.Attribute.DateTime;
					default:
						throw new ApplicationException("unknown attr name: " + attrName);
				}
			}
		}
		#endregion
	}
}
