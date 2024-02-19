namespace BO;

public class EngineerInTask
{
    /// <summary>
    /// Gets or sets the unique identifier for the engineer in a task.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the name of the engineer in a task.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the object.
    /// </summary>
    /// <returns>A string representation of the EngineerInTask object.</returns>
    public override string ToString() => this.ToStringProperty();
}
