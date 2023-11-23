

namespace Dal;

internal static class DataSource
{
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependensy> Dependensies { get; } = new();

    internal static class Config
    {
        internal const int IdD = 1000;
        private static int nextDependensyId = IdD;
        internal static int NextDependensyId { get => nextDependensyId++; }
       
        internal const int IdT = 1000;
        private static int nextTaskId = IdT;
        internal static int NextTaskId { get => nextTaskId++; }
    }

}
