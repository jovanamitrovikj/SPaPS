using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPaPS.Data;
using SPaPS.Enums;
using SPaPS.Models;

namespace SPaPS.Controllers
{
    [Authorize(Policy = nameof(AuthorizePolicy.AdminAuthPolicy))]
    [Authorize(Policy = nameof(AuthorizePolicy.ClientAuthPolicy))]

    public class ReferenceTypesController : Controller
    {
        private readonly SPaPSContext _context;

        public ReferenceTypesController(SPaPSContext context)
        {
            _context = context;
        }
        [AllowAnonymous]

        // GET: ReferenceTypes
        public async Task<IActionResult> Index()
        {
              return _context.ReferenceTypes != null ? 
                          View(await _context.ReferenceTypes.ToListAsync()) :
                          Problem("Entity set 'SPaPSContext.ReferenceTypes'  is null.");
        }

        // GET: ReferenceTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ReferenceTypes == null)
            {
                return NotFound();
            }

            var referenceType = await _context.ReferenceTypes
                .FirstOrDefaultAsync(m => m.ReferenceTypeId == id);
            if (referenceType == null)
            {
                return NotFound();
            }

            return View(referenceType);
        }

        // GET: ReferenceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReferenceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReferenceType referenceType)
        {
            if (ModelState.IsValid)
            {
                referenceType.CreatedBy =1;
                referenceType.CreatedOn = DateTime.Now;
                _context.Add(referenceType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(referenceType);
        }

        // GET: ReferenceTypes/Edit/5
        public async Task<IActionResult> Edit(long? id, Boolean readOnly)
        {
            if (id == null || _context.ReferenceTypes == null)
            {
                return NotFound();
            }


            var referenceType = await _context.ReferenceTypes.FindAsync(id);
            if (referenceType == null)
            {
                return NotFound();
            }
            return View(referenceType);
        }

        // POST: ReferenceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,ReferenceType referenceType)
        {
            if (id != referenceType.ReferenceTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   referenceType.UpdatedBy =1;
                   referenceType.UpdatedOn= DateTime.Now;
                    _context.Update(referenceType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReferenceTypeExists(referenceType.ReferenceTypeId))
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
            return View(referenceType);
        }

        // GET: ReferenceTypes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ReferenceTypes == null)
            {
                return NotFound();
            }

            var referenceType = await _context.ReferenceTypes
                .FirstOrDefaultAsync(m => m.ReferenceTypeId == id);
            if (referenceType == null)
            {
                return NotFound();
            }

            return View(referenceType);
        }

        // POST: ReferenceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ReferenceTypes == null)
            {
                return Problem("Entity set 'SPaPSContext.ReferenceTypes'  is null.");
            }
            var referenceType = await _context.ReferenceTypes.FindAsync(id);
            if (referenceType != null)
            {
                _context.ReferenceTypes.Remove(referenceType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReferenceTypeExists(long id)
        {
          return (_context.ReferenceTypes?.Any(e => e.ReferenceTypeId == id)).GetValueOrDefault();
        }
    }
}
