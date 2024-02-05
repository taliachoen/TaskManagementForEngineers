

using BlApi;
using System.Data.Common;
using System.Xml.Linq;

namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public ITask Task => new TaskImplementation();

    /// <summary>
    /// Action to delete entity data from the files. 
    /// </summary>
    public void Reset()
    {
        //DataSource.Engineers.Clear();
        //DataSource.Dependencies.Clear();
        //DataSource.Tasks.Clear();
    }
}