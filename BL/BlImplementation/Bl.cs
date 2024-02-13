

using BlApi;
using System.Data.Common;
using System.Xml.Linq;

namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? StartProject { get => StartProject; set => StartProject = value; }
    public DateTime? EndProject { get => EndProject; set => EndProject = value; }

    /// <summary>
    /// Action to delete entity data from the files. 
    /// </summary>
    public void Reset()
    {
        DalApi.Factory.Get.Reset();
    }

  
}