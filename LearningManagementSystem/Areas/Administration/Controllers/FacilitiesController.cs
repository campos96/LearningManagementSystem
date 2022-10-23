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
    public class FacilitiesController : Controller
    {
        private readonly LMSContext _context;

        public FacilitiesController(LMSContext context)
        {
            _context = context;
        }

        // GET: Administration/Facilities
        public async Task<IActionResult> Index()
        {
            var lMSContext = _context.Facilities.Include(f => f.Location);
            return View(await lMSContext.ToListAsync());
        }

        // GET: Administration/Facilities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities
                .Include(f => f.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // GET: Administration/Facilities/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City");
            return View();
        }

        // POST: Administration/Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LocationId,Name,Address")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                facility.Id = Guid.NewGuid();
                _context.Add(facility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City", facility.LocationId);
            return View(facility);
        }

        // GET: Administration/Facilities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City", facility.LocationId);
            return View(facility);
        }

        // POST: Administration/Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,LocationId,Name,Address")] Facility facility)
        {
            if (id != facility.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(facility.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City", facility.LocationId);
            return View(facility);
        }

        // GET: Administration/Facilities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities
                .Include(f => f.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: Administration/Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Facilities == null)
            {
                return Problem("Entity set 'LMSContext.Facilities'  is null.");
            }
            var facility = await _context.Facilities.FindAsync(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityExists(Guid id)
        {
          return _context.Facilities.Any(e => e.Id == id);
        }
    }
}
