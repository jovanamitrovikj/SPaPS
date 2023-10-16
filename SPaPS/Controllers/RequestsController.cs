using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPaPS.Data;
using SPaPS.Enums;
using SPaPS.Models;
using Request = SPaPS.Models.Request;
using DataAccess.Services;
using NuGet.Common;
using System.Net.Http;

namespace SPaPS.Controllers
{
    public class RequestsController : Controller
    {
        private readonly SPaPSContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        private readonly IEmailSenderEnhance _emailService;

        public RequestsController(SPaPSContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var Client = await _context.Clients.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();
            var clientServices = _context.ClientServices.Where(x => x.ClientId == Client.ClientId).Select(x => x.ServiceId).ToList();
            List<Request> spapsContext;

            if (User.IsInRole(EnumRoles.Company.ToString()))
            {

                spapsContext = await _context.Requests.Where(x => clientServices.Contains(x.ServiceId)).Include(r => r.Activity).Include(r => r.Client).Include(r => r.Service).ToListAsync();
            }
            else
            {
                spapsContext = await _context.Requests.Where(x => x.ClientId == Client.ClientId).Include(r => r.Activity).Include(r => r.Client).Include(r => r.Service).ToListAsync();

            }
            return View(spapsContext);
        }



        // GET: Requests/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Activity)
                .Include(r => r.Client)
                .Include(r => r.Service)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "Name");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Description");
            ViewBag.BuildingTypes = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 4).ToList(), "ReferenceTypeId", "ReferenceTypeId");

            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request)
        {
            if (ModelState.IsValid)
            {
                var client = await _context.Clients.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();
                request.ClientId = client.ClientId;
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", request.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", request.ServiceId);
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", request.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", request.ServiceId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Request request)
        {
            if (id != request.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = await _context.Clients.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();
                    request.ClientId = client.ClientId;
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestId))
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
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", request.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", request.ServiceId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Activity)
                .Include(r => r.Client)
                .Include(r => r.Service)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'SPaPSContext.Requests'  is null.");
            }
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(long id)
        {
          return (_context.Requests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }

        //private async Task SendEmailNotifications(Request request)
        //{
        //    var service =  _context.Services.FirstOrDefault(s => s.ServiceId == request.ServiceId);

        //    // Find users with the "Company" role
        //    var companyUsers = await _userManager.GetUsersInRoleAsync("Company");

        //    // Send email notifications to company users
        //    foreach (var user in companyUsers)
        //    {
        //        // Construct the email message with a link to view the request details
        //        var callbackUrl = Url.Action("Details", "Requests", new { id = request.RequestId }, Request.Scheme);
        //        var message = new MailMessage
        //        {
        //            Subject = "New Request Notification",
        //            Body = $"A new request has been created that matches your services. Click here to view the request details: {callbackUrl}",
        //            From = new MailAddress(_configuration["SmtpSettings:Username"]),
        //        };

        //        message.To.Add(user.Email);

        //        // Send the email
        //        smtpClient.Send(message);
        //    }
        //}
    }
}
