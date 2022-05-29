using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BTIT.EPM
{
    [DependsOn(typeof(EPMCoreSharedModule))]
    public class EPMApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EPMApplicationSharedModule).GetAssembly());
        }
    }
}