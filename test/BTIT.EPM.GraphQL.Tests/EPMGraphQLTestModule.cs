using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BTIT.EPM.Configure;
using BTIT.EPM.Startup;
using BTIT.EPM.Test.Base;

namespace BTIT.EPM.GraphQL.Tests
{
    [DependsOn(
        typeof(EPMGraphQLModule),
        typeof(EPMTestBaseModule))]
    public class EPMGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EPMGraphQLTestModule).GetAssembly());
        }
    }
}