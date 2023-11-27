using Dal;
using DalApi;
using DO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DalTest
{
    internal class Program
    {
        private static IDependensy? s_dalDependensy = new DependensyImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();

        // Helper method to parse integer input with validation
        private static int GetIntInput(string message)
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input. Please enter a correct number.");
                Console.Write(message);
            }
            return input;
        }

        // Helper method to parse double input with validation
        private static double GetDoubleInput(string message)
        {
            double input;
            while (!double.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input. Please enter a correct number.");
                Console.Write(message);
            }
            return input;
        }

        // Helper method to parse DateTime input with validation
        private static DateTime GetDateTimeInput(string message)
        {
            DateTime input;
            while (!DateTime.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input. Please enter a correct date and time.");
                Console.Write(message);
            }
            return input;
        }



        // Action that prints the main menu
        private static void PrintMainMenu()
        {
            Console.WriteLine("1. Dependensy");
            Console.WriteLine("2. Engineer");
            Console.WriteLine("3. Task");
        }

        // Getting the user's selection in the menu
        private static int GetUserChoice(string message)
        {
            Console.Write(message);
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a correct number.");
                Console.Write(message);
            }
            return choice;
        }

        // Print CRUD menu
        private static void PrintEntityMenu(string entityName)
        {
            Console.WriteLine(entityName + "  Menu:");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Read");
            Console.WriteLine("3. Read All");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");
        }

        // Dependensy's menu
        private static void DependensyMenu()
        {
            try
            {
                int? id;
                bool exit = false;
                while (!exit)
                {
                    string entityName = "Dependensy";
                    PrintEntityMenu(entityName);
                    int DependensyChoice = GetUserChoice("Choose a CRUD operation or 0 to exit: ");

                    switch (DependensyChoice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            // Create operation
                            Console.WriteLine("Insert a DependentTask and DependsOnTask (DependentTask, DependsOnTask)");
                            Dependensy dependensyCreate = new()
                            {
                                DependentTask = GetIntInput("Enter DependentTask: "),
                                DependsOnTask = GetIntInput("Enter DependsOnTask: ")
                            };
                            id = s_dalDependensy?.Create(dependensyCreate);
                            Console.WriteLine("Succeeded");
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_dalDependensy!.Read(idToFind));
                            break;
                        case 3:
                            // ReadAll operation
                            Console.WriteLine("All Dependensies:");
                            List<Dependensy> dependensies = s_dalDependensy!.ReadAll();
                            foreach (var dependensyReadAll in dependensies)
                            {
                                Console.WriteLine(dependensyReadAll);
                            }
                            break;
                        case 4:
                            // Update operation
                            Console.WriteLine("Enter the properties of dependensy (id, DependentTask, DependsOnTask)");
                            Dependensy dependensyUpdate = new()
                            {
                                Id = GetIntInput("Enter ID: "),
                                DependentTask = GetIntInput("Enter DependentTask: "),
                                DependsOnTask = GetIntInput("Enter DependsOnTask: ")
                            };
                            s_dalDependensy!.Update(dependensyUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_dalDependensy!.Delete(DeletionID);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }
        }

        // Engineer's menu
        private static void EngineerMenu()
        {
            try
            {
                int? id;
                bool exit = false;
                while (!exit)
                {
                    string entityName = "Engineer";
                    PrintEntityMenu(entityName);
                    int entityChoice = GetUserChoice("Choose a CRUD operation or exit: ");
                    switch (entityChoice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            // Create operation
                            Console.WriteLine("Enter the properties of engineer (id, email, cost, name,level)");
                            Engineer engineerCreate = new()
                            {
                                Id = GetIntInput("Enter ID: "),
                                Email = Console.ReadLine(),
                                Cost = GetDoubleInput("Enter Cost: "),
                                Name = Console.ReadLine(),
                                Level = (DO.EngineerExperience)GetIntInput("Enter Complexity: ")
                            };
                            id = s_dalEngineer?.Create(engineerCreate);
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_dalEngineer!.Read(idToFind));
                            break;
                        case 3:
                            // ReadAll operation
                            Console.WriteLine("All Dependensies:");
                            List<Engineer> engineers = s_dalEngineer!.ReadAll();
                            foreach (var engineerReadAll in engineers)
                            {
                                Console.WriteLine(engineerReadAll);
                            }
                            break;
                        case 4:
                            // Update operation
                            Console.WriteLine("Enter the properties of engineer (id, email, cost, name,level)");
                            Engineer engineerUpdate = new()
                            {
                                Id = GetIntInput("Enter ID: "),
                                Email = Console.ReadLine(),
                                Cost = GetDoubleInput("Enter Cost: "),
                                Name = Console.ReadLine(),
                                Level = (DO.EngineerExperience)GetIntInput("Enter Complexity: ")
                            };
                            s_dalEngineer!.Update(engineerUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_dalEngineer!.Delete(DeletionID);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }
        }

        // Task's menu
        private static void TaskMenu()
        {
            try
            {
                int? id;
                bool exit = false;
                while (!exit)
                {
                    string entityName = "Task";
                    PrintEntityMenu(entityName);
                    int entityChoice = GetUserChoice("Choose a CRUD operation or exit: ");

                    switch (entityChoice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            // Create operation
                            Console.WriteLine("Enter the properties of task");
                            Console.WriteLine("(Alias, Description, RequiredEffortTime, IsMilestone, StartDate, ScheduledDate, DeadlineDate, CompleteDate, Deliverables, Remarks, EngineerId)");
                            DO.Task taskCreate = new()
                            {
                                Alias = Console.ReadLine(),
                                Description = Console.ReadLine(),
                                RequiredEffortTime = TimeSpan.FromHours(GetDoubleInput("Enter RequiredEffortTime (in hours): ")),
                                IsMilestone = GetIntInput("Enter IsMilestone (1 for true, 0 for false): ") == 1,
                                Copmlexity = (DO.EngineerExperience)GetIntInput("Enter Complexity: "),
                                StartDate = GetDateTimeInput("Enter StartDate: "),
                                ScheduledDate = GetDateTimeInput("Enter ScheduledDate: "),
                                DeadlineDate = GetDateTimeInput("Enter DeadlineDate: "),
                                CompleteDate = GetDateTimeInput("Enter CompleteDate: "),
                                Deliverables = Console.ReadLine(),
                                Remarks = Console.ReadLine(),
                                EngineerId = GetIntInput("Enter EngineerId: ")
                            };
                            id = s_dalTask?.Create(taskCreate);
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_dalTask!.Read(idToFind));
                            break;
                        case 3:
                            // ReadAll operation
                            Console.WriteLine("All Task:");
                            List<DO.Task> tasks = s_dalTask!.ReadAll();
                            foreach (var taskReadAll in tasks)
                            {
                                Console.WriteLine(taskReadAll);
                            }
                            break;
                        case 4:
                            // Update operation
                            Console.WriteLine("Enter the properties of task");
                            DO.Task taskUpdate = new()
                            {
                                Id = GetIntInput("Enter ID: "),
                                Alias = Console.ReadLine(),
                                Description = Console.ReadLine(),
                                RequiredEffortTime = TimeSpan.FromHours(GetDoubleInput("Enter RequiredEffortTime (in hours): ")),
                                IsMilestone = GetIntInput("Enter IsMilestone (1 for true, 0 for false): ") == 1,
                                Copmlexity = (DO.EngineerExperience)GetIntInput("Enter Complexity: "),
                                StartDate = GetDateTimeInput("Enter StartDate: "),
                                ScheduledDate = GetDateTimeInput("Enter ScheduledDate: "),
                                DeadlineDate = GetDateTimeInput("Enter DeadlineDate: "),
                                CompleteDate = GetDateTimeInput("Enter CompleteDate: "),
                                Deliverables = Console.ReadLine(),
                                Remarks = Console.ReadLine(),
                                EngineerId = GetIntInput("Enter EngineerId: ")
                            };
                            s_dalTask!.Update(taskUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_dalTask!.Delete(DeletionID);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }
        }

        // Main program
        private static void Main()
        {
            try
            {
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependensy);
                bool exit = false;
                while (!exit)
                {
                    PrintMainMenu();
                    int menuChoice = GetUserChoice("Enter the entity number you want to check or enter 0 to exit: ");
                    switch (menuChoice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            DependensyMenu();
                            break;
                        case 2:
                            EngineerMenu();
                            break;
                        case 3:
                            TaskMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }
        }
    }



}






