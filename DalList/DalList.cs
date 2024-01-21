namespace Dal;
using DalApi;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    //Private static field for each interface
    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();

    /// <summary>
    ///reset operation that deletes all data
    /// </summary>
    public void Reset()
    {
        DataSource.Engineers.Clear();
        DataSource.Dependencies.Clear();
        DataSource.Tasks.Clear();
    }
    
    /// <summary>
    ///Implementation of return operations of end dates
    /// </summary>
    /// <returns></returns>
    public DateTime? ReturnEndProject()
    {
        return DataSource.Config.endProject;
    }

    /// <summary>
    /// Implementation of return operations of project start
    /// </summary>
    /// <returns></returns>
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

