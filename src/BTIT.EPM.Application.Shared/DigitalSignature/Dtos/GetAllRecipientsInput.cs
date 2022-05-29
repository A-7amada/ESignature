using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetAllRecipientsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int TypeFilter { get; set; }

		public string FirstNameFilter { get; set; }

		public string LastNameFilter { get; set; }

		public string EmailFilter { get; set; }

		public int IsSignerFilter { get; set; }

		public Guid? CodeFilter { get; set; }

		public DateTime? MaxViewDateFilter { get; set; }
		public DateTime? MinViewDateFilter { get; set; }

		public DateTime? MaxSignatureDateFilter { get; set; }
		public DateTime? MinSignatureDateFilter { get; set; }

		public string SignerPinFilter { get; set; }

		public int IsSignedFilter { get; set; }

		public int? MaxSigneOrderFilter { get; set; }
		public int? MinSigneOrderFilter { get; set; }

		public string FieldNameFilter { get; set; }

		public DateTime? MaxSentDateFilter { get; set; }
		public DateTime? MinSentDateFilter { get; set; }

		public int IsSentFilter { get; set; }

		public short? MaxSignerPinTriesCountFilter { get; set; }
		public short? MinSignerPinTriesCountFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string DocumentRequestDocumentTitleFilter { get; set; }

		 
    }
}