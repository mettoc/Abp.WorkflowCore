using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace WorkflowDemo.Localization
{
    public static class WorkflowDemoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(WorkflowDemoConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(WorkflowDemoLocalizationConfigurer).GetAssembly(),
                        "WorkflowDemo.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
