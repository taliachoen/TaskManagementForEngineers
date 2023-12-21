
namespace DO;
/// <summary>
/// An entity that contains the attributes of the engineer
/// </summary>
/// <param name="Id">Unique ID number </param>
/// <param name="Email">Email</param>
/// <param name="Cost">cost per hour</param>
/// <param name="Name">The name of the engineer</param>
/// <param name="Level">Engineer level</param>
public record Engineer
(
    int Id,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    DO.EngineerExperience? Level = null
)

{
    public Engineer() : this(0) { }
}