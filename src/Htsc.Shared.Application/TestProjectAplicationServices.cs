using Abp.Application.Services;

namespace Htsc.Shared
{
    public class TestProjectAplicationServices: ApplicationService
    {
        public string HelloShared()
        {
            return "Welcome to Shared";
        }
    }
}
