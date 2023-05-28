namespace Htsc.Shared
{
    public class SharedConsts
    {
        public const string LocalizationSourceName = "Shared";

        public const string ConnectionStringName = "Default";
        public const int Timeout = 6000;  // dapper time out
        public const string EnvironmentProductionName = "Production";
        public const int MaxRowInPage = 500000;
        public const int MaxExcelRow = 500000;
        public const string MaxGenerateExcelRowConfigurationKey = "MaxGenerateExcelRow";
    }
}
