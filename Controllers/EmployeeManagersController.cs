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
    public class EmployeeManagersController : Controller
    {
        private readonly LeaveSchedulerContext _context;

        public EmployeeManagersController(LeaveSchedulerContext context)
        {
            _context = context;
        }

        // GET: EmployeeManagers
        public async Task<IActionResult> Index()
        {
              return View(await _context.EmployeeManager.ToListAsync());
        }

        // GET: EmployeeManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeManager == null)
            {
                return NotFound();
            }

            var employeeManager = await _context.EmployeeManager
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employeeManager == null)
            {
                return NotFound();
            }

            return View(employeeManager);
        }

        // GET: EmployeeManagers/Create
        public IActionResult Create(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        // POST: EmployeeManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManagerID,EmployeeID")] EmployeeManager employeeManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeManager);
        }

        // GET: EmployeeManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeeManager == null)
            {
                return NotFound();
            }
            // Check if this employee already has a manager
            var result = from e in _context.EmployeeManager
                         where e.EmployeeID == id
                         select e;
            if (result.Any()) // If a manager exists, edit the current manager
                return View(result.First());
            else // if a manager does not exist, create one
                return LocalRedirect($"/EmployeeManagers/Create/{id}");
        }

        // POST: EmployeeManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ManagerID,EmployeeID")] EmployeeManager employeeManager)
        {
            if (id != employeeManager.EmployeeID)
            {
                return NotFound();
            }
            // Get the correct EmployeeManagerID so that it can be updated
            var result = from em in _context.EmployeeManager
                         where em.EmployeeID == employeeManager.EmployeeID
                         select em;
            int emID = (int)result.First().ID;

            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeManager em = _context.EmployeeManager.Find(emID);
                    em.ManagerID = employeeManager.ManagerID;
                    em.EmployeeID = employeeManager.EmployeeID;

                    _context.EmployeeManager.Update(em);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeManagerExists(emID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return LocalRedirect("/Employees");
            }
            return LocalRedirect("/Schedule");
        }

        // GET: EmployeeManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeManager == null)
            {
                return NotFound();
            }

            var employeeManager = await _context.EmployeeManager
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employeeManager == null)
            {
                return NotFound();
            }

            return View(employeeManager);
        }

        // POST: EmployeeManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeManager == null)
            {
                return Problem("Entity set 'LeaveSchedulerContext.EmployeeManager'  is null.");
            }
            var employeeManager = await _context.EmployeeManager.FindAsync(id);
            if (employeeManager != null)
            {
                _context.EmployeeManager.Remove(employeeManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeManagerExists(int id)
        {
          return _context.EmployeeManager.Any(e => e.ID == id);
        }
    }
}
