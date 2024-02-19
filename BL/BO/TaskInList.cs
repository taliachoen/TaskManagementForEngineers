namespace BO;

public class TaskInList
{
    /// <summary>
    /// Gets or sets the unique identifier for the task in a list.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the description of the task in a list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the alias of the task in a list.
    /// </summary>
    public string? Alias { get; set; }

    /// <summary>
    /// Gets or sets the status of the task in a list.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the object.
    /// </summary>
    /// <returns>A string representation of the TaskInList object.</returns>
    public override string ToString() => this.ToStringProperty();
}
