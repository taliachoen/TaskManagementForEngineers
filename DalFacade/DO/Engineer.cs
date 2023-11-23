
namespace DO;
/// <summary>
/// יישות מהנס 
/// </summary>
/// <param name="Id">מזהה המהנדס</param>
/// <param name="Email"> מייל המהנדס</param>
/// <param name="Cost">מחיר לשעה</param>
/// <param name="Name">שם המהנדס</param>
/// <param name="Level">שלב המהנדס</param>
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