namespace BlTest
{

    //בדיקת תקינות למחרוזת
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


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
            Console.WriteLine("0. Exit");
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
                            BO.Engineer engineerCreate = new()
                            {
                                Id = GetIntInput("Enter ID: "),
                                Email = Console.ReadLine(),
                                Cost = GetDoubleInput("Enter Cost: "),
                                Name = Console.ReadLine(),
                                Level = (BO.EngineerExperience)GetIntInput("Enter Complexity: "),
                                Task = null
                            };
                            id = s_bl.Engineer.Create(engineerCreate);
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_bl.Engineer!.Read(idToFind));
                            break;
                        case 3:
                            // ReadAll operation
                            Console.WriteLine("All Dependencies:");
                            List<BO.Engineer> engineers = s_bl.Engineer!.ReadAll().ToList();
                            foreach (var engineerReadAll in engineers)
                            {
                                Console.WriteLine(engineerReadAll);
                            }
                            break;
                        case 4:
                            // Update operation

                            BO.Engineer engineerUpdate;
                            Console.WriteLine("   y / n  האם אתה רוצה לעדכן את המשימות שלך בנוסף??");
                            string ?YORn = Console.ReadLine();
                            if (YORn == "n" || YORn=="N")
                            {
                               Console.WriteLine("Enter the properties of engineer (id, email, cost, name,level )");
                                engineerUpdate = new()
                                {
                                    Id = GetIntInput("Enter ID: "),
                                    Email = Console.ReadLine(),
                                    Cost = GetDoubleInput("Enter Cost: "),
                                    Name = Console.ReadLine(),
                                    Level = (BO.EngineerExperience)GetIntInput("Enter Complexity: "),
                                };
                            }
                            else
                            {
                                Console.WriteLine("Enter the properties of engineer (id, email, cost, name,level,task {id, alias} )");
                                engineerUpdate = new()
                                {
                                    Id = GetIntInput("Enter ID: "),
                                    Email = Console.ReadLine(),
                                    Cost = GetDoubleInput("Enter Cost: "),
                                    Name = Console.ReadLine(),
                                    Level = (BO.EngineerExperience)GetIntInput("Enter Complexity: "),
                                    Task = new BO.TaskInEngineer { Id = GetIntInput("Enter id and alias of your task: "), Alias = Console.ReadLine() }
                                };

                            }
                            s_bl.Engineer!.Update(engineerUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_bl.Engineer!.Delete(DeletionID);
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
                int id;
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
                            Console.WriteLine("(Alias, Description, RequiredEffortTime, StartDate, ScheduledDate, DeadlineDate, CompleteDate, Deliverables, Remarks, EngineerId)");
                            BO.Task taskCreate = new()
                            {
                                Alias = Console.ReadLine(),
                                Description = Console.ReadLine(),
                                RequiredEffortTime = TimeSpan.FromHours(GetDoubleInput("Enter RequiredEffortTime (in hours): ")),
                                Copmlexity = (BO.EngineerExperience)GetIntInput("Enter Complexity: "),
                                StartDate = GetDateTimeInput("Enter StartDate: "),
                                ScheduledDate = GetDateTimeInput("Enter ScheduledDate: "),
                                CompleteDate = GetDateTimeInput("Enter CompleteDate: "),
                                Deliverables = Console.ReadLine(),
                                Remarks = Console.ReadLine(),
                                Engineer = new BO.EngineerInTask { Id = GetIntInput("Enter EngineerId and Engineer name: "), Name = Console.ReadLine() }
                            };
                            id = s_bl.Task.Create(taskCreate);
                            break;
                        case 2:
                            // Read operation
                            Console.WriteLine("Enter id to read: ");
                            int idToFind = GetIntInput("Enter ID: ");
                            Console.WriteLine(s_bl.Task.Read(idToFind));
                            break;
                        case 3:
                            // ReadAll operation
                            Console.WriteLine("All Task:");
                            List<BO.Task> tasks = s_bl.Task.ReadAll().ToList();
                            foreach (var taskReadAll in tasks)
                            {
                                Console.WriteLine(taskReadAll);
                            }
                            break;
                        case 4:
                            // Update operation
                            Console.WriteLine("Enter the properties of task");
                            BO.Task taskUpdate = new()
                            {
                                Id = GetIntInput("Enter ID: "),
                                Alias = Console.ReadLine(),
                                Description = Console.ReadLine(),
                                RequiredEffortTime = TimeSpan.FromHours(GetDoubleInput("Enter RequiredEffortTime (in hours): ")),
                                Copmlexity = (BO.EngineerExperience)GetIntInput("Enter Complexity: "),
                                StartDate = GetDateTimeInput("Enter StartDate: "),
                                ScheduledDate = GetDateTimeInput("Enter ScheduledDate: "),
                                CompleteDate = GetDateTimeInput("Enter CompleteDate: "),
                                Deliverables = Console.ReadLine(),
                                Remarks = Console.ReadLine(),
                                Engineer = new BO.EngineerInTask { Id = GetIntInput("Enter EngineerId and Engineer name: "), Name = Console.ReadLine() }
                            };
                            s_bl.Task!.Update(taskUpdate);
                            break;
                        case 5:
                            // Delete operation
                            Console.WriteLine("Enter an ID to delete");
                            int DeletionID = GetIntInput("Enter ID: ");
                            s_bl.Task!.Delete(DeletionID);
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
                            //DependencyMenu(s_bl);
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
                            if (ans == "Y" || ans == "y")
                            {
                                s_bl.Reset();
                                DalTest.Initialization.Do();
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

