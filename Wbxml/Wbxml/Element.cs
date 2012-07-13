using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Comtech.Wbxml
{
	public class Element
	{
		private Tag tag;
		private AttributeList attributes;
		private List<Element> children;

		private const string DateTimeFormat = "yyyyMMddHHmmss";

		protected Element()
		{
		}

		#region Read from stream
		public static Element ReadFromStream(Stream s, Encoding encoding)
		{
			Element element = new Element();
			return ReadFromStream(s, encoding, element);
		}

		public static Element ReadFromStream(Stream s, Encoding encoding, Element element)
		{
			byte token = ReadByte(s, "element token");
			byte tagId = (byte)(token & 0x3F); // reset WBXML "tag with attributes" bit and "tag with content" bit
			if (tagId < 5)
				throw new ApplicationException(String.Format("expected element token, found {0} at {1}", token.ToString("X2"), s.Position));
			element.tag = (Tag)tagId;
			if ((token & 0x80) != 0)
			{
				element.attributes = new AttributeList();
				element.ReadAttributes(s, encoding);
			}
			if ((token & 0x40) != 0)
			{
				element.children = new List<Element>();
				element.ReadContent(s, encoding);
			}
			return element;
		}

		protected void ReadAttributes(Stream s, Encoding encoding)
		{
			while (true)
			{
				byte token = ReadByte(s, "attribute token or end token");
				if (token == 0x01)
				{
					// end of attributes
					break;
				}
				Attribute attr = (Attribute)token;
				byte source = ReadByte(s, "inline string token (03)");
				if (source != 0x03)
					throw new ApplicationException(String.Format("expected inline string token (03), found {0} at {1}", source.ToString("X2"), s.Position));
				if (attributes.ContainsKey(attr))
					throw new ApplicationException("duplicate attribute: " + attr);
				// read the inline string
				List<byte> bytes = new List<byte>();
				while (true)
				{
					byte b = ReadByte(s, "inline string");
					if (b == 0x00)
						break;
					bytes.Add(b);
				}
				string value = encoding.GetString(bytes.ToArray());
				attributes.Add(attr, value);
			}
		}

		protected void ReadContent(Stream s, Encoding encoding)
		{
			while (true)
			{
				byte token = ReadByte(s, "element token or end token");
				if (token == 0x01)
				{
					// end of children
					break;
				}
				s.Position--;
				Element child = Element.ReadFromStream(s, encoding);
				children.Add(child);
			}
		}

		protected static byte ReadByte(Stream s, string expected)
		{
			int b = s.ReadByte();
			if (b == -1)
				throw new ApplicationException(String.Format("unexpected end of stream at {0} (expected {1})", s.Position, expected));
			return (byte)b;
		}
		#endregion

		#region Read from XML
		public static Element ReadFromXml(XmlElement xmlElement)
		{
			Element element = new Element();
			return ReadFromXml(xmlElement, element);
		}

		public static Element ReadFromXml(XmlElement xmlElement, Element element)
		{
			element.tag = GetTagByName(xmlElement.LocalName);
			if (xmlElement.Attributes.Count > 0)
			{
				element.attributes = new AttributeList();
				element.ReadAttributes(xmlElement.Attributes);
			}
			if (xmlElement.ChildNodes.Count > 0)
			{
				element.children = new List<Element>();
				element.ReadContent(xmlElement.ChildNodes);
			}
			return element;
		}

		protected void ReadAttributes(XmlAttributeCollection xmlAttrs)
		{
			foreach (XmlAttribute xmlAttr in xmlAttrs)
			{
				Attribute attr = GetAttributeByName(xmlAttr.LocalName);
				if (attributes.ContainsKey(attr))
					throw new ApplicationException("duplicate attribute: " + attr);
				attributes.Add(attr, xmlAttr.Value);
			}
		}

		protected void ReadContent(XmlNodeList xmlChildNodes)
		{
			foreach (XmlNode xmlNode in xmlChildNodes)
			{
				if (xmlNode is XmlElement)
				{
					Element child = Element.ReadFromXml(xmlNode as XmlElement);
					children.Add(child);
				}
			}
		}

		private static Tag GetTagByName(string tagName)
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
					//case "itm":
					//	return Tag.Item;
					default:
						throw new ApplicationException("unknown tag name: " + tagName);
				}
			}
		}

		private static Attribute GetAttributeByName(string attrName)
		{
			try
			{
				return (Attribute)Enum.Parse(typeof(Attribute), attrName, true);
			}
			catch (ArgumentException)
			{
				// check shortcuts/synonyms
				switch (attrName)
				{
					case "dt":
						return Attribute.Dt;
					default:
						throw new ApplicationException("unknown attr name: " + attrName);
				}
			}
		}
		#endregion

		#region Write to XML
		public XmlElement WriteToXml(XmlDocument doc)
		{
			XmlElement xmlElement = doc.CreateElement(this.Tag.ToString().ToLower());
			if (this.HasAttributes)
			{
				foreach (Attribute attr in this.Attributes.Keys)
				{
					XmlAttribute xmlAttr = doc.CreateAttribute(attr.ToString().ToLower());
					xmlAttr.Value = this[attr];
					xmlElement.Attributes.Append(xmlAttr);
				}
			}
			if (this.HasChildren)
			{
				foreach (Element child in this.Children)
				{
					xmlElement.AppendChild(child.WriteToXml(doc));
				}
			}
			return xmlElement;
		}

		public string ToXmlString()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement docRoot = WriteToXml(doc);
			return docRoot.OuterXml;
		}
		#endregion

		#region Properties and accessors
		public bool HasAttributes
		{
			get { return attributes != null; }
		}

		public bool HasChildren
		{
			get { return children != null; }
		}

		public Tag Tag
		{
			get { return tag; }
		}

		public AttributeList Attributes
		{
			get { return attributes; }
		}

		public List<Element> Children
		{
			get { return children; }
		}

		public string this[Attribute attr]
		{
			get
			{
				try
				{
					return this.attributes[attr];
				}
				catch (KeyNotFoundException)
				{
					string message = String.Format("Attribute \"{0}\" not found in element <{1}>",
						attr.ToString().ToLower(), this.tag.ToString().ToLower());
					throw new KeyNotFoundException(message);
				}
			}
		}

		public string GetStringAttributeOrNull(Attribute attr)
		{
			return attributes.ContainsKey(attr) ? this.attributes[attr] : null;
		}

		public int GetIntAttribute(Attribute attr)
		{
			string value = this[attr];
			try
			{
				return Convert.ToInt32(value);
			}
			catch (FormatException)
			{
				string message = String.Format("Element <{0}>, attribute \"{1}\": expected integer, found \"{2}\"",
					this.tag.ToString().ToLower(), attr.ToString().ToLower(), value);
				throw new FormatException(message);
			}
		}

		public int? GetIntAttributeOrNull(Attribute attr)
		{
			return attributes.ContainsKey(attr) ? (int?)GetIntAttribute(attr) : (int?)null;
		}

		public short GetShortAttribute(Attribute attr)
		{
			string value = this[attr];
			try
			{
				return Convert.ToInt16(value);
			}
			catch (FormatException)
			{
				string message = String.Format("Element <{0}>, attribute \"{1}\": expected short, found \"{2}\"",
					this.tag.ToString().ToLower(), attr.ToString().ToLower(), value);
				throw new FormatException(message);
			}
		}

		public short? GetShortAttributeOrNull(Attribute attr)
		{
			return attributes.ContainsKey(attr) ? (short?)GetShortAttribute(attr) : (short?)null;
		}

		public long GetLongAttribute(Attribute attr)
		{
			string value = this[attr];
			try
			{
				return Convert.ToInt64(value);
			}
			catch (FormatException)
			{
				string message = String.Format("Element <{0}>, attribute \"{1}\": expected long, found \"{2}\"",
					this.tag.ToString().ToLower(), attr.ToString().ToLower(), value);
				throw new FormatException(message);
			}
		}

		public decimal GetDecimalAttribute(Attribute attr)
		{
			string value = this[attr];
			try
			{
				return Convert.ToDecimal(value, System.Globalization.NumberFormatInfo.InvariantInfo);
			}
			catch (FormatException)
			{
				string message = String.Format("Element <{0}>, attribute \"{1}\": expected decimal, found \"{2}\"",
					this.tag.ToString().ToLower(), attr.ToString().ToLower(), value);
				throw new FormatException(message);
			}
		}

		public decimal? GetDecimalAttributeOrNull(Attribute attr)
		{
			return attributes.ContainsKey(attr) ? (decimal?)GetDecimalAttribute(attr) : (decimal?)null;
		}

		public DateTime GetDateTimeAttribute(Attribute attr)
		{
			string value = this[attr];
			try
			{
				return DateTime.ParseExact(value, DateTimeFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
			}
			catch (FormatException)
			{
				string message = String.Format("Element <{0}>, attribute \"{1}\": expected datetime, found \"{2}\"",
					this.tag.ToString().ToLower(), attr.ToString().ToLower(), value);
				throw new FormatException(message);
			}
		}

		public void SetAttribute(Attribute attr, DateTime dt)
		{
			attributes[attr] = dt.ToString(DateTimeFormat);
		}
		#endregion
	}
}
