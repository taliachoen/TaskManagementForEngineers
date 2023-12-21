
namespace Dal;
using DalApi;
using DO;
internal class EngineerImplementation : IEngineer
{
    //Creating the CRUD operations for the engineer

    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Any(x => x.Id == item.Id))
        {
            throw new DalAlreadyExistsException("Object of type Engineer with such Id already exists.");
        }
        else
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }
    }

    public void Delete(int id)
    {

        Engineer? engineerToDelete = DataSource.Engineers.FirstOrDefault(x => x.Id == id) ?? throw new DalDoesNotExistException("Object of type Engineer with such Id does not exist.");

        // Checking whether there are tasks that depend on this engineer
        if (DataSource.Tasks.Any(x => x.EngineerId == id))
        {
            throw new DalDeletionImpossible("It is not possible to delete the engineer because he has existing tasks");
        }

        DataSource.Engineers.Remove(engineerToDelete);
    }

    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(x => x.Id == id);
        return engineer;
    }

    public void Update(Engineer item)
    {
        Engineer? existingEngineer = DataSource.Engineers.FirstOrDefault(x => x.Id == item.Id) ?? throw new DalDoesNotExistException("Object of type Engineer with such Id does not exist.");
        DataSource.Engineers.Remove(existingEngineer);
        DataSource.Engineers.Add(item);
    }

    // Reads all entity objects
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }

    // A read operation that receives a function
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }
}

