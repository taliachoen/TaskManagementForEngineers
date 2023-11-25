using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {
        //TRY 
        private static IDependensy? s_dalDependensy = new DependensyImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();

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
        private static void DependensyMenu(int numOfEntityl)
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
                        Console.WriteLine("enter the proporties of dependensy");
                        int id=int.Parse(Console.ReadLine());   
                        int dependsOnTask = int.Parse(Console.ReadLine());
                        int dependentTask = int.Parse(Console.ReadLine());  
                        Dependensy t = new (id,dependentTask,dependsOnTask);
                        s_dalDependensy.Create(t);
                        break;
                    case 2:
                        // Test Read operation
                        TestRead(dal);
                        break;
                    case 3:
                        // Test Update operation
                        TestUpdate(dal);
                        break;
                    case 4:
                        // Test Delete operation
                        TestDelete(dal);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void EngineerMenu(int numOfEntity)
        {

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Engineer Menu:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1.  Create");
                Console.WriteLine("2.  Read");
                Console.WriteLine("3.  ReadAll");
                Console.WriteLine("4.  Update");
                Console.WriteLine("5.  Delete");
                int entityChoice = GetUserChoice("Choose a CRUD operation or exit: ");

                switch (entityChoice)
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        // Test Create operation
                        TestCreate(dal);
                        break;
                    case 2:
                        // Test Read operation
                        TestRead(dal);
                        break;
                    case 3:
                        // Test Update operation
                        TestUpdate(dal);
                        break;
                    case 4:
                        // Test Delete operation
                        TestDelete(dal);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void TaskMenu(int numOfEntity)
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
                        TestCreate(dal);
                        break;
                    case 2:
                        // Test Read operation
                        TestRead(dal);
                        break;
                    case 3:
                        // Test Update operation
                        TestUpdate(dal);
                        break;
                    case 4:
                        // Test Delete operation
                        TestDelete(dal);
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
                        DependensyMenu(menuChoice);
                        break;
                    case 2:
                        //int.TryParse(Console.ReadLine(), out menuChoice);
                        EngineerMenu(menuChoice);
                        break;
                    case 3:
                        //int.TryParse(Console.ReadLine(), out menuChoice);
                        TaskMenu(menuChoice);
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






