using System;
using System.Collections.Generic;
using System.Text;

namespace Comtech.Wbxml
{
	public enum Tag
	{
		Req = 5,	// request
		Rsp = 6,	// response
		Lst = 7,	// list
		Row = 8,	// record in a list
		Fld = 9,	// field in a record or in a list
		File = 10,	// file to download tag
		Done = 11,	// processed file notification
		Tot = 12,	// totals tag
	};
}
