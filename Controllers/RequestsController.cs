using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveScheduler.Data;
using LeaveScheduler.Models;

namespace LeaveScheduler.Controllers
{
    public class RequestsController : Controller
    {
        private readonly LeaveSchedulerContext _context;

        public RequestsController(LeaveSchedulerContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
              return View(await _context.Request.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create(int? id)
        {
            ViewData["name"] = _context.Employee.Find(id).FirstName + " " +
                _context.Employee.Find(id).LastName;
            ViewData["id"] = id;

            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestID,RequestDate,RequestStatus")] Request request, int id)
        {
            if (ModelState.IsValid && _context.Employee.Find(id).AvailableLeaveTime > 0)
            {
                request.EmployeeID = id;
                _context.Add(request);
                _context.Employee.Find(id).AvailableLeaveTime--;
                await _context.SaveChangesAsync();
                return LocalRedirect($"~/Schedule/Details/{request.EmployeeID}");
            }
            return LocalRedirect($"~/Schedule/Details/{id}");
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the user is a manager
            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            int employeeID = Convert.ToInt32(identity.Claims.First(c => c.Type == "EmployeeID").Value);
            
            var result = from p in _context.Manager
                         where p.EmployeeID == employeeID
                         select p;

            if (result.Any())
                ViewData["isManager"] = true;
            else
                ViewData["isManager"] = false;

            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,EmployeeID,RequestDate,RequestStatus")] Request request)
        {
            if (id != request.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return LocalRedirect($"~/Schedule/Details/{request.EmployeeID}");
            }
            return LocalRedirect($"~/Schedule/Details/{request.EmployeeID}");
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Request == null)
            {
                return Problem("Entity set 'LeaveSchedulerContext.Request'  is null.");
            }
            var request = await _context.Request.FindAsync(id);
            if (request != null)
            {
                _context.Request.Remove(request);
                _context.Employee.Find(request.EmployeeID).AvailableLeaveTime++;
            }
            
            await _context.SaveChangesAsync();
            return LocalRedirect($"~/Schedule/Details/{request.EmployeeID}");
        }

        private bool RequestExists(int id)
        {
          return _context.Request.Any(e => e.RequestID == id);
        }
    }
}
