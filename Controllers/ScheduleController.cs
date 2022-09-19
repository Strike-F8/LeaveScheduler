using LeaveScheduler.Data;
using LeaveScheduler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveScheduler.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {

        private readonly LeaveSchedulerContext _context;

        public ScheduleController(LeaveSchedulerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Check the current user's identity to retrieve the appropriate list of schedules
            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
            int employeeID = Convert.ToInt32(identity.Claims.First(c => c.Type == "EmployeeID").Value);

            if(IsManager(employeeID))
            {
                // User is a manager and should have access to the schedules of employees under him/her
                var manager = from p in _context.Manager
                            where p.EmployeeID == employeeID
                            select p;
                int managerID = manager.First().ManagerID;

                var employees = from p in _context.EmployeeManager
                                join emp in _context.Employee on p.EmployeeID equals emp.EmployeeID
                                where p.ManagerID == managerID
                                select emp;

                var m = from p in _context.Employee
                          where p.EmployeeID == employeeID
                          select p;

                List<Employee> employeeList = new()
                {
                    m.First()
                };
                employeeList.AddRange(employees.ToList());

                return View(employeeList);
            }
            else
            {
                // user is not a manager and should only have access to his/her own schedule
                var result = from p in _context.Employee
                             where p.EmployeeID == employeeID
                             select p;

                return _context.Employee != null ?
                    View(await result.ToListAsync()) :
                    Problem("Entity set 'LeaveSchedulerContext.Employee' is null.");
            }

            /*return _context.Employee != null ?
                        View(await _context.Employee.ToListAsync()) :
                        Problem("Entity set 'LeaveSchedulerContext.Employee' is null."); */
        }
        public bool IsManager(int id)
        {
            var result = from p in _context.Manager
                         where p.EmployeeID == id
                         select p;
            if (result.Any())
                return true;
            return false;
        }

        public ViewResult Details(int? id)
        {
            ViewData["name"] = _context.Employee.Find(id).FirstName + " " +
                _context.Employee.Find(id).LastName;
            ViewData["id"] = id;
            ViewData["days"] = _context.Employee.Find(id).AvailableLeaveTime;

            List<EmpRequestMiniViewModel> model = new();
            using (_context)
            {
                var results = from p in _context.Employee
                              join req in _context.Request on p.EmployeeID equals req.EmployeeID
                              where req.EmployeeID == id orderby req.RequestDate
                              select new EmpRequestMiniViewModel()
                              {
                                  EmployeeName = p.FirstName + " " + p.LastName,
                                  RequestDate = req.RequestDate,
                                  RequestStatus = req.RequestStatus,
                                  RequestID = req.RequestID
                              };
                model = results.ToList();
            }

            return View(model);
        }

        public IActionResult Create()
        {
            // create a new request
            return View();
        }

        public IActionResult Edit()
        {
            // edit an existing request
            // Managers can also approve or deny requests here
            return View();
        }

        public IActionResult ShowSchedule()
        {
            // shows the calendar with requests on it for the selected employee
            return View();
        }
    }
}
