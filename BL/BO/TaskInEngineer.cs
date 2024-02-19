namespace BO;

/// <summary>
/// Represents a task in the context of an engineer.
/// </summary>
public class TaskInEngineer
{
    /// <summary>
    /// Gets or sets the unique identifier for the task in an engineer context.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the alias of the task in an engineer context.
    /// </summary>
    public string? Alias { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the object.
    /// </summary>
    /// <returns>A string representation of the TaskInEngineer object.</returns>
    public override string ToString() => this.ToStringProperty();
}
