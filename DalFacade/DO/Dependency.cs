
namespace DO;

/// <summary>
///An entity that save the data about the dependencies of each task/// </summary>
/// <param name="Id">Unique ID number </param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependsOnTask">ID number of previous task</param>
///

public record Dependency
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
)
{
    public Dependency() : this(0) { }
}
