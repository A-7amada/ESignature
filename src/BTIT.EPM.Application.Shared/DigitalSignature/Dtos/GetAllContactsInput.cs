using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetAllContactsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string FirstNameFilter { get; set; }

		public string LastNameFilter { get; set; }

		public string EmailFilter { get; set; }

		public string PhoneNumberFilter { get; set; }



    }
}