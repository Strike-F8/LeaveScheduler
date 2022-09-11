namespace LeaveScheduler.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }

        public int EmployeeID { get; set; }

        public int RequestID { get; set; }

        public String NotificationType { get; set; }

        public DateTime NotificationDate { get; set; }
    }
}
