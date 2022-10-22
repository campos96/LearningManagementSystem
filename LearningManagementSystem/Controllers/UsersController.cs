using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningManagementSystem.Data;
using LearningManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Plugins;

namespace LearningManagementSystem.Controllers
{
	[Authorize]
	public class UsersController : Controller
	{
		private readonly LMSContext _context;

		public UsersController(LMSContext context)
		{
			_context = context;
		}

		// GET: Users
		public async Task<IActionResult> Index()
		{
			var users = await _context.Users
				.Where(u => u.DeletedAt == null)
				.ToListAsync();
			return View(users);
		}

		// GET: Users/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null || _context.Users == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.Where(u => u.DeletedAt == null)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// GET: Users/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Users/Create
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Create(RegisterViewModel registerViewModel)
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
					CreatedAt = DateTime.Now,
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

			return RedirectToAction(nameof(Index));
		}

		// GET: Users/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null || _context.Users == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.Where(u => u.DeletedAt == null)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		// POST: Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, User user)
		{
			if (id != user.Id)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return View(user);
			}

			try
			{
				user.UpdatedAt = DateTime.Now;
				_context.Update(user);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(user.Id))
				{
					return NotFound();
				}
				throw;
			}
			return RedirectToAction(nameof(Index));
		}

		// GET: Users/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null || _context.Users == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.Where(u => u.DeletedAt == null)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			if (_context.Users == null)
			{
				return Problem("Entity set 'LMSContext.User'  is null.");
			}

			var user = await _context.Users
				.Where(u => u.DeletedAt == null)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (user != null)
			{
				user.DeletedAt = DateTime.Now;
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool UserExists(Guid id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}
