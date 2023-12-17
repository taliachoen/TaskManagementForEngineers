
namespace Dal;
using DalApi;
using DO;
using System.Text;

//Creating the CRUD operations for dependencies
internal class DependensyImplementation : IDependensy
{
    public int Create(Dependensy item)
    {
        int newID = DataSource.Config.NextDependensyId;
        Dependensy newItem = new()
        {
            Id = newID,
            DependentTask = item.DependentTask,
            DependsOnTask = item.DependsOnTask
        };
        DataSource.Dependensies.Add(newItem);
        return newID;
    }

    public void Delete(int id)
    {
        Dependensy? dependensyToDelete = DataSource.Dependensies.FirstOrDefault(x => x.Id == id);

        if (dependensyToDelete == null)
        {
            throw new Exception("Object of type Dependensy with such Id does not exist.");
        }

        DataSource.Dependensies.Remove(dependensyToDelete);
    }

    public Dependensy? Read(int id)
    {
        return DataSource.Dependensies.FirstOrDefault(x => x.Id == id);
    }

    public List<Dependensy> ReadAll()
    {
        return new List<Dependensy>(DataSource.Dependensies);
    }

    public void Update(Dependensy item)
    {
        Dependensy? existingDependensy = DataSource.Dependensies.FirstOrDefault(x => x.Id == item.Id) ?? throw new Exception("Object of type Dependensy with such Id does not exist.");
        DataSource.Dependensies.Remove(existingDependensy);
        DataSource.Dependensies.Add(item);
    }
}


   

