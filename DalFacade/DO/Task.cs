namespace DO;
/// <summary>
/// יישות המבטאה את המשימות והתכונות שלהם
/// </summary>
/// <param name="Id">מספר זיהוי של המשימה</param>
/// <param name="Alias">כינוי של המשימה</param>
/// <param name="Description">תיאור המשימה</param>
/// <param name="CreatedAtDate">תאריך יצירת המשימה</param>
/// <param name="RequiredEffortTime">זמן נדרש למשימה</param>
/// <param name="IsMilestone"> אבן דרך</param>    ??????????????????????????????
/// <param name="Copmlexity">מורכבות המשימה</param>
/// <param name="StartDate">תאריך התחלה</param>
/// <param name="ScheduledDate">תאריך מתוכנן לסיום המשימה</param>
/// <param name="DeadlineDate">דד ליין למשימה</param>
/// <param name="CompleteDate">תאריך סיום המשימה</param>
/// <param name="Deliverables">תוצר</param                 ????????????????????
/// <param name="Remarks">הערות</param>
/// <param name="EngineerId">מזהה המהנדס</param>
public record Task
(
    int Id,
    string? Alias=null,
    string? Description = null,
    DateTime? CreatedAtDate = null,
    TimeSpan? RequiredEffortTime = null,
    bool? IsMilestone = null,
    DO.EngineerExperience? Copmlexity = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string ?Deliverables = null,
    string ?Remarks = null,
    int? EngineerId = null
)
{
    public Task() : this(0) { }
}