
namespace Dal;
using DalApi;
using DO;
//Creating the CRUD operations for the engineer
public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? newItem = DataSource.Engineers.Find(x => x.Id == item.Id);
        if (newItem != null)
            throw new Exception("Object of type Engineer with such Id does exist.");
        else
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }
    }

    public void Delete(int id)
    {
        Engineer? newItem = DataSource.Engineers.Find(x => x.Id == id);
        if (newItem == null)
        {
            throw new Exception("Object of type Engineer with such Id does not exist.");
        }
        else
        {
            //Checking whether there are tasks that depend on this engineer
           if (DataSource.Tasks.Any(x => x.EngineerId== id))
              throw new Exception("It is not possible to delete the engineer because he has existing tasks");
           DataSource.Engineers.Remove(newItem);
        }
        
    }

    public Engineer? Read(int id)
    {
        Engineer? newItem = DataSource.Engineers.Find(x => x.Id == id);
        if (newItem != null)
            return newItem;
        return null;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        Engineer? newItem = DataSource.Engineers.Find(x => x.Id == item.Id);
        if (newItem == null)
            throw new Exception("Object of type Engineer with such Id does not exist.");
        else
        {
            DataSource.Engineers.Remove(newItem);
            DataSource.Engineers.Add(item);
        }

    }
}
