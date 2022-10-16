using LearningManagementSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LearningManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;

namespace LearningManagementSystem.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly LMSContext _context;

		public AccountController(LMSContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _context.Users
				.Where(u => u.Username == User.Identity!.Name)
				.FirstAsync();

			return View(user);
		}

		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			var user = await _context.Users
				.Where(u => u.Username == loginViewModel.Username && u.DeletedAt == null && u.DisabledAt == null)
				.FirstOrDefaultAsync();

			if (user == null)
			{
				ModelState.AddModelError(nameof(LoginViewModel.Username), "User not found.");
				return View(loginViewModel);
			}

			var userIdentity = await _context.UserIdentities
				.FirstOrDefaultAsync(ui => ui.UserId == user.Id && ui.Password == loginViewModel.Password);
			if (userIdentity == null)
			{
				ModelState.AddModelError(nameof(LoginViewModel.Password), "Invalid credentials.");
				return View(loginViewModel);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim("FullName", user.Fullname),
				new Claim(ClaimTypes.Role, "Administrator"),
			};

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties
			{
				AllowRefresh = true,
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
				IsPersistent = loginViewModel.RememberMe,
				//RedirectUri = <string>
				// The full path or absolute URI to be used as an http 
				// redirect response value.
			};

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
			userIdentity.LastLoginDate = DateTime.Now;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[AllowAnonymous]
		public IActionResult Register()
		{
			return View(new RegisterViewModel { CreatedAt = DateTime.Now });
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(registerViewModel);
			}

			var _user = await _context.Users
				.FirstOrDefaultAsync(u => u.Username == registerViewModel.Username);

			if (_user != null)
			{
				ModelState.AddModelError(nameof(registerViewModel.Username), "Username already taken.");
				return View(registerViewModel);
			}
			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				var user = new User
				{
					Id = Guid.NewGuid(),
					Username = registerViewModel.Username,
					Email = registerViewModel.Email,
					Name = registerViewModel.Name,
					Lastname = registerViewModel.Lastname,
					CreatedAt = registerViewModel.CreatedAt,
				};

				_context.Users.Add(user);
				await _context.SaveChangesAsync();

				var userIdentity = new UserIdentity
				{
					UserId = user.Id,
					Password = registerViewModel.Password,
					PasswordResetDate = DateTime.Now
				};

				_context.UserIdentities.Add(userIdentity);
				await _context.SaveChangesAsync();
				transaction.Commit();
			}

			return RedirectToAction(nameof(Login));
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction(nameof(Login));
		}
	}
}
