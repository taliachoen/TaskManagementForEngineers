
namespace Dal;
using DalApi;
using DO;
//Creating the CRUD operations for the engineer
internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Any(x => x.Id == item.Id))
        {
            throw new Exception("Object of type Engineer with such Id already exists.");
        }
        else
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }
    }

    public void Delete(int id)
    {

        Engineer? engineerToDelete = DataSource.Engineers.FirstOrDefault(x => x.Id == id);

        if (engineerToDelete == null)
        {
            throw new Exception("Object of type Engineer with such Id does not exist.");
        }

        // Checking whether there are tasks that depend on this engineer
        if (DataSource.Tasks.Any(x => x.EngineerId == id))
        {
            throw new Exception("It is not possible to delete the engineer because he has existing tasks");
        }

        DataSource.Engineers.Remove(engineerToDelete);
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(x => x.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        Engineer? existingEngineer = DataSource.Engineers.FirstOrDefault(x => x.Id == item.Id);

        if (existingEngineer == null)
        {
            throw new Exception("Object of type Engineer with such Id does not exist.");
        }

        DataSource.Engineers.Remove(existingEngineer);
        DataSource.Engineers.Add(item);
    }
}

