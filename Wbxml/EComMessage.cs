using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;
using Comtech.Wbxml;
using Comtech.Utils;

namespace Comtech
{
	public abstract class EComMessage : Message
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public enum ResponseCode
		{
			GenericError = -1,
			Success = 0,
			OriginalTransactionNotFound = 22,
			ReversalAlreadyDone = 24,
			TotalsMismatch = 31,
			NoVouchersAvailable = 41,
			BillNotFound = 42,
			PaymentRefused = 43,
		}

		public static readonly string DefaultApp = AppConfig.AppID;
		public static readonly string DefaultVersion = "2"; // last major version number of terminal app
		public bool UseCompatibilityMode(string version)
		{
			return String.IsNullOrEmpty(version) || version[0] == '1';
		}

		public EComMessage(Tag type, WbxmlWriter.ElementFlags flags)
			: base(type, flags)
		{
		}

		public EComMessage(Tag type)
			: base(type)
		{
		}

		// temp for Mobile Vikings
		private RootElement tempRootElement;
		public RootElement RootElement { get { return tempRootElement; } }

		// temp for Mobile Vikings
		protected override void WriteAttributes(WbxmlWriter w)
		{
			foreach (var attr in tempRootElement.Attributes.Keys)
				w.WriteAttribute(attr, tempRootElement.Attributes[attr]);
		}

		public static EComMessage ReadFromStream(Stream s)
		{
			byte authType;
			byte[] rawData;
			byte[] mac;
			RootElement root = Message.ReadElementFromStream(s, out authType, out rawData, out mac);
			EComMessage msg = CreateMessageFromElement(root);
			msg.SetIncomingAuthData(authType, rawData, mac);
			return msg;
		}

        public override void WriteToStream(Stream s)
        {
            // create the message body in a buffer
            MemoryStream ms = new MemoryStream();
            WbxmlWriter w = new WbxmlWriter(ms, AppConfig.TerminalEncoding);
            w.WriteHeader();
            w.WriteElement(tempRootElement);

            byte[] buf = ms.ToArray();
            int len = buf.Length;

            log.Debug("sending message: length: " + len + "\r\n" + HexHelper.ToHexDump(buf));

            // write message into the TCP/IP stream (length + body + MAC if needed)
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(BinaryHelper.SwapByteOrder(len));
            bw.Write(buf);
        }

		public static EComMessage CreateMessageFromElement(RootElement element)
		{
			// temp for Mobile Vikings
			EComMessage msg = null;
			if (!element.HasAttributes)
				throw new ApplicationException("received an element without attributes: " + element.Tag);
			switch (element.Tag)
			{
				case Tag.Req:
					msg = new RequestMessage(element);
					break;
				case Tag.Rsp:
					msg = new ResponseMessage(element);
					break;
				default:
					throw new ApplicationException("message tag not supported: " + element.Tag);
			}
			msg.tempRootElement = element;
			return msg;

	
			//EComMessage msg = null;
			//if (!element.HasAttributes)
			//    throw new ApplicationException("received an element without attributes: " + element.Tag);
			//switch (element.Tag)
			//{
			//    case Tag.Req:
			//        string app = element[Comtech.Wbxml.Attribute.App];
			//        string id = element[Comtech.Wbxml.Attribute.Id];
			//        if (id == "topup")
			//            msg = new AuthorizationMessage(element);
			//        //else if (id == "voucher")
			//        //	msg = new AuthorizationMessage(element);
			//        //else if (id == "reversal")
			//        //	msg = new AuthorizationMessage(element);
			//        else if (id == "checktopup")
			//            msg = new AuthorizationMessage(element);
			//        else if (id == "closeday")
			//            msg = new CloseDayMessage(element);
			//        else if (id == "balance")
			//            msg = new BalanceMessage(element);
			//        else if (id == "history")
			//            msg = new HistoryMessage(element);
			//        else if (id == "billinfo")
			//            msg = new BillInfoMessage(element);
			//        else if (id == "billpayment")
			//            msg = new BillPaymentMessage(element);
			//        else if (id == "problemreport")
			//            msg = new ProblemReportMessage(element);
			//        else if (id == "4gsecure")
			//            msg = new _4GSecureMessage(element);
			//        else if (id == "handshake")
			//            msg = new HandshakeMessage(element);
			//        else if (id == "download")
			//            msg = new DownloadMessage(element);
			//        else if (id == "notify")
			//            msg = new NotificationMessage(element);
			//        else
			//            throw new ApplicationException(String.Format("message id not supported: {0} {1}", app, id));
			//        break;
			//    case Tag.Rsp:
			//        // for terminal simulator
			//        // hack
			//        if (element.Attributes.ContainsKey(Comtech.Wbxml.Attribute.Ftp))
			//        {
			//            msg = new DownloadResponseMessage(element);
			//        }
			//        else
			//        {
			//            msg = new ResponseMessage(element);
			//        }
			//        break;
			//    default:
			//        throw new ApplicationException("message tag not supported: " + element.Tag);
			//}
			//return msg;
		}

		/// <summary>
		/// Returns an integer representation of the specified amount for the terminal.
		/// E.g. if decimal points = 2, the amount 7.5 will be sent to the terminal as "750".
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		//public static string AmountToTerminalFormat(decimal amount)
		//{
		//    int factor = Convert.ToInt32(Math.Pow(10, AppConfig.DecimalPointsForCurrencyAmounts));
		//    int convertedAmount = Convert.ToInt32(amount * factor);
		//    return convertedAmount.ToString("0", System.Globalization.CultureInfo.InvariantCulture);
		//}

		/// <summary>
		/// Converts the specified amount from its integer representation on the terminal
		/// to its normal decimal value.
		/// E.g. if decimal points = 2 and the terminal sends "750", it means 7.5.
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		//public static decimal AmountFromTerminalFormat(string amount)
		//{
		//    int factor = Convert.ToInt32(Math.Pow(10, AppConfig.DecimalPointsForCurrencyAmounts));
		//    int terminalAmount = Convert.ToInt32(amount, System.Globalization.CultureInfo.InvariantCulture);
		//    return Convert.ToDecimal(terminalAmount) / factor;
		//}
	}
}
