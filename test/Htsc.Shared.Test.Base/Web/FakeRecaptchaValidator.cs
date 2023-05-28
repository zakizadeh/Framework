using System.Threading.Tasks;
using Htsc.Shared.Security.Recaptcha;

namespace Htsc.Shared.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
