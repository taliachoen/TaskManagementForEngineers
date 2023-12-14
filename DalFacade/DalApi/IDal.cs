

namespace DalApi;

public interface IDal
{
    IDependensy Dependensy { get; }
    IEngineer Engineer { get; }
    ITask Task { get; } 
}

