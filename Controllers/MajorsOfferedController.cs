﻿using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniPlanner.Areas.Identity.Data;
using UniPlanner.Models;

namespace UniPlanner.Controllers
{  [Authorize]
    
    public class MajorsOfferedController : Controller
    {
        
        private readonly UniPlannerContext _context;

        public MajorsOfferedController(UniPlannerContext context)
        {
            _context = context;
        }

        // GET: MajorsOffered
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string SearchString,
    int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["nameSort"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["programmeSort"] = sortOrder == "programme" ? "programme_desc" : "programme";
            ViewData["universitySort"] = sortOrder == "university" ? "university_desc" : "university";

            if (_context.MajorsOffered == null)
            {
                return Problem("Entity set 'UniversityPlanner.MajorsOffered'  is null.");
            }

            if (SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                SearchString = currentFilter;
            }



            var name = from n in _context.MajorsOffered.Include(m => m.UniProgramme).Include(m => m.UniProgramme.Programme).Include(m => m.UniProgramme.UniversityInfo)
            select n;

            

            if (!String.IsNullOrEmpty(SearchString)) //filter feature changed to int
            {
                name = name.Where(s => s.Name!.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    name = name.OrderByDescending(s => s.Name);
                    break;

                case "programme_desc":
                    name = name.OrderByDescending(s => s.UniProgramme.Programme.Name);
                    break;
                case "university_desc":
                    name = name.OrderByDescending(s => s.UniProgramme.UniversityInfo.Name);
                    break;
                case "name":
                    name = name.OrderBy(s => s.Name);
                    break;

                case "programme":
                    name = name.OrderBy(s => s.UniProgramme.Programme.Name);
                    break;
                case "university":
                    name = name.OrderBy(s => s.UniProgramme.UniversityInfo.Name);
                    break;

                default:
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<MajorsOffered>.CreateAsync(name.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // var uniProgramme = (from c in _context.UniProgramme.Include(u => u.Programme).Include(u => u.UniversityInfo) 
            //                select new SelectListItem 
            //               { Text = k.Programme.Name + "|" + k.UniversityInfo.Name, Value = k.UniProgrammeID.ToString() });
            var uniProgramme = _context.UniProgramme.Include(u => u.Programme).Include(u => u.UniversityInfo)
                                .Select(s => new
                                {
                                    Text = s.Programme.Name + " | " + s.UniversityInfo.Name,
                                    Value = s.UniProgrammeID

                                }
                                );
            ViewData["UniProgrammeID"] = new SelectList(uniProgramme, "Value", "Text");
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
        [Authorize(Roles = "Admin")]
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
            var uniProgramme = _context.UniProgramme.Include(u => u.Programme).Include(u => u.UniversityInfo)
                    .Select(s => new
                    {
                        Text = s.Programme.Name + " | " + s.UniversityInfo.Name,
                        Value = s.UniProgrammeID

                    }
                    );
            ViewData["UniProgrammeID"] = new SelectList(uniProgramme, "Value", "Text");
            return View(majorsOffered);
        }

        // POST: MajorsOffered/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MajorsOffered == null)
            {
                return NotFound();
            }

            var majorsOffered = await _context.MajorsOffered.Include(m => m.UniProgramme).Include(m => m.UniProgramme.Programme).Include(m => m.UniProgramme.UniversityInfo)
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
