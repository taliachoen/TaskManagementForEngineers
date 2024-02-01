

namespace BlApi;

public interface IEngineer
{
    /// <summary>
    /// בקשת רשימת מהנדסים
    /// </summary>
    /// <returns>רשימת מהנדסים</returns>
    public IEnumerable<BO.Engineer> GetEngineersList();

    /// <summary>
    /// בקשת רשימת מהנדסים עם סינון על פי רמה מסוימת
    /// </summary>
    /// <param name="level">הרמה לסינון</param>
    /// <returns>רשימת מהנדסים לפי הסינון</returns>
    //public IEnumerable<BO.Engineer> GetEngineersByLevel(int level);

    /// <summary>
    /// בקשת פרטי מהנדס על פי מזהה 
    /// </summary>
    /// <param name="engineerId">מזהה המהנדס</param>
    /// <returns>אובייקט מהנדס שלפי מזהה</returns>
    public BO.Engineer GetEngineerDetails(int engineerId);

    /// <summary>
    /// הוספת מהנדס 
    /// </summary>
    /// <param name="newEngineer">אובייקט מהנדס להוספה</param>
    public void AddEngineer(BO.Engineer newEngineer);

    /// <summary>
    /// מחיקת מהנדס 
    /// </summary>
    /// <param name="engineerId">מזהה המהנדס למחיקה</param>
    public void DeleteEngineer(int engineerId);

    /// <summary>
    /// עדכון נתונים של מהנדס
    /// </summary>
    /// <param name="updatedEngineer">אובייקט מהנדס עם הנתונים המעודכנים</param>
    public void UpdateEngineerDetails(BO.Engineer updatedEngineer);

}
