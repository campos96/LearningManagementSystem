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
    public class DepartmentsController : Controller
    {
        private readonly LMSContext _context;

        public DepartmentsController(LMSContext context)
        {
            _context = context;
        }

        // GET: Administration/Departments
        public async Task<IActionResult> Index()
        {
            var lMSContext = _context.Departments.Include(d => d.Facility).Include(d => d.ParentDepartment);
            return View(await lMSContext.ToListAsync());
        }

        // GET: Administration/Departments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Facility)
                .Include(d => d.ParentDepartment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Administration/Departments/Create
        public IActionResult Create()
        {
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "Id", "Name");
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Administration/Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FacilityId,Number,Name,ParentDepartmentId")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.Id = Guid.NewGuid();
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "Id", "Name", department.FacilityId);
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        // GET: Administration/Departments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "Id", "Name", department.FacilityId);
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        // POST: Administration/Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FacilityId,Number,Name,ParentDepartmentId")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "Id", "Name", department.FacilityId);
            ViewData["ParentDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.ParentDepartmentId);
            return View(department);
        }

        // GET: Administration/Departments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Facility)
                .Include(d => d.ParentDepartment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Administration/Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Departments == null)
            {
                return Problem("Entity set 'LMSContext.Departments'  is null.");
            }
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(Guid id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
