using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniPlanner.Areas.Identity.Data;
using UniPlanner.Models;

namespace UniPlanner.Controllers
{
    public class UniversityInfoController : Controller
    {
        private readonly UniPlannerContext _context;

        public UniversityInfoController(UniPlannerContext context)
        {
            _context = context;
        }

        // GET: UniversityInfo
        public async Task<IActionResult> Index()
        {
              return _context.UniversityInfo != null ? 
                          View(await _context.UniversityInfo.ToListAsync()) :
                          Problem("Entity set 'UniPlannerContext.UniversityInfo'  is null.");
        }

        // GET: UniversityInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UniversityInfo == null)
            {
                return NotFound();
            }

            var universityInfo = await _context.UniversityInfo
                .FirstOrDefaultAsync(m => m.UniversityInfoID == id);
            if (universityInfo == null)
            {
                return NotFound();
            }

            return View(universityInfo);
        }

        // GET: UniversityInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UniversityInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniversityInfoID,Name,City,Region,THErank,QSrank,ARWUrank")] UniversityInfo universityInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(universityInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(universityInfo);
        }

        // GET: UniversityInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UniversityInfo == null)
            {
                return NotFound();
            }

            var universityInfo = await _context.UniversityInfo.FindAsync(id);
            if (universityInfo == null)
            {
                return NotFound();
            }
            return View(universityInfo);
        }

        // POST: UniversityInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UniversityInfoID,Name,City,Region,THErank,QSrank,ARWUrank")] UniversityInfo universityInfo)
        {
            if (id != universityInfo.UniversityInfoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(universityInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniversityInfoExists(universityInfo.UniversityInfoID))
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
            return View(universityInfo);
        }

        // GET: UniversityInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UniversityInfo == null)
            {
                return NotFound();
            }

            var universityInfo = await _context.UniversityInfo
                .FirstOrDefaultAsync(m => m.UniversityInfoID == id);
            if (universityInfo == null)
            {
                return NotFound();
            }

            return View(universityInfo);
        }

        // POST: UniversityInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UniversityInfo == null)
            {
                return Problem("Entity set 'UniPlannerContext.UniversityInfo'  is null.");
            }
            var universityInfo = await _context.UniversityInfo.FindAsync(id);
            if (universityInfo != null)
            {
                _context.UniversityInfo.Remove(universityInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UniversityInfoExists(int id)
        {
          return (_context.UniversityInfo?.Any(e => e.UniversityInfoID == id)).GetValueOrDefault();
        }
    }
}
