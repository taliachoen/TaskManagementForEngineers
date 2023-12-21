
namespace Dal;
using DalApi;
using DO;
using System.Text;


internal class DependensyImplementation : IDependensy
{
    //Creating the CRUD operations for dependencies

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
        Dependensy? dependensyToDelete = DataSource.Dependensies.FirstOrDefault(x => x.Id == id) ?? throw new DalDoesNotExistException("Object of type Dependensy with such Id does not exist.");
        DataSource.Dependensies.Remove(dependensyToDelete);
    }

    public Dependensy? Read(int id)
    {
        return DataSource.Dependensies.FirstOrDefault(x => x.Id == id) ?? throw new DalDoesNotExistException("Object of type Dependensy with such Id does not exist.");
    }

    public void Update(Dependensy item)
    {
        Dependensy? existingDependensy = DataSource.Dependensies.FirstOrDefault(x => x.Id == item.Id) ?? throw new DalDoesNotExistException("Object of type Dependensy with such Id does not exist.");
        DataSource.Dependensies.Remove(existingDependensy);
        DataSource.Dependensies.Add(item);
    }

    // Reads all entity objects
    public IEnumerable<Dependensy> ReadAll(Func<Dependensy, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependensies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependensies
               select item;
    }

    // A read operation that receives a function
    public Dependensy? Read(Func<Dependensy, bool> filter)
    {
        return DataSource.Dependensies.FirstOrDefault(filter);
    }
}




