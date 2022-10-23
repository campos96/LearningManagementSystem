using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningManagementSystem.Data;
using LearningManagementSystem.Models;

namespace LearningManagementSystem.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class LocationsController : Controller
    {
        private readonly LMSContext _context;

        public LocationsController(LMSContext context)
        {
            _context = context;
        }

        // GET: Administration/Locations
        public async Task<IActionResult> Index()
        {
            var lMSContext = _context.Locations.Include(l => l.Company);
            return View(await lMSContext.ToListAsync());
        }

        // GET: Administration/Locations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Administration/Locations/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Administration/Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,City,State,Country")] Location location)
        {
            if (ModelState.IsValid)
            {
                location.Id = Guid.NewGuid();
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", location.CompanyId);
            return View(location);
        }

        // GET: Administration/Locations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", location.CompanyId);
            return View(location);
        }

        // POST: Administration/Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CompanyId,City,State,Country")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", location.CompanyId);
            return View(location);
        }

        // GET: Administration/Locations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Administration/Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Locations == null)
            {
                return Problem("Entity set 'LMSContext.Locations'  is null.");
            }
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(Guid id)
        {
          return _context.Locations.Any(e => e.Id == id);
        }
    }
}
