
namespace DalApi;

public interface ICrud<T> where T : class
{
    
    //Creates new entity object in DAL 
    int Create(T item);
    //Reads entity object by its ID 
    T? Read(int id);
    // Reads all entity objects 
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);
    //Updates entity object 
    void Update(T item);
    //Deletes an object by its Id 
    void Delete(int id);
    // A read operation that receives a function
    T? Read(Func<T, bool> filter); 


}

