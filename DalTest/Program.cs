using Dal;
using DalApi;
using DO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DalTest
{
    internal class Program
    {
        //TRY 
        private static IDependensy? s_dalDependensy = new DependensyImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static int? id;

        //פעולה שמדפיסה את התפריט הראשי

        private static void PrintMainMenu()
        {
            Console.WriteLine("1. Dependensy");
            Console.WriteLine("2. Engineer");
            Console.WriteLine("3. Task");
        }

        //קבלת הבחירה של המשתמש בתפריט 
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

        //תפריט CRUD
        private static void PrintEntityMenu()
        {
            Console.WriteLine("Entity Menu:");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Test Create");
            Console.WriteLine("2. Test Read");
            Console.WriteLine("3. Test Update");
            Console.WriteLine("4. Test Delete");
        }

        //תפריט של ישיות
        private static void DependensyMenu()
        {

            bool exit = false;
            while (!exit)
            {
                PrintEntityMenu();
                int DependensyChoice = GetUserChoice("Choose a CRUD operation or 0 to exit: ");

                switch (DependensyChoice)
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        // Test Create operation
                        Dependensy dependensy = new()
                        {
                            DependentTask = int.Parse(Console.ReadLine()?? ""),
                            DependsOnTask = int.Parse(Console.ReadLine() ?? "")
                        };
                        id = s_dalDependensy?.Create(dependensy);
                        break;
                    case 2:
                        // Test Read operation
                        Console.WriteLine("Enter id to read: ");
                        int idToFind = int.Parse(Console.ReadLine() ?? "");
                        Console.WriteLine(s_dalDependensy!.Read(idToFind));
                         break;
                    case 3:
                        // Test ReadAll operation
                        Console.WriteLine("Organs in the entity:");
                        Console.WriteLine(s_dalDependensy!.ReadAll());
                        break;
                    case 4:
                        // Test Update operation
                        Console.WriteLine("enter the proporties of dependensy");
                        Dependensy dependensy1 = new()
                        {
                            DependentTask = int.Parse(Console.ReadLine() ?? ""),
                            DependsOnTask = int.Parse(Console.ReadLine() ?? "")
                        };
                        s_dalDependensy!.Update(dependensy1);
                        break;
                    case 5:
                        // Test Delete operation
                        Console.WriteLine("Enter an ID to delete");
                        int DeletionID = int.Parse(Console.ReadLine() ?? "");
                        s_dalDependensy!.Delete(DeletionID);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void EngineerMenu()
        {

            bool exit = false;
            while (!exit)
            {
                PrintEntityMenu();
                 int entityChoice = GetUserChoice("Choose a CRUD operation or exit: ");

                switch (entityChoice)
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        // Test Create operation
                        Console.WriteLine("enter the proporties of engineer");
                        Engineer engineer = new()
                        {
                            Email = Console.ReadLine(),
                            Cost = double.Parse(Console.ReadLine() ?? ""),
                            Name = Console.ReadLine()
                        };
                         id = s_dalEngineer?.Create(engineer);

                        break;
                    case 2:
                        // Test Read operation
                        Console.WriteLine("Enter id to read: ");
                        int idToFind = int.Parse(Console.ReadLine() ?? "");
                        Console.WriteLine(s_dalEngineer!.Read(idToFind));
                        break;
                    case 3:
                        // Test ReadAll operation
                        Console.WriteLine("Organs in the entity:");
                        Console.WriteLine(s_dalEngineer!.ReadAll());
                        break;
                    case 4:
                        // Test Update operation
                        Console.WriteLine("enter the proporties of engineer");
                        Engineer engineer1 = new()
                        {
                            Id = int.Parse(Console.ReadLine() ?? ""),
                            Email = Console.ReadLine(),
                            Cost = double.Parse(Console.ReadLine() ?? ""),
                            Name = Console.ReadLine()
                        };
                        s_dalEngineer!.Update(engineer1);
                        break;
                    case 5:
                        // Test Delete operation
                        Console.WriteLine("Enter an ID to delete");
                        int DeletionID = int.Parse(Console.ReadLine()?? "");
                        s_dalEngineer!.Delete(DeletionID);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                    break;
                }
            }
        }

        private static void TaskMenu()
        {
  
            bool exit = false;
            while (!exit)
            {
                PrintEntityMenu();
                int entityChoice = GetUserChoice("Choose a CRUD operation or exit: ");

                switch (entityChoice)
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        // Test Create operation
                        Console.WriteLine("enter the proporties of task");
                        DO.Task task = new DO.Task()
                        {
                            Alias = Console.ReadLine(),
                            Description = Console.ReadLine(),
                            RequiredEffortTime = TimeSpan.Parse(Console.ReadLine()?? ""),
                            IsMilestone = bool.Parse(Console.ReadLine() ?? ""),
                           // DO.EngineerExperience? Copmlexity =Console.ReadLine(),
                            StartDate = DateTime.Parse(Console.ReadLine() ?? ""),
                            ScheduledDate = DateTime.Parse(Console.ReadLine() ?? "" ),
                            DeadlineDate = DateTime.Parse(Console.ReadLine() ?? ""),
                            CompleteDate = DateTime.Parse(Console.ReadLine()     ?? ""),
                            Deliverables = Console.ReadLine() ?? "",
                            Remarks = Console.ReadLine() ?? "",
                            EngineerId = int.Parse(Console.ReadLine() ?? "" )
                        };
                        id = s_dalTask?.Create(task);
                        break;
                    case 2:
                        // Test Read operation
                        Console.WriteLine("Enter id to read: ");
                        int idToFind = int.Parse(Console.ReadLine() ?? "");
                        Console.WriteLine(s_dalTask!.Read(idToFind));
                        break;
                    case 3:
                        // Test ReadAll operation
                        Console.WriteLine("Organs in the entity:");
                        Console.WriteLine(s_dalTask!.ReadAll());
                        break;
                    case 4:
                        // Test Update operation
                        Console.WriteLine("enter the proporties of task");
                        DO.Task newTask = new DO.Task()
                        {
                            Id=int.Parse(Console.ReadLine() ?? ""),
                            Alias = Console.ReadLine(),
                            Description = Console.ReadLine(),
                            RequiredEffortTime = TimeSpan.Parse(Console.ReadLine() ?? ""),
                            IsMilestone = bool.Parse(Console.ReadLine() ?? ""),
                            // DO.EngineerExperience? Copmlexity =Console.ReadLine(),
                            StartDate = DateTime.Parse(Console.ReadLine() ?? ""),
                            ScheduledDate = DateTime.Parse(Console.ReadLine() ?? ""),
                            DeadlineDate = DateTime.Parse(Console.ReadLine() ?? ""),
                            CompleteDate = DateTime.Parse(Console.ReadLine() ?? ""),
                            Deliverables = Console.ReadLine() ?? "",
                            Remarks = Console.ReadLine() ?? "",
                            EngineerId = int.Parse(Console.ReadLine() ?? "")
                        };
                        s_dalTask!.Update(newTask);
                        break;
                    case 5:
                        // Test Delete operation
                        Console.WriteLine("Enter an ID to delete");                       
                        int DeletionID = int.Parse(Console.ReadLine() ?? "");
                        s_dalEngineer!.Delete(DeletionID);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void Main()
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
                        //int.TryParse(Console.ReadLine(), out menuChoice);
                        DependensyMenu();
                        break;
                    case 2:
                        //int.TryParse(Console.ReadLine(), out menuChoice);
                        EngineerMenu();
                        break;
                    case 3:
                        //int.TryParse(Console.ReadLine(), out menuChoice);
                        TaskMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }



        //CATCH
    }

}






