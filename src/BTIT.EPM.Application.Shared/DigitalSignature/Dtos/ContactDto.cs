
using System;
using Abp.Application.Services.Dto;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class ContactDto : EntityDto<long>
    {
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }



    }
}