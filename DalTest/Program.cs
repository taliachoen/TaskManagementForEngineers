using Dal;
using DalApi;
using DO;



//DalTest
namespace DalTest
{
    internal class Program
    {
        //A private, read-only, static field of the IDal interface type 
        static readonly IDal s_dal = new DalXml(); 
        
        /// <summary>
        /// Helper method to parse integer input with validation 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Helper method to parse double input with validation 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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
       
        /// <summary>
        /// Helper method to parse DateTime input with validation 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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
       
        /// <summary>
        /// Action that prints the main menu 
        /// </summary>
        private static void PrintMainMenu()
        {
            Console.WriteLine("0. Exit" );
            Console.WriteLine("1. Dependency");
            Console.WriteLine("2. Engineer");
            Console.WriteLine("3. Task");
            Console.WriteLine("4. Initial data");
        }
      
        /// <summary>
        /// Getting the user's selection in the menu 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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
      
        /// <summary>
        /// Print CRUD menu 
        /// </summary>
        /// <param name="entityName"></param>
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
       
        /// <summary>
        /// Dependency's menu
        /// </summary>
        /// <param name="s_dal"></param>
        private static void DependencyMenu(IDal s_dal)
        {
            try
            {
                int? id;
                bool exit = false;
                while (!exit)
                {
                    string entityName = "Dependency";
                    PrintEntityMenu(entityName);
                    int DependencyChoice = GetUserChoice("Choose a CRUD operation or 0 to exit: ");

                    switch (DependencyChoice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            // Create operation
                            Console.WriteLine("Insert a DependentTask and DependsOnTask (DependentTask, DependsOnTask)");
                            Dependency DependencyCreate = new()
                            {
                                DependentTask = GetIntInput("Enter DependentTask: "),
                                DependsOnTask = GetIntInput("Enter DependsOnTask: ")
                            };
                            id = s_dal.Dependency?.Create(DependencyCreate);
                            Console.WriteLine("Succeeded");
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_dal.Dependency!.Read(idToFind));
                            break;
                        case 3:


                            // ReadAll operation
                            Console.WriteLine("All Dependencies:");
                            List<Dependency> Dependencies = s_dal.Dependency!.ReadAll().ToList();
                            foreach (var DependencyReadAll in Dependencies)
                            {
                                Console.WriteLine(DependencyReadAll);
                            }
                            break;
                        case 4:
                            // Update operation
                            Console.WriteLine("Enter the properties of Dependency (id, DependentTask, DependsOnTask)");
                            Dependency DependencyUpdate = new()
                            {
                                Id = GetIntInput("Enter ID: "),
                                DependentTask = GetIntInput("Enter DependentTask: "),
                                DependsOnTask = GetIntInput("Enter DependsOnTask: ")
                            };
                            s_dal.Dependency!.Update(DependencyUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_dal.Dependency!.Delete(DeletionID);
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
       
        /// <summary>
        /// Engineer's menu 
        /// </summary>
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
                            id = s_dal.Engineer?.Create(engineerCreate);
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_dal.Engineer!.Read(idToFind));
                            break;
                        case 3:
                            // ReadAll operation
                            Console.WriteLine("All Dependencies:");
                            List<Engineer> engineers = s_dal.Engineer!.ReadAll().ToList();
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
                            s_dal.Engineer!.Update(engineerUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_dal.Engineer!.Delete(DeletionID);
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
      
        /// <summary>
        /// Task's menu 
        /// </summary>
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
                            id = s_dal.Task?.Create(taskCreate);
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_dal.Task!.Read(idToFind));
                            break;
                        case 3:
                            // ReadAll operation
                            Console.WriteLine("All Task:");
                            List<DO.Task> tasks = s_dal.Task!.ReadAll().ToList();
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
                            s_dal.Task!.Update(taskUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_dal.Task!.Delete(DeletionID);
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
      
        /// <summary>
        /// Main program 
        /// </summary>
        /// <exception cref="FormatException"></exception>
        private static void Main()
        {
            try
            {
                bool exit = false;
                while (!exit)
                {
                    PrintMainMenu();
                    int menuChoice = GetUserChoice("Enter your choice: ");
                    switch (menuChoice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            DependencyMenu(s_dal);
                            break;
                        case 2:
                            EngineerMenu();
                            break;
                        case 3:
                            TaskMenu();
                            break;
                        case 4:
                            Console.Write("Would you like to create Initial data? (Y/N)"); 
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            if (ans == "Y"||ans=="y")
                            {
                                s_dal.Reset();  
                                Initialization.Do(s_dal);
                                Console.WriteLine("The operation was successful");
                            }
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






