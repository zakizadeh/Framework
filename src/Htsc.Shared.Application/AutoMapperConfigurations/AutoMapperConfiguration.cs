using Abp.AutoMapper;
using Abp.Configuration.Startup;

namespace Htsc.Shared.AutoMapperConfigurations
{
    public partial class AutoMapperConfiguration
    {
        public static void ServiceLogMapper(IAbpStartupConfiguration configuration)
        {
            configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
               // config.CreateMap<ServiceLogFilterDto, ServiceLogFilterMO>();
               
               // config.CreateMap<DailyDeadPeopleandCustomerMO, DailyDeadPeopleandCustomerForGroupByDto>()
               //.ForMember(dto => dto.IsInqueryDisable, opt => opt.MapFrom(s => true))
              // ;
                
             

                config.AddReceiveInformationReportConfig();
            });
        }
    }

}
