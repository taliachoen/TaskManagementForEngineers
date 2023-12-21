namespace Dal;
using DalApi;

sealed public class DalList : IDal
{

    //Private static field for each interface
    public IDependensy Dependensy => new DependensyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    //Implementation of a reset operation that deletes all data
    public void Reset()
    {
        DataSource.Engineers.Clear();
        DataSource.Dependensies.Clear();
        DataSource.Tasks.Clear();
    }

    //Implementation of return operations of project start and end dates
    public DateTime? ReturnEndProject()
    {
        return DataSource.Config.endProject;
    }

    public DateTime? ReturnStartProject()
    {
        return DataSource.Config.startProject;
    }

    //Implementation of update operations of the start and end date of a project
    public void UpdateStartProject(DateTime? value)
    {
        DataSource.Config.startProject = value;
    }

    public void UpdateEndProject(DateTime? value)
    {
        DataSource.Config.endProject = value;
    }
}

