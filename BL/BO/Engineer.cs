namespace BO;

public class Engineer
{
    /// <summary>
    /// Gets or sets the unique identifier for the engineer.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the name of the engineer.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the engineer.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the experience level of the engineer.
    /// </summary>
    public EngineerExperience? Level { get; set; }

    /// <summary>
    /// Gets or sets the hourly salary for the engineer.
    /// </summary>
    public double? Cost { get; set; }

    /// <summary>
    /// Gets or sets the task assigned to the engineer.
    /// </summary>
    public TaskInEngineer? Task { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the object.
    /// </summary>
    /// <returns>A string representation of the Engineer object.</returns>
    public override string ToString() => this.ToStringProperty();
}
