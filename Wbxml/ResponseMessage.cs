using System;
using Comtech.Utils;
using Comtech.Wbxml;

namespace Comtech
{
	class ResponseMessage : EComMessage
	{
		//public ResponseMessage(EComMessage.ResponseCode code, string message, string stan)
		//    : base(Tag.Rsp, WbxmlWriter.ElementFlags.HasAttributes)
		//{
		//    this.code = (int)code;
		//    this.displayedMessage = message;
		//    this.printedMessage = message;
		//    this.stan = stan;
		//}

		//// for handshake/login
		//public ResponseMessage(bool sendDateTime)
		//    : base(Tag.Rsp, WbxmlWriter.ElementFlags.HasAttributes)
		//{
		//    this.code = 0;
		//    this.sendDateTime = sendDateTime;
		//}

		public ResponseMessage(Element element)
			: base(Tag.Rsp, WbxmlWriter.ElementFlags.HasAttributes)
		{
			ConstructFromElement(element);
		}
	}
}
