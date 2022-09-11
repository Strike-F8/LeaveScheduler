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
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ManagerID,EmployeeID")] EmployeeManager employeeManager)
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

            var employeeManager = await _context.EmployeeManager.FindAsync(id);
            if (employeeManager == null)
            {
                return NotFound();
            }
            return View(employeeManager);
        }

        // POST: EmployeeManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ManagerID,EmployeeID")] EmployeeManager employeeManager)
        {
            if (id != employeeManager.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeManagerExists(employeeManager.ID))
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
            return View(employeeManager);
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
