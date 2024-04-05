namespace BO;

public class Task
{
    /// <summary>
    /// Gets or sets the unique identifier for the task.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the alias of the task.
    /// </summary>
    public string? Alias { get; set; }

    /// <summary>
    /// Gets or sets the date when the task was created.
    /// </summary>
    public DateTime? CreatedAtDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    public Status ?Status { get; set; }

    /// <summary>
    /// Gets or sets the list of dependencies for the task.
    /// </summary>
    public List<TaskInList>? Dependencies { get; set; }

    /// <summary>
    /// Gets or sets the required effort time for the task.
    /// </summary>
    public TimeSpan? RequiredEffortTime { get; set; }

    /// <summary>
    /// Gets or sets the start date of the task.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the scheduled date for the task.
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// Gets or sets the forecasted date for the task.
    /// </summary>
    public DateTime? ForecastDate { get; set; }

    /// <summary>
    /// Gets or sets the completion date of the task.
    /// </summary>
    public DateTime? CompleteDate { get; set; }

    /// <summary>
    /// Gets or sets the deliverables associated with the task.
    /// </summary>
    public string? Deliverables { get; set; }

    /// <summary>
    /// Gets or sets remarks related to the task.
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// Gets or sets the engineer assigned to the task.
    /// </summary>
    public EngineerInTask? Engineer { get; set; }

    /// <summary>
    /// Gets or sets the complexity level of the task.
    /// </summary>
    public EngineerExperience ?Copmlexity { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the object.
    /// </summary>
    /// <returns>A string representation of the Task object.</returns>
    public override string ToString() => this.ToStringProperty();
}
