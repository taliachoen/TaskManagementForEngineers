
namespace DO;

/// <summary>
///ישות ששומרת את הנתונים לגבי כל משימה האם היא תלויה במשימות אחרות או שיש משימות אחרות שתלויות בה
/// </summary>
/// <param name="Id">תלות המשימותהתלות בין המשימה </param>
/// <param name="DependentTask">האם המשימה תלויה במשימות אחרות</param>
/// <param name="DependsOnTask">האם משימות אחרות תלויות במשימה זו</param>
public record Dependensy
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
)
{
    public Dependensy(): this(0) { }
}
