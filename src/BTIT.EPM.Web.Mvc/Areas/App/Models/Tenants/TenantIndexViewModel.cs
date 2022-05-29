using System.Collections.Generic;
using BTIT.EPM.Editions.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}