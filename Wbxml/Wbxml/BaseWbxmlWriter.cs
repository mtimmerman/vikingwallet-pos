using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Comtech.Wbxml
{
	public class BaseWbxmlWriter
	{
		private Stream s;
		private Encoding encoding;

		public BaseWbxmlWriter(Stream s, Encoding encoding)
		{
			this.s = s;
			this.encoding = encoding;
		}

		public void WriteHeader()
		{
			//s.WriteByte(0x01); // WBXML version 1.1
			//s.WriteByte(0x01); // Unknown public identifier
			//s.WriteByte(0x04); // charset=iso-8859-1
			//s.WriteByte(0x00); // String table length
			// 03 01 03 00
			s.WriteByte(0x03); // ?
			s.WriteByte(0x01); // Unknown public identifier
			s.WriteByte(0x03); // charset=US-ASCII // TODO: should be configurable?
			s.WriteByte(0x00); // String table length
		}

		public void WriteAttribute(byte attr, object value)
		{
			if (value == null)
				return;
			WriteAttribute(attr, value.ToString());
		}

		public void WriteAttribute(byte attr, string value)
		{
			if (value == null)
				return;
			s.WriteByte(attr);
			s.WriteByte(0x03); // inline string follows
			byte[] buf = encoding.GetBytes(value);
			s.Write(buf, 0, buf.Length);
			s.WriteByte(0x00); // end of string
		}

		public void WriteElement(byte tagToken)
		{
			s.WriteByte(tagToken);
		}

		public void WriteTerminator()
		{
			s.WriteByte(0x01); // END (of attribute list)
		}
	}
}
