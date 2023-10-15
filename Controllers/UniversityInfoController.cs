using System;
using System.Collections.Generic;
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
{
    [Authorize]
    public class UniversityInfoController : Controller
    { 
        private readonly UniPlannerContext _context;
        
        public UniversityInfoController(UniPlannerContext context)
        {
            _context = context;
        }

        // GET: UniversityInfo
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string SearchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["CitySort"] = sortOrder == "city" ? "city_desc" : "city";
            ViewData["RegionSort"] = sortOrder == "region" ? "region_desc" : "region";
            ViewData["THErankSort"] = sortOrder == "THErank" ? "THErank_desc" : "THErank";
            ViewData["QSrankSort"] = sortOrder == "QSrank" ? "QSrank_desc" : "QSrank";
            ViewData["ARWUrankSort"] = sortOrder == "ARWUrank" ? "ARWUrank_desc" : "ARWUrank";
           
            if (_context.UniversityInfo == null)
            {
                return Problem("Entity set 'UniversityPlanner.UniversityInfo'  is null.");
            }

            if (SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            var name = from n in _context.UniversityInfo
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
                case "city_desc":
                    name = name.OrderByDescending(s => s.City);
                    break;
                case "region_desc":
                    name = name.OrderByDescending(s => s.Region);
                    break;
                case "THErank_desc":
                    name = name.OrderByDescending(s => s.THErank);
                    break;
                case "QSrank_desc":
                    name = name.OrderByDescending(s => s.QSrank);
                    break;
                case "ARWUrank_desc":
                    name = name.OrderByDescending(s => s.ARWUrank);
                    break;
                case "name":
                    name = name.OrderBy(s => s.Name);
                    break;
                case "city":
                    name = name.OrderBy(s => s.City);
                    break;
                case "region":
                    name = name.OrderBy(s => s.Region);
                    break;
                case "THErank":
                    name = name.OrderBy(s => s.THErank);
                    break;
                case "QSrank":
                    name = name.OrderBy(s => s.QSrank);
                    break;
                case "ARWUrank":
                    name = name.OrderBy(s => s.ARWUrank);
                    break;
                default:
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<UniversityInfo>.CreateAsync(name.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UniversityInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("UniversityInfoID,Name,City,Region,THErank,QSrank,ARWUrank")] UniversityInfo universityInfo)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(universityInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(universityInfo);
        }

        // GET: UniversityInfo/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("UniversityInfoID,Name,City,Region,THErank,QSrank,ARWUrank")] UniversityInfo universityInfo)
        {
            if (id != universityInfo.UniversityInfoID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
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
        [Authorize(Roles = "Admin")]
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
