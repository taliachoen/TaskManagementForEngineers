namespace DO;
/// <summary>
/// An entity that expresses their tasks and attributes.
/// </summary>
/// <param name="Id">Task identification number</param>
/// <param name="Alias">nickname</param>
/// <param name="Description">Description</param>
/// <param name="CreatedAtDate">Task creation date</param>
/// <param name="RequiredEffortTime">The amount of time required to perform the task.</param>
/// <param name="IsMilestone">Milestone</param>
/// <param name="Copmlexity">The difficulty level of the task</param>
/// <param name="StartDate">Date of commencement of work on the assignment</param>
/// <param name="ScheduledDate">Planned date for starting work</param>
/// <param name="DeadlineDate"> Possible final end date(deadline)</param>
/// <param name="CompleteDate">Actual end date</param>
/// <param name="Deliverables">product</param               
/// <param name="Remarks">Remarks</param>
/// <param name="EngineerId">The engineer ID assigned to the task</param>
public record Task
(
    int Id,
    string? Alias = null,
    string? Description = null,
    DateTime? CreatedAtDate = null,
    TimeSpan? RequiredEffortTime = null,
    bool? IsMilestone = null,
    DO.EngineerExperience? Copmlexity = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null
)
{
    public Task() : this(0) { }
}