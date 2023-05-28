using Abp.Modules;
using Abp.Reflection.Extensions;


using Htsc.Shared.Localization;


namespace Htsc.Shared
{
    public class SharedCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            SharedLocalizationConfigurer.Configure(Configuration.Localization);

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SharedCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
           // IocManager.Register<IUserDelegationConfiguration, UserDelegationConfiguration>();

           // IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
