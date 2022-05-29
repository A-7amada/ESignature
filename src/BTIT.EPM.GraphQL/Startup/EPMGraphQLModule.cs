using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BTIT.EPM.Startup
{
    [DependsOn(typeof(EPMCoreModule))]
    public class EPMGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EPMGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}