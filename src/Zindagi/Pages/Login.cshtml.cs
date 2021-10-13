using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zindagi.Infra.Auth;

namespace Zindagi.Pages
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string redirectUri) =>
            await HttpContext.ChallengeAsync(AuthConstants.AuthenticationScheme, new AuthenticationProperties { RedirectUri = redirectUri });
    }
}
