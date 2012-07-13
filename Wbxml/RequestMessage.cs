using System;
using System.Collections.Generic;
using Comtech.Utils;
using Comtech.Wbxml;

namespace Comtech
{
	class RequestMessage : EComMessage
	{
		//public RequestMessage()
		//    : base(Tag.Req, WbxmlWriter.ElementFlags.HasAttributes)
		//{
		//}

		public RequestMessage(Element element)
			: base(Tag.Req, WbxmlWriter.ElementFlags.HasAttributes)
		{
			ConstructFromElement(element);
		}
	}
}
