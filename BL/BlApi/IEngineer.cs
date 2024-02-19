namespace BlApi;

public interface IEngineer
{
    /// <summary>
    /// Request a list of engineers.
    /// </summary>
    /// <returns>List of engineers</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

    /// <summary>
    /// Request a list of engineers filtered by a specific level.
    /// </summary>
    /// <param name="level">The level for filtering</param>
    /// <returns>List of engineers based on the filter</returns>
    public IEnumerable<BO.Engineer> GetEngineersByLevel(int level);

    /// <summary>
    /// Request details of an engineer based on the identifier.
    /// </summary>
    /// <param name="engineerId">Engineer's identifier</param>
    /// <returns>Engineer object based on the identifier</returns>
    public BO.Engineer Read(int engineerId);

    /// <summary>
    /// Add a new engineer.
    /// </summary>
    /// <param name="newEngineer">Engineer object to add</param>
    public int Create(BO.Engineer newEngineer);

    /// <summary>
    /// Delete an engineer.
    /// </summary>
    /// <param name="engineerId">Engineer's identifier for deletion</param>
    public void Delete(int engineerId);

    /// <summary>
    /// Update engineer's data.
    /// </summary>
    /// <param name="updatedEngineer">Engineer object with updated data</param>
    public void Update(BO.Engineer updatedEngineer);
}
