using MessagePack;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveScheduler.Models
{
    public class EmployeeManager
    {
        public int ID { get; set; }

        [Column(Order = 0)]
        public int ManagerID { get; set; }

        [Column(Order = 1)]
        public int EmployeeID { get; set; }
    }
}
