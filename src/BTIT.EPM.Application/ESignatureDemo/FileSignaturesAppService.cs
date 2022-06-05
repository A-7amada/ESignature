using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using BTIT.EPM.ESignatureDemo.Exporting;
using BTIT.EPM.ESignatureDemo.Dtos;
using BTIT.EPM.Dto;
using Abp.Application.Services.Dto;
using BTIT.EPM.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using BTIT.EPM.Storage;

namespace BTIT.EPM.ESignatureDemo
{
    [AbpAuthorize(AppPermissions.Pages_FileSignatures)]
    public class FileSignaturesAppService : EPMAppServiceBase, IFileSignaturesAppService
    {
        private readonly IRepository<FileSignature> _fileSignatureRepository;
        private readonly IFileSignaturesExcelExporter _fileSignaturesExcelExporter;

        public FileSignaturesAppService(IRepository<FileSignature> fileSignatureRepository, IFileSignaturesExcelExporter fileSignaturesExcelExporter)
        {
            _fileSignatureRepository = fileSignatureRepository;
            _fileSignaturesExcelExporter = fileSignaturesExcelExporter;

        }

        public async Task<PagedResultDto<GetFileSignatureForViewDto>> GetAll(GetAllFileSignaturesInput input)
        {

            var filteredFileSignatures = _fileSignatureRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Describtion.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescribtionFilter), e => e.Describtion == input.DescribtionFilter);

            var pagedAndFilteredFileSignatures = filteredFileSignatures
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var fileSignatures = from o in pagedAndFilteredFileSignatures
                                 select new
                                 {

                                     o.Name,
                                     o.Describtion,
                                     Id = o.Id
                                 };

            var totalCount = await filteredFileSignatures.CountAsync();

            var dbList = await fileSignatures.ToListAsync();
            var results = new List<GetFileSignatureForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetFileSignatureForViewDto()
                {
                    FileSignature = new FileSignatureDto
                    {

                        Name = o.Name,
                        Describtion = o.Describtion,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetFileSignatureForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetFileSignatureForViewDto> GetFileSignatureForView(int id)
        {
            var fileSignature = await _fileSignatureRepository.GetAsync(id);

            var output = new GetFileSignatureForViewDto { FileSignature = ObjectMapper.Map<FileSignatureDto>(fileSignature) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FileSignatures_Edit)]
        public async Task<GetFileSignatureForEditOutput> GetFileSignatureForEdit(EntityDto input)
        {
            var fileSignature = await _fileSignatureRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFileSignatureForEditOutput { FileSignature = ObjectMapper.Map<CreateOrEditFileSignatureDto>(fileSignature) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFileSignatureDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_FileSignatures_Create)]
        protected virtual async Task Create(CreateOrEditFileSignatureDto input)
        {
            var fileSignature = ObjectMapper.Map<FileSignature>(input);

            if (AbpSession.TenantId != null)
            {
                fileSignature.TenantId = (int?)AbpSession.TenantId;
            }

            await _fileSignatureRepository.InsertAsync(fileSignature);

        }

        [AbpAuthorize(AppPermissions.Pages_FileSignatures_Edit)]
        protected virtual async Task Update(CreateOrEditFileSignatureDto input)
        {
            var fileSignature = await _fileSignatureRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, fileSignature);

        }

        [AbpAuthorize(AppPermissions.Pages_FileSignatures_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _fileSignatureRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFileSignaturesToExcel(GetAllFileSignaturesForExcelInput input)
        {

            var filteredFileSignatures = _fileSignatureRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Describtion.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescribtionFilter), e => e.Describtion == input.DescribtionFilter);

            var query = (from o in filteredFileSignatures
                         select new GetFileSignatureForViewDto()
                         {
                             FileSignature = new FileSignatureDto
                             {
                                 Name = o.Name,
                                 Describtion = o.Describtion,
                                 Id = o.Id
                             }
                         });

            var fileSignatureListDtos = await query.ToListAsync();

            return _fileSignaturesExcelExporter.ExportToFile(fileSignatureListDtos);
        }

    }
}