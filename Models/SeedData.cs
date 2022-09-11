using LeaveScheduler.Data;
using Microsoft.EntityFrameworkCore;

namespace LeaveScheduler.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LeaveSchedulerContext(serviceProvider.GetRequiredService<DbContextOptions<LeaveSchedulerContext>>()))
            {
                if (!context.Employee.Any())
                {
                    context.Employee.AddRange(
                        new Employee
                        {
                            FirstName = "Bob",
                            LastName = "Smith",
                            AvailableLeaveTime = 14
                        },
                        new Employee
                        {
                            FirstName = "George",
                            LastName = "Thomas",
                            AvailableLeaveTime = 14
                        },
                        new Employee
                        {
                            FirstName = "Emma",
                            LastName = "Rogers",
                            AvailableLeaveTime = 14
                        },
                        new Employee
                        {
                            FirstName = "Carl",
                            LastName = "Smith",
                            AvailableLeaveTime = 14
                        },
                        new Employee
                        {
                            FirstName = "Jane",
                            LastName = "Doe",
                            AvailableLeaveTime = 14
                        }
                        );

                    context.Manager.AddRange(
                        new Manager
                        {
                            // set Jane Doe as a manager
                            EmployeeID = 5
                        },
                        new Manager
                        {
                            // set Carl Smith as a manager
                            EmployeeID = 4
                        }
                        );

                    context.EmployeeManager.AddRange(
                        new EmployeeManager
                        {
                            // set Jane Doe as manager of Bob Smith
                            ManagerID = 1,
                            EmployeeID = 1
                        },
                        new EmployeeManager
                        {
                            // set Jane Doe as manager of George Thomas
                            ManagerID = 1,
                            EmployeeID = 2
                        },
                        new EmployeeManager
                        {
                            // set Jane Doe as manager of Emma Rogers
                            ManagerID = 1,
                            EmployeeID = 3
                        },
                        new EmployeeManager
                        {
                            // set Carl Smith as manager of Jane Doe
                            ManagerID = 2,
                            EmployeeID = 5
                        }
                        );

                    context.User.AddRange(
                        new User
                        {
                            // Create login for Bob Smith
                            EmployeeID = 6,
                            UserName = "bob.smith",
                            Password = "bobpassword"
                        },
                        new User
                        {
                            EmployeeID = 7,
                            UserName = "george.thomas",
                            Password = "georgepassword",
                        },
                        new User
                        {
                            EmployeeID = 8,
                            UserName = "emma.rogers",
                            Password = "emmapassword"
                        },
                        new User
                        {
                            EmployeeID = 9,
                            UserName = "carl.smith",
                            Password = "carlpassword"
                        },
                        new User
                        {
                            EmployeeID = 10,
                            UserName = "jane.doe",
                            Password = "janepassword"
                        }
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
