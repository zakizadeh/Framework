using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore;
using Htsc.Shared.Configuration;
using Htsc.Shared.EntityFrameworkCore;
using Abp.Application.Services;

namespace Htsc.Shared.Web.Startup
{
    [DependsOn(
       typeof(SharedApplicationModule),
       typeof(SharedEntityFrameworkCoreModule),
       typeof(AbpAspNetCoreModule))]
    public class SharedWebMvcModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public SharedWebMvcModule(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(SharedConsts.ConnectionStringName);

            Configuration.Navigation.Providers.Add<SharedNavigationProvider>();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(SharedApplicationModule).GetAssembly(), "Shared"
                );

        } 

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SharedWebMvcModule).GetAssembly());
        }

    }

    //public class TestProjectAplicationServices : ApplicationService
    //{
    //    public string HelloMpose()
    //    {
    //        return "Welcome Dead Customer";
    //    }
    //}
}