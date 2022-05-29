using Abp.Domain.Services;

namespace BTIT.EPM
{
    public abstract class EPMDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected EPMDomainServiceBase()
        {
            LocalizationSourceName = EPMConsts.LocalizationSourceName;
        }
    }
}
