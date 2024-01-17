

namespace Dal;

internal static class DataSource
{
    //create the lists
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependensy> Dependensies { get; } = new();

    internal static class Config
    {
        //Project start date 
        internal static DateTime? startProject = null;
        //Project completion date
        internal static DateTime? endProject = null;
        //Creating a unique ID value for the dependency
        internal const int IdD = 1000;
        private static int nextDependensyId = IdD;
        internal static int NextDependensyId { get => nextDependensyId++; }
       //יצירת ערכי מזהה יחודי למשימה
        internal const int IdT = 10;
        private static int nextTaskId = IdT;
        internal static int NextTaskId { get => nextTaskId++; }
    }

}
