

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using BTIT.EPM.DigitalSignature.Exporting;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using Abp.Application.Services.Dto;
using BTIT.EPM.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BTIT.EPM.DigitalSignature
{
	[AbpAuthorize(AppPermissions.Pages_Contacts)]
    public class ContactsAppService : EPMAppServiceBase, IContactsAppService
    {
		 private readonly IRepository<Contact, long> _contactRepository;
		 private readonly IContactsExcelExporter _contactsExcelExporter;
		 

		  public ContactsAppService(IRepository<Contact, long> contactRepository, IContactsExcelExporter contactsExcelExporter ) 
		  {
			_contactRepository = contactRepository;
			_contactsExcelExporter = contactsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input)
         {
			
			var filteredContacts = _contactRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter),  e => e.FirstName == input.FirstNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter),  e => e.LastName == input.LastNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter),  e => e.PhoneNumber == input.PhoneNumberFilter);

			var pagedAndFilteredContacts = filteredContacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var contacts = from o in pagedAndFilteredContacts
                         select new GetContactForViewDto() {
							Contact = new ContactDto
							{
                                FirstName = o.FirstName,
                                LastName = o.LastName,
                                Email = o.Email,
                                PhoneNumber = o.PhoneNumber,
                                Id = o.Id
							}
						};

            var totalCount = await filteredContacts.CountAsync();

            return new PagedResultDto<GetContactForViewDto>(
                totalCount,
                await contacts.ToListAsync()
            );
         }
		 
		 public async Task<GetContactForViewDto> GetContactForView(long id)
         {
            var contact = await _contactRepository.GetAsync(id);

            var output = new GetContactForViewDto { Contact = ObjectMapper.Map<ContactDto>(contact) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
		 public async Task<GetContactForEditOutput> GetContactForEdit(EntityDto<long> input)
         {
            var contact = await _contactRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetContactForEditOutput {Contact = ObjectMapper.Map<CreateOrEditContactDto>(contact)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditContactDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Contacts_Create)]
		 protected virtual async Task Create(CreateOrEditContactDto input)
         {
            var contact = ObjectMapper.Map<Contact>(input);

			
			if (AbpSession.TenantId != null)
			{
				contact.TenantId = (int) AbpSession.TenantId;
			}
		

            await _contactRepository.InsertAsync(contact);
         }

		 [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
		 protected virtual async Task Update(CreateOrEditContactDto input)
         {
            var contact = await _contactRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, contact);
         }

		 [AbpAuthorize(AppPermissions.Pages_Contacts_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _contactRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input)
         {
			
			var filteredContacts = _contactRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter),  e => e.FirstName == input.FirstNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter),  e => e.LastName == input.LastNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter),  e => e.PhoneNumber == input.PhoneNumberFilter);

			var query = (from o in filteredContacts
                         select new GetContactForViewDto() { 
							Contact = new ContactDto
							{
                                FirstName = o.FirstName,
                                LastName = o.LastName,
                                Email = o.Email,
                                PhoneNumber = o.PhoneNumber,
                                Id = o.Id
							}
						 });


            var contactListDtos = await query.ToListAsync();

            return _contactsExcelExporter.ExportToFile(contactListDtos);
         }


    }
}