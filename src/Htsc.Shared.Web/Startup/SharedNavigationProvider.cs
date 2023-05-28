using Abp.Application.Navigation;
using Abp.Localization;

namespace Htsc.Shared.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class SharedNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            /*
              new AppMenuItem('Settings', 'Pages.Tenant.Dashboard', 'flaticon-settings orange', '/app/main/dashboard'),
            new AppMenuItem('Shared', 'Pages.Shared.home', 'flaticon-settings orange', '/shared/home'),
             */
            context.Manager.Menus.Add("Shared", new MenuDefinition("Shared", L("Shared"), " flaticon-rotate red "));
            //context.Manager.Menus["Shared"]
            //.AddItem(

            //new MenuItemDefinition(
            //               PageNames.BaseInformation,
            //               L("Management"),
            //               icon: "flaticon-settings font-darkGreen",
            //               permissionDependency: new SimplePermissionDependency(AppPermissions.SharedModule_Management)
            //               )
            //                         //.AddItem(
            //                         //                              new MenuItemDefinition(
            //                         //                                  PageNames.ServiceLog,
            //                         //                                  L("ServiceLog"),
            //                         //                                  url: "/Shared/base-information/service-logs",
            //                         //                                  icon: "flaticon2-line font-darkGreen",
            //                         //                                  permissionDependency: new SimplePermissionDependency(AppPermissions.SharedModule_Management_ServiceLog_Menu)
            //                         //                                  )
            //                         //                              )
            //                         //)





            //       );

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SharedConsts.LocalizationSourceName);
        }
    }
}
