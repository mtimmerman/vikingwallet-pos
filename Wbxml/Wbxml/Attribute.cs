namespace Comtech.Wbxml
{
	public enum Attribute
	{
        /*
		App = 5,	// application identifier 
		Id = 6,		// request identifier
		Dt = 7,		// date/time stamp
		Tid = 8,	// terminal identifier
		Mid = 9,	// merchant identifier
		Seq = 10,	// sequence number
		Amt = 11,	// amount of the operation
		Code = 12,	// response code
		Dsp = 13,	// message to display
		Prt = 14,	// message to print
		Aut = 15,	// security scheme
		Ser = 16,	// card serial number
		Bat = 17,	// batch number
		Sap = 18,	// terminal S.A.P. code
		Sno = 19,	// terminal serial number
		Exp = 20,	// voucher expiry date
		Action = 21,// action to be performed
		Val = 22,	// parameter value in response
		Ftp = 23,	// address of ftp server
		Login = 24, // login to ftp server
		Pwd = 25,	// password to ftp server
		Dir = 26,	// directory path of ftp server
		Svc = 27,	// service identifier
		Pin = 28,	// voucher recharge code
		Ver = 29,	// terminal app version
		Qty = 30,	// quantity (operation-specific; e.g. the number of items ordered)
		Tel = 31,	// telephone number
		Mod = 32,	// mode
		Pmt = 33,	// payment type
		Imsi = 34,	// IMSI
		Imei = 35,	// IMEI
		Acc = 36,	// bill payment / account number
		Meter = 37, // bill payment / meter number
		Bank = 38,	// bill payment / bank name
		Cheque = 39,// bill payment / cheque number
        */
        App = 5,   // application identifier , to be defined by Comtech
        Id = 6,    // request identifier or field identifier
        Dt = 7,    // date/time stamp
        Tid = 8,   // terminal identifier
        Mid = 9,   // merchant identifier, to be sppecified by VM if necessary
        Seq = 10,  // sequence number, authorization number of transaction reference number
        Amt = 11,  // amount of the operation
        Code = 12, // response code 
        Dsp = 13,  // message to display
        Prt = 14,  // message to print
        Val = 15,  // Parameter value in response
        Otp = 16,  // Password for deal retrieving
        Ver = 17,  // application version
        Pmt = 18,  // payment mode
        Pan = 19,  // card PAN
        Deal = 20, // deal identification number, to be specified by VM                
	}
}
