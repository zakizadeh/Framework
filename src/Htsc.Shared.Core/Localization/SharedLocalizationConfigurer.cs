using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Htsc.Shared.Localization
{
    public static class SharedLocalizationConfigurer
    {

        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    SharedConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(SharedLocalizationConfigurer).GetAssembly(),
                        "Htsc.Shared.Localization.Shared"
                    )
                )
            );
        }
    }
}