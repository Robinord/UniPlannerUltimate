using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UniPlanner.Areas.Identity.Data;
using UniPlanner.Models;

namespace UniPlanner.Controllers
{  [Authorize]
    public class ProgrammeController : Controller
    {
        
        private readonly UniPlannerContext _context;

        public ProgrammeController(UniPlannerContext context)
        {
            _context = context;
        }

        // GET: Programme
        public async Task<IActionResult> Index(
     string sortOrder,
     string currentFilter,
     string SearchString,
     int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = sortOrder == "name" ? "name_desc" : "name";

            if (_context.Programme == null)
            {
                return Problem("Entity set 'UniversityPlanner.Programme'  is null.");
            }

            if (SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            var name = from n in _context.Programme
                       select n;

            if (!String.IsNullOrEmpty(SearchString)) //filter feature
            {
                name = name.Where(s => s.Name!.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    name = name.OrderByDescending(s => s.Name);
                    break;
               
                case "name":
                    name = name.OrderBy(s => s.Name);
                    break;

                default:
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Programme>.CreateAsync(name.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Programme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Programme == null)
            {
                return NotFound();
            }

            var programme = await _context.Programme
                .FirstOrDefaultAsync(m => m.ProgrammeID == id);
            if (programme == null)
            {
                return NotFound();
            }

            return View(programme);
        }

        // GET: Programme/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Programme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgrammeID,Name")] Programme programme)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(programme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programme);
        }

        // GET: Programme/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Programme == null)
            {
                return NotFound();
            }

            var programme = await _context.Programme.FindAsync(id);
            if (programme == null)
            {
                return NotFound();
            }
            return View(programme);
        }

        // POST: Programme/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProgrammeID,Name")] Programme programme)
        {
            if (id != programme.ProgrammeID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(programme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgrammeExists(programme.ProgrammeID))
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
            return View(programme);
        }

        // GET: Programme/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Programme == null)
            {
                return NotFound();
            }

            var programme = await _context.Programme
                .FirstOrDefaultAsync(m => m.ProgrammeID == id);
            if (programme == null)
            {
                return NotFound();
            }

            return View(programme);
        }

        // POST: Programme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Programme == null)
            {
                return Problem("Entity set 'UniPlannerContext.Programme'  is null.");
            }
            var programme = await _context.Programme.FindAsync(id);
            if (programme != null)
            {
                _context.Programme.Remove(programme);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgrammeExists(int id)
        {
          return (_context.Programme?.Any(e => e.ProgrammeID == id)).GetValueOrDefault();
        }
    }
}
