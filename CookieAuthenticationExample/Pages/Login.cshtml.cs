using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CookieAuthenticationExample.Models;
using CookieAuthenticationExample.Services;
using Microsoft.AspNetCore.Authorization;

namespace CookieAuthenticationExample.Pages
{
	[AllowAnonymous]
	public class LoginModel : PageModel
	{
		private readonly UserService _userService;

		public LoginModel(UserService userService)
		{
			_userService = userService;
		}

		public async Task<IActionResult> OnGetAsync(string paramUsername, string paramPassword)
        {
            if (string.IsNullOrEmpty(paramUsername) || string.IsNullOrEmpty(paramPassword))
                return LocalRedirect(NavigationPages.Home);

			try
			{
				// Clear the existing external cookie
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
			catch { }


			if (!_userService.CheckDatabaseIfPasswordMatches(paramUsername, paramPassword))
			{
				//no login possible
				return LocalRedirect(NavigationPages.Home);
			}
			
			//todo get user roles from Database via UserService
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
				//Todo add proper exception handling
				string error = ex.Message;

			}
			return LocalRedirect(NavigationPages.Home);
		}
	}

}
