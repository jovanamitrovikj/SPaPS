using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPaPS.Data;
using SPaPS.Models;

namespace SPaPS.Controllers
{
    public class ServiceActivitiesController : Controller
    {
        private readonly SPaPSContext _context;

        public ServiceActivitiesController(SPaPSContext context)
        {
            _context = context;
        }

        // GET: ServiceActivities
        public async Task<IActionResult> Index()
        {
            var sPaPSContext = _context.ServiceActivities.Include(s => s.Activity).Include(s => s.Service);
            return View(await sPaPSContext.ToListAsync());
        }

        // GET: ServiceActivities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ServiceActivities == null)
            {
                return NotFound();
            }

            var serviceActivity = await _context.ServiceActivities
                .Include(s => s.Activity)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.ServiceActivityId == id);
            if (serviceActivity == null)
            {
                return NotFound();
            }

            return View(serviceActivity);
        }

        // GET: ServiceActivities/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId");
            return View();
        }

        // POST: ServiceActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceActivityId,ServiceId,ActivityId")] ServiceActivity serviceActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", serviceActivity.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", serviceActivity.ServiceId);
            return View(serviceActivity);
        }

        // GET: ServiceActivities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.ServiceActivities == null)
            {
                return NotFound();
            }

            var serviceActivity = await _context.ServiceActivities.FindAsync(id);
            if (serviceActivity == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", serviceActivity.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", serviceActivity.ServiceId);
            return View(serviceActivity);
        }

        // POST: ServiceActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ServiceActivityId,ServiceId,ActivityId")] ServiceActivity serviceActivity)
        {
            if (id != serviceActivity.ServiceActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceActivityExists(serviceActivity.ServiceActivityId))
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
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", serviceActivity.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", serviceActivity.ServiceId);
            return View(serviceActivity);
        }

        // GET: ServiceActivities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ServiceActivities == null)
            {
                return NotFound();
            }

            var serviceActivity = await _context.ServiceActivities
                .Include(s => s.Activity)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.ServiceActivityId == id);
            if (serviceActivity == null)
            {
                return NotFound();
            }

            return View(serviceActivity);
        }

        // POST: ServiceActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ServiceActivities == null)
            {
                return Problem("Entity set 'SPaPSContext.ServiceActivities'  is null.");
            }
            var serviceActivity = await _context.ServiceActivities.FindAsync(id);
            if (serviceActivity != null)
            {
                _context.ServiceActivities.Remove(serviceActivity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceActivityExists(long id)
        {
          return (_context.ServiceActivities?.Any(e => e.ServiceActivityId == id)).GetValueOrDefault();
        }
    }
}
