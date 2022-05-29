namespace BTIT.EPM.DigitalSignature
{
    public class ContactConsts
    {

		public const int MinFirstNameLength = 0;
		public const int MaxFirstNameLength = 500;
						
		public const int MinLastNameLength = 0;
		public const int MaxLastNameLength = 500;
						
		public const int MinEmailLength = 0;
		public const int MaxEmailLength = 500;
		public const string EmailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
						
		public const int MinPhoneNumberLength = 0;
		public const int MaxPhoneNumberLength = 100;
		public const string PhoneNumberRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$";
						
    }
}