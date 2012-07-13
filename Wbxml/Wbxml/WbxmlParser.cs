using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Comtech.Wbxml
{
	public class WbxmlParser
	{
		private RootElement rootElement;

		public RootElement RootElement
		{
			get { return rootElement; }
		}

		public WbxmlParser(byte[] buffer, Encoding encoding)
		{
			MemoryStream ms = new MemoryStream(buffer);
			rootElement = RootElement.ReadFromStreamWithHeader(ms, encoding);
		}
	}
}
