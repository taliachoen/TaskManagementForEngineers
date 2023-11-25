
namespace DO;
/// <summary>
/// יישות מהנס 
/// </summary>
/// <param name="Id">מספר מזהה ייחודי </param>
/// <param name="Email">כתובת דוא"ל</param>
/// <param name="Cost">עלות לשעה</param>
/// <param name="Name">שם המהנדס</param>
/// <param name="Level">רמת המהנדס</param>
public record Engineer
(
    int Id,
    string ?Email = null,
    double ?Cost = null,
    string ?Name = null,
    DO.EngineerExperience ?Level = null
)

{
    public Engineer() : this(0) { }
}