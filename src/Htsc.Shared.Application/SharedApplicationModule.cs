using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Htsc.Shared.Authorization;

namespace Htsc.Shared
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(SharedCoreModule)
        )]
    public class SharedApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SharedApplicationModule).GetAssembly());
        }
    }
}