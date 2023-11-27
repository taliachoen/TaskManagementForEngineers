
namespace Dal;
using DalApi;
using DO;
using System.Text;

//Creating the CRUD operations for dependencies
public class DependensyImplementation : IDependensy
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
        Dependensy? newItem = DataSource.Dependensies.Find(x => x.Id == id);
        if (newItem == null)
        {
            throw new Exception("Object of type Dependensy with such Id does not exist.");
        }
        else
        {
            DataSource.Dependensies.Remove(newItem);
        }
    }

    public Dependensy? Read(int id)
    {
        Dependensy? newItem = DataSource.Dependensies.Find(x => x.Id == id);
        if (newItem != null)
            return newItem;
        return null;
    }

    public List<Dependensy> ReadAll()
    {
        return new List<Dependensy>(DataSource.Dependensies);
    }

    public void Update(Dependensy item)
{
    Dependensy? newItem = DataSource.Dependensies.Find(x => x.Id == item.Id);
    if (newItem == null)
        throw new Exception("Object of type Dependensy with such Id does not exist.");
    else
    {
        DataSource.Dependensies.Remove(newItem);
        DataSource.Dependensies.Add(item);
    }

}
}
