
namespace DO;

/// <summary>
///An entity that save the data about the dependencies of each task/// </summary>
/// <param name="Id">Unique ID number </param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependsOnTask">ID number of previous task</param>
///

public record Dependensy
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
)
{
    public Dependensy() : this(0) { }
}
