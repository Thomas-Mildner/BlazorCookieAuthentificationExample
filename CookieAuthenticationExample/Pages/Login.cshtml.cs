using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CookieAuthenticationExample.Pages
{
	[AllowAnonymous]
	public class LoginModel : PageModel
	{
        public async Task<IActionResult> OnGetAsync(string paramUsername, string paramPassword)
		{
			string returnUrl = Url.Content("~/");
			try
			{
				// Clear the existing external cookie
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
			catch { }


			//TODO check paramUserName & paramPassword in DB
			
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, paramUsername),
				new Claim(ClaimTypes.Role, "Administrator"),
			};
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties
			{
				IsPersistent = true,
				RedirectUri = this.Request.Host.Value
			};
			try
			{
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
			}
			catch (Exception ex)
			{
				string error = ex.Message;

			}
			return LocalRedirect(returnUrl);
		}
	}

}
