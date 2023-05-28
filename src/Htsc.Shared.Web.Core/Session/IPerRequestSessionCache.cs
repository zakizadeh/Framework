using System.Threading.Tasks;
using Htsc.Shared.Sessions.Dto;

namespace Htsc.Shared.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
