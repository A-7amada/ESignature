namespace BTIT.EPM.DigitalSignature
{
    public class RecipientConsts
    {

		public const int MinFirstNameLength = 0;
		public const int MaxFirstNameLength = 500;
						
		public const int MinLastNameLength = 0;
		public const int MaxLastNameLength = 500;
						
		public const int MinEmailLength = 0;
		public const int MaxEmailLength = 500;
		public const string EmailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
						
		public const int MinSignerPinLength = 0;
		public const int MaxSignerPinLength = 100;
						
		public const int MinFieldNameLength = 0;
		public const int MaxFieldNameLength = 500;
						
		public const int MinMobileNumberLength = 0;
		public const int MaxMobileNumberLength = 20;
		public const string MobileNumberRegex = @"\d{10}";
						
    }
}