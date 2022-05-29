using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BTIT.EPM.Authorization;

namespace BTIT.EPM
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(EPMApplicationSharedModule),
        typeof(EPMCoreModule)
        )]
    public class EPMApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EPMApplicationModule).GetAssembly());
        }
    }
}