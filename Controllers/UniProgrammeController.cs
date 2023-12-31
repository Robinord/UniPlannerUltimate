﻿using System;
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
    public class UniProgrammeController : Controller
    {
        private readonly UniPlannerContext _context;

        public UniProgrammeController(UniPlannerContext context)
        {
            _context = context;
        }

        // GET: UniProgramme
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string SearchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["nameSort"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["rankScoreSort"] = sortOrder == "rankScore" ? "rankScore_desc" : "rankScore";
            ViewData["universitySort"] = sortOrder == "university" ? "university_desc" : "university";

            if (_context.UniProgramme == null)
            {
                return Problem("Entity set 'UniversityPlanner.UniProgramme'  is null.");
            }

            if (SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            var name = from n in _context.UniProgramme.Include(u => u.Programme).Include(u => u.UniversityInfo)
                select n;

            if (!String.IsNullOrEmpty(SearchString)) //filter feature
            {
                name = name.Where(s => s.Programme.Name!.Contains(SearchString));

            }
            

            switch (sortOrder)
            {
                case "name_desc":
                    name = name.OrderByDescending(s => s.Programme.Name);
                    break;
                case "rankScore_desc":
                    name = name.OrderByDescending(s => s.RankScore);
                    break;
                case "university_desc":
                    name = name.OrderByDescending(s => s.UniversityInfo.Name);
                    break;
                case "name":
                    name = name.OrderBy(s => s.Programme.Name);
                    break;
                case "rankScore":
                    name = name.OrderBy(s => s.RankScore);
                    break;
                case "university":
                    name = name.OrderBy(s => s.UniversityInfo.Name);
                    break;
                default:
                    break;
            }
            

            int pageSize = 10;
            return View(await PaginatedList<UniProgramme>.CreateAsync(name.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: UniProgramme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UniProgramme == null)
            {
                return NotFound();
            }

            var uniProgramme = await _context.UniProgramme
                .Include(u => u.Programme)
                .Include(u => u.UniversityInfo)
                .FirstOrDefaultAsync(m => m.UniProgrammeID == id);
            if (uniProgramme == null)
            {
                return NotFound();
            }

            return View(uniProgramme);
        }

        // GET: UniProgramme/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ProgrammeID"] = new SelectList(_context.Programme, "ProgrammeID", "Name");
            ViewData["UniversityInfoID"] = new SelectList(_context.UniversityInfo, "UniversityInfoID", "Name");
            return View();
        }

        // POST: UniProgramme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("UniProgrammeID,UniversityInfoID,ProgrammeID,Link,RankScore")] UniProgramme uniProgramme)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(uniProgramme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProgrammeID"] = new SelectList(_context.Programme, "ProgrammeID", "ProgrammeID", uniProgramme.ProgrammeID);
            ViewData["UniversityInfoID"] = new SelectList(_context.UniversityInfo, "UniversityInfoID", "UniversityInfoID", uniProgramme.UniversityInfoID);
            return View(uniProgramme);
        }

        // GET: UniProgramme/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UniProgramme == null)
            {
                return NotFound();
            }

            var uniProgramme = await _context.UniProgramme.FindAsync(id);
            if (uniProgramme == null)
            {
                return NotFound();
            }
            ViewData["ProgrammeID"] = new SelectList(_context.Programme, "ProgrammeID", "Name", uniProgramme.ProgrammeID);
            ViewData["UniversityInfoID"] = new SelectList(_context.UniversityInfo, "UniversityInfoID", "Name", uniProgramme.UniversityInfoID);
            return View(uniProgramme);
        }

        // POST: UniProgramme/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("UniProgrammeID,UniversityInfoID,ProgrammeID,Link,RankScore")] UniProgramme uniProgramme)
        {
            if (id != uniProgramme.UniProgrammeID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(uniProgramme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniProgrammeExists(uniProgramme.UniProgrammeID))
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
            ViewData["ProgrammeID"] = new SelectList(_context.Programme, "ProgrammeID", "ProgrammeID", uniProgramme.ProgrammeID);
            ViewData["UniversityInfoID"] = new SelectList(_context.UniversityInfo, "UniversityInfoID", "UniversityInfoID", uniProgramme.UniversityInfoID);
            return View(uniProgramme);
        }

        // GET: UniProgramme/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UniProgramme == null)
            {
                return NotFound();
            }

            var uniProgramme = await _context.UniProgramme
                .Include(u => u.Programme)
                .Include(u => u.UniversityInfo)
                .FirstOrDefaultAsync(m => m.UniProgrammeID == id);
            if (uniProgramme == null)
            {
                return NotFound();
            }

            return View(uniProgramme);
        }

        // POST: UniProgramme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UniProgramme == null)
            {
                return Problem("Entity set 'UniPlannerContext.UniProgramme'  is null.");
            }
            var uniProgramme = await _context.UniProgramme.FindAsync(id);
            if (uniProgramme != null)
            {
                _context.UniProgramme.Remove(uniProgramme);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UniProgrammeExists(int id)
        {
          return (_context.UniProgramme?.Any(e => e.UniProgrammeID == id)).GetValueOrDefault();
        }
    }
}
