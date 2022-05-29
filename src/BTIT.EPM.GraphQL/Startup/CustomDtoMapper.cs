using AutoMapper;
using BTIT.EPM.Authorization.Users;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Startup
{
    public static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Roles, options => options.Ignore())
                .ForMember(dto => dto.OrganizationUnits, options => options.Ignore());

            configuration.CreateMap<Recipient, DocumentRequestViewAndSignDto>()
                .ForMember(dto => dto.Code, options => options.MapFrom(r => r.Code))
                .ForMember(dto => dto.DocumentRequest, options => options.MapFrom(r => r.DocumentRequestFk))
                .ForMember(dto => dto.DocumentRequestId, options => options.MapFrom(r => r.DocumentRequestId))
                .ForMember(dto => dto.Email, options => options.MapFrom(r => r.Email))
                .ForMember(dto => dto.IsSigned, options => options.MapFrom(r => r.IsSigned))
                .ForMember(dto => dto.ViewDate, options => options.MapFrom(r => r.ViewDate))
                .ForMember(dto => dto.SignatureDate, options => options.MapFrom(r => r.SignatureDate))
                .ForMember(dto => dto.SignerPin, options => options.MapFrom(r => r.SignerPin))
                .ForMember(dto => dto.SigneOrder, options => options.MapFrom(r => r.SigneOrder))
                .ForMember(dto => dto.UserId, options => options.MapFrom(r => r.UserId))
                .ForMember(dto => dto.Name, options => options.MapFrom(r => r.UserFk!=null ?r.UserFk.FullName:string.Empty))
                .ForMember(dto => dto.UserName, options => options.MapFrom(r => r.UserFk != null ? r.UserFk.UserName:string.Empty));

        }
    }
}