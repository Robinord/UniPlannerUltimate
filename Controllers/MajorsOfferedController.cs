using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniPlanner.Areas.Identity.Data;
using UniPlanner.Models;

namespace UniPlanner.Controllers
{
    
    public class MajorsOfferedController : Controller
    {
        private readonly UniPlannerContext _context;

        public MajorsOfferedController(UniPlannerContext context)
        {
            _context = context;
        }

        // GET: MajorsOffered
        public async Task<IActionResult> Index(int SearchInt)
        {

            if (_context.MajorsOffered == null)
            {
                return Problem("Entity set 'UniversityPlanner.MajorsOffered'  is null.");
            }


            var name = from n in _context.MajorsOffered.Include(m => m.UniProgramme)
            select n;

            

            if (SearchInt != 0)
            {
                name = name.Where(s => s.UniProgrammeID! == SearchInt);

            }
            return View(await name.ToListAsync());
        }

        // GET: MajorsOffered/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MajorsOffered == null)
            {
                return NotFound();
            }

            var majorsOffered = await _context.MajorsOffered
                .Include(m => m.UniProgramme)
                .FirstOrDefaultAsync(m => m.MajorsOfferedID == id);
            if (majorsOffered == null)
            {
                return NotFound();
            }

            return View(majorsOffered);
        }

        // GET: MajorsOffered/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UniProgrammeID"] = new SelectList(_context.UniProgramme, "UniProgrammeID", "UniProgrammeID");
            return View();
        }

        // POST: MajorsOffered/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("MajorsOfferedID,UniProgrammeID,Name,Link")] MajorsOffered majorsOffered)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(majorsOffered);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UniProgrammeID"] = new SelectList(_context.UniProgramme, "UniProgrammeID", "UniProgrammeID", majorsOffered.UniProgrammeID);
            return View(majorsOffered);
        }

        // GET: MajorsOffered/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MajorsOffered == null)
            {
                return NotFound();
            }

            var majorsOffered = await _context.MajorsOffered.FindAsync(id);
            if (majorsOffered == null)
            {
                return NotFound();
            }
            ViewData["UniProgrammeID"] = new SelectList(_context.UniProgramme, "UniProgrammeID", "UniProgrammeID", majorsOffered.UniProgrammeID);
            return View(majorsOffered);
        }

        // POST: MajorsOffered/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("MajorsOfferedID,UniProgrammeID,Name,Link")] MajorsOffered majorsOffered)
        {
            if (id != majorsOffered.MajorsOfferedID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(majorsOffered);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MajorsOfferedExists(majorsOffered.MajorsOfferedID))
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
            ViewData["UniProgrammeID"] = new SelectList(_context.UniProgramme, "UniProgrammeID", "UniProgrammeID", majorsOffered.UniProgrammeID);
            return View(majorsOffered);
        }

        // GET: MajorsOffered/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MajorsOffered == null)
            {
                return NotFound();
            }

            var majorsOffered = await _context.MajorsOffered
                .Include(m => m.UniProgramme)
                .FirstOrDefaultAsync(m => m.MajorsOfferedID == id);
            if (majorsOffered == null)
            {
                return NotFound();
            }

            return View(majorsOffered);
        }

        // POST: MajorsOffered/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MajorsOffered == null)
            {
                return Problem("Entity set 'UniPlannerContext.MajorsOffered'  is null.");
            }
            var majorsOffered = await _context.MajorsOffered.FindAsync(id);
            if (majorsOffered != null)
            {
                _context.MajorsOffered.Remove(majorsOffered);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MajorsOfferedExists(int id)
        {
          return (_context.MajorsOffered?.Any(e => e.MajorsOfferedID == id)).GetValueOrDefault();
        }
    }
}
