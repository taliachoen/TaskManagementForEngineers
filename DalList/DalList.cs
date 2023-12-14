namespace Dal;
using DalApi;

sealed public class DalList : IDal
{
    public IDependensy Dependensy => new DependensyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

}

