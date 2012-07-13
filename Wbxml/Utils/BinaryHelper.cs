using System;
using System.Collections.Generic;
using System.Text;

namespace Comtech.Utils
{
	public class BinaryHelper
	{
		public static int SwapByteOrder(int src)
		{
			int[] i = new int[4];
			i[0] = src & 0xff;
			i[1] = (src >> 8) & 0xff;
			i[2] = (src >> 16) & 0xff;
			i[3] = (src >> 24) & 0xff;
			return ((i[0] << 24) | (i[1] << 16) | (i[2] << 8) | i[3]);
		}
	}
}
