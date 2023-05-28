using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Htsc.Shared.Configure;
using Htsc.Shared.Startup;
using Htsc.Shared.Test.Base;

namespace Htsc.Shared.GraphQL.Tests
{
    [DependsOn(
        typeof(SharedGraphQLModule),
        typeof(SharedTestBaseModule))]
    public class SharedGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SharedGraphQLTestModule).GetAssembly());
        }
    }
}