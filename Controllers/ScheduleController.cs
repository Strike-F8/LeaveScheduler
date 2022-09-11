using LeaveScheduler.Data;
using LeaveScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveScheduler.Controllers
{
    public class ScheduleController : Controller
    {

        private readonly LeaveSchedulerContext _context;

        public ScheduleController(LeaveSchedulerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return _context.Employee != null ?
                        View(await _context.Employee.ToListAsync()) :
                        Problem("Entity set 'LeaveSchedulerContext.Employee' is null.");
        }

        public ViewResult Details(int? id)
        {
            ViewData["name"] = _context.Employee.Find(id).FirstName + " " +
                _context.Employee.Find(id).LastName;
            ViewData["id"] = id;
            ViewData["days"] = _context.Employee.Find(id).AvailableLeaveTime;

            List<EmpRequestMiniViewModel> model = new List<EmpRequestMiniViewModel>();
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
