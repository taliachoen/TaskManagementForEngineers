

namespace Dal;

internal static class DataSource
{
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependensy> Dependensies { get; } = new();

    internal static class Config
    {
        //Project start date attribute
        internal static DateTime? startProject = null;

        //The scheduled project completion date attribute
        internal static DateTime? endProject = null;

        //Creating a unique ID value for the dependency
        internal const int IdD = 1000;
        private static int nextDependensyId = IdD;

        //Returning a unique identifier value for the dependency
        internal static int NextDependensyId { get => nextDependensyId++; }

        //creating a unique ID value for the task
        internal const int IdT = 10;
        private static int nextTaskId = IdT;

        //Return a unique ID value for the task
        internal static int NextTaskId { get => nextTaskId++; }
    }

}
