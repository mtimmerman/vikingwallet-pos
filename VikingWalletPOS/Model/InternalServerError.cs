using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingWalletPOS.Model
{
    public class InternalServerError : ResultObject
    {
        public InternalServerError(Exception exception)
        {
            messages = new Message[1];
            messages[0] = new Message();
            messages[0].msg_code = "X_002";
            messages[0].msg_text = exception.ToString();
            messages[0].opt_field_type = "VikingWalletPOS";
            messages[0].opt_field_value = exception.StackTrace;
        }
    }
}
