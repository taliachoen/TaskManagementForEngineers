namespace Dal;
using DalApi;
using DO;
using System.Text;


internal class DependencyImplementation : IDependency
{
    //Creating the CRUD operations for dependencies

    public int Create(Dependency item)
    {
        int newID = DataSource.Config.NextDependencyId;
        Dependency newItem = new()
        {
            Id = newID,
            DependentTask = item.DependentTask,
            DependsOnTask = item.DependsOnTask
        };
        DataSource.Dependencies.Add(newItem);
        return newID;
    }

    public void Delete(int id)
    {
        Dependency? DependencyToDelete = DataSource.Dependencies.FirstOrDefault(x => x.Id == id) ?? throw new DalDoesNotExistException("Object of type Dependency with such Id does not exist.");
        DataSource.Dependencies.Remove(DependencyToDelete);
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(x => x.Id == id) ?? throw new DalDoesNotExistException("Object of type Dependency with such Id does not exist.");
    }

    public void Update(Dependency item)
    {
        Dependency? existingDependency = DataSource.Dependencies.FirstOrDefault(x => x.Id == item.Id) ?? throw new DalDoesNotExistException("Object of type Dependency with such Id does not exist.");
        DataSource.Dependencies.Remove(existingDependency);
        DataSource.Dependencies.Add(item);
    }

    // Reads all entity objects
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;
    }

    // A read operation that receives a function
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }
}

