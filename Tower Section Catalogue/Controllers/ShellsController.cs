using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tower_Section_Catalogue.Data;
using Tower_Section_Catalogue.Models;

namespace Tower_Section_Catalogue.Controllers
{
    public class ShellsController : Controller
    {
        private readonly Tower_Section_CatalogueContext _context;

        public ShellsController(Tower_Section_CatalogueContext context)
        {
            _context = context;
        }

        // GET: Shells
        public async Task<IActionResult> Index()
        {
            var tower_Section_CatalogueContext = _context.Shell.Include(s => s.TowerSection);
            return View(await tower_Section_CatalogueContext.ToListAsync());
        }

        // GET: Shells/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shell == null)
            {
                return NotFound();
            }

            var shell = await _context.Shell
                .Include(s => s.TowerSection)
                .FirstOrDefaultAsync(m => m.ShellPosition == id);
            if (shell == null)
            {
                return NotFound();
            }

            return View(shell);
        }

        // GET: Shells/Create
        public IActionResult Create()
        {
            ViewData["TowerSectionId"] = new SelectList(_context.Set<TowerSection>(), "Id", "Id");
            return View();
        }

        // POST: Shells/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shell shell)
        {
            if (ModelState.IsValid)
            {
                // Check if this is the first shell in the section, so no diameter continuity constraint applies.
                var isFirstShell = !_context.Shell.Any(s => s.TowerSectionId == shell.TowerSectionId);

                if (!isFirstShell)
                {
                    // Find the previous shell in the same section with the highest ShellPosition.
                    var previousShell = _context.Shell
                        .Where(s => s.TowerSectionId == shell.TowerSectionId)
                        .OrderByDescending(s => s.ShellPosition)
                        .FirstOrDefault();

                    if (previousShell != null && previousShell.TopDiameter != shell.BottomDiameter)
                    {
                        // If the constraint is not met, add a model error.
                        ModelState.AddModelError("BottomDiameter", "The bottom diameter of this shell must match the top diameter of the previous shell.");
                        ViewData["TowerSectionId"] = new SelectList(_context.Set<TowerSection>(), "Id", "Id", shell.TowerSectionId);
                        return View(shell);
                    }
                }

                // If all constraints are met, add the shell to the context and save it to the database.
                _context.Add(shell);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return to the view with the errors.
            ViewData["TowerSectionId"] = new SelectList(_context.Set<TowerSection>(), "Id", "Id", shell.TowerSectionId);
            return View(shell);
        }

        // GET: Shells/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shell == null)
            {
                return NotFound();
            }

            var shell = await _context.Shell.FindAsync(id);
            if (shell == null)
            {
                return NotFound();
            }
            ViewData["TowerSectionId"] = new SelectList(_context.Set<TowerSection>(), "Id", "Id", shell.TowerSectionId);
            return View(shell);
        }

        // POST: Shells/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShellPosition,Height,BottomDiameter,TopDiameter,Thickness,SteelDensity,TowerSectionId")] Shell shell)
        {
            if (id != shell.ShellPosition)
            {
                return NotFound();
            }

            // Check if this is the first shell in the section, so no diameter continuity constraint applies.
            var isFirstShell = !_context.Shell.Any(s => s.TowerSectionId == shell.TowerSectionId);

            if (!isFirstShell)
            {
                // Find the previous shell in the same section with the highest ShellPosition.
                var previousShell = _context.Shell
                    .Where(s => s.TowerSectionId == shell.TowerSectionId && s.ShellPosition < shell.ShellPosition)
                    .OrderByDescending(s => s.ShellPosition)
                    .FirstOrDefault();

                if (previousShell != null && previousShell.TopDiameter != shell.BottomDiameter)
                {
                    ModelState.AddModelError("BottomDiameter", "The bottom diameter of this shell must match the top diameter of the previous shell.");
                    ViewData["TowerSectionId"] = new SelectList(_context.Set<TowerSection>(), "Id", "Id", shell.TowerSectionId);
                    return View(shell);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shell);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShellExists(shell.ShellPosition))
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

            ViewData["TowerSectionId"] = new SelectList(_context.Set<TowerSection>(), "Id", "Id", shell.TowerSectionId);
            return View(shell);
        }

        // GET: Shells/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shell == null)
            {
                return NotFound();
            }

            var shell = await _context.Shell
                .Include(s => s.TowerSection)
                .FirstOrDefaultAsync(m => m.ShellPosition == id);
            if (shell == null)
            {
                return NotFound();
            }

            return View(shell);
        }

        // POST: Shells/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shell == null)
            {
                return Problem("Entity set 'Tower_Section_CatalogueContext.Shell'  is null.");
            }
            var shell = await _context.Shell.FindAsync(id);
            if (shell != null)
            {
                _context.Shell.Remove(shell);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShellExists(int id)
        {
          return (_context.Shell?.Any(e => e.ShellPosition == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> RetrieveByPartNumber([FromQuery] string partNumber)
        {
            if (string.IsNullOrEmpty(partNumber))
            {
                return BadRequest("Part number cannot be empty.");
            }

            var towerSection = await _context.TowerSection
                .Include(ts => ts.Shells) // Include related shells
                .FirstOrDefaultAsync(ts => ts.partNumber == partNumber);

            if (towerSection == null)
            {
                return NotFound($"Tower section with part number '{partNumber}' not found.");
            }

            return View("RetrieveByPartNumber", towerSection); // You can create a "Details" view to display detailed information.
        }

    }

}

