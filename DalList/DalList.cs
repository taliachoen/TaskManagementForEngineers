namespace Dal;
using DalApi;

sealed public class DalList : IDal
{
    //Private static field for each interface
    public IDependensy Dependensy => new DependensyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();

    /// <summary>
    ///reset operation that deletes all data
    /// </summary>
    public void Reset()
    {
        DataSource.Engineers.Clear();
        DataSource.Dependensies.Clear();
        DataSource.Tasks.Clear();
    }
    /// <summary>
    ///Implementation of return operations of project start and end dates
    /// </summary>
    /// <returns></returns>
    public DateTime? ReturnEndProject()
    {
        return DataSource.Config.endProject;
    }

    public DateTime? ReturnStartProject()
    {
        return DataSource.Config.startProject;
    }
    /// <summary>
    ///Implementation of update operations of the start and end date of a project
    /// </summary>
    /// <param name="value"></param>
    public void UpdateStartProject(DateTime? value) 
    {
        DataSource.Config.startProject=value;
    }

    /// <summary>
    /// Implementation of update operations of the end and end date of a project
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEndProject(DateTime? value)
    {
        DataSource.Config.endProject = value;
    }

}

