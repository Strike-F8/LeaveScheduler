using MessagePack;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveScheduler.Models
{
    public class EmployeeManager
    {
        public EmployeeManager()
        {

        }
        public EmployeeManager(int managerID, int employeeID)
        {
            ManagerID = managerID;
            EmployeeID = employeeID;
        }

        public int ID { get; set; }

        [Column(Order = 0)]
        public int ManagerID { get; set; }

        [Column(Order = 1)]
        public int EmployeeID { get; set; }
    }
}
