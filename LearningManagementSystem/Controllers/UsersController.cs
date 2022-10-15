using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningManagementSystem.Data;
using LearningManagementSystem.Models;

namespace LearningManagementSystem.Controllers
{
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
			var users = await _context.User
				.Where(u => u.DeletedAt == null)
				.ToListAsync();
			return View(users);
		}

		// GET: Users/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null || _context.User == null)
			{
				return NotFound();
			}

			var user = await _context.User
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
			return View(new User { CreatedAt = DateTime.Now });
		}

		// POST: Users/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(User user)
		{
			if (!ModelState.IsValid)
			{
				return View(user);
			}

			user.Id = Guid.NewGuid();
			user.CreatedAt = DateTime.Now;
			_context.Add(user);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Users/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null || _context.User == null)
			{
				return NotFound();
			}

			var user = await _context.User
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
			if (id == null || _context.User == null)
			{
				return NotFound();
			}

			var user = await _context.User
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
			if (_context.User == null)
			{
				return Problem("Entity set 'LMSContext.User'  is null.");
			}

			var user = await _context.User
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
			return _context.User.Any(e => e.Id == id);
		}
	}
}
