
namespace DO;

/// <summary>
///ישות ששומרת את הנתונים לגבי כל משימה האם היא תלויה במשימות אחרות או שיש משימות אחרות שתלויות בה
/// </summary>
/// <param name="Id">מספר מזהה ייחודי </param>
/// <param name="DependentTask">מספר מזהה של משימה תלויה</param>
/// <param name="DependsOnTask">מספר מזהה של משימה קודמת</param>
public record Dependensy
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
)
{
    public Dependensy(): this(0) { }
}
