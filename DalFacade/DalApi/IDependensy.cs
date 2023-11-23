

namespace DalApi;
using DO;
public interface IDependensy
{
    int Create(Dependensy item); //Creates new entity object in DAL
    Dependensy? Read(int id); //Reads entity object by its ID 
    List<Dependensy> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Dependensy item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
