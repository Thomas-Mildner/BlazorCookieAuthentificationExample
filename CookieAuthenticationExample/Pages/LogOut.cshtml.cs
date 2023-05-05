using CookieAuthenticationExample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CookieAuthenticationExample.Pages
{
    public class LogOutModel : PageModel
    {
		public async Task<IActionResult> OnGetAsync()
		{
			// Clear the existing external cookie
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return LocalRedirect(NavigationPages.Home);
		}
	}
}
