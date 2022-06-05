using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;


namespace BTIT.EPM.Documents.Dtos
{
    public class CreateDocumentInputDto : EntityDto<long?>
    {




            [Required]
            [StringLength(DocumentConsts.MaxFileNameLength, MinimumLength = DocumentConsts.MinFileNameLength)]
            public string FileName { get; set; }

            [Required]
            [StringLength(DocumentConsts.MaxExtensionLength, MinimumLength = DocumentConsts.MinExtensionLength)]
            public string Extension { get; set; }

            public long? Size { get; set; }

            [StringLength(DocumentConsts.MaxContentTypeLength, MinimumLength = DocumentConsts.MinContentTypeLength)]
            public string ContentType { get; set; }


            //public string Comment { get; set; }

            // public DocumentTypeEnum? DocumentTypeEnum { get; set; }

            public long? DocumentBagId { get; set; }

            public Guid? BinaryObjectId { get; set; }

            public byte[] FileBytes { set; get; }

            public string Description { set; get; }

            public bool? IsCreateDocumentBagId { set; get; }


        }


    }

