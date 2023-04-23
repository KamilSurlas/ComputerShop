using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComputerShop.Data;
using ComputerShop.Models;

namespace ComputerShop.Controllers
{
    public class CategoryGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoryGroups
        public async Task<IActionResult> Index()
        {
              return _context.CategoryGroups != null ? 
                          View(await _context.CategoryGroups.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CategoryGroups'  is null.");
        }

        // GET: CategoryGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryGroups == null)
            {
                return NotFound();
            }

            var categoryGroup = await _context.CategoryGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryGroup == null)
            {
                return NotFound();
            }

            return View(categoryGroup);
        }

        // GET: CategoryGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CategoryGroup categoryGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryGroup);
        }

        // GET: CategoryGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryGroups == null)
            {
                return NotFound();
            }

            var categoryGroup = await _context.CategoryGroups.FindAsync(id);
            if (categoryGroup == null)
            {
                return NotFound();
            }
            return View(categoryGroup);
        }

        // POST: CategoryGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CategoryGroup categoryGroup)
        {
            if (id != categoryGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryGroupExists(categoryGroup.Id))
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
            return View(categoryGroup);
        }

        // GET: CategoryGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryGroups == null)
            {
                return NotFound();
            }

            var categoryGroup = await _context.CategoryGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryGroup == null)
            {
                return NotFound();
            }

            return View(categoryGroup);
        }

        // POST: CategoryGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryGroups == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CategoryGroups'  is null.");
            }
            var categoryGroup = await _context.CategoryGroups.FindAsync(id);
            if (categoryGroup != null)
            {
                _context.CategoryGroups.Remove(categoryGroup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryGroupExists(int id)
        {
          return (_context.CategoryGroups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
