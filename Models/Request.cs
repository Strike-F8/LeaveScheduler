using MessagePack;

namespace LeaveScheduler.Models
{
    public class Request
    {
        [Key(0)]
        public int RequestID { get; set; }
        [Key(1)]
        public int EmployeeID { get; set; }

        public DateTime RequestDate { get; set; }

        public bool RequestStatus { get; set; }
    }
}
