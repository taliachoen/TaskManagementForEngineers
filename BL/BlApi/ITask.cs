

namespace BlApi;

public interface ITask
{

    /// <summary>
    /// בקשת רשימת משימות
    /// </summary>
    /// <returns>רשימת משימות</returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
    /// <summary>
    /// בקשת רשימת משימות עם סינון על פי רמת המהנדס
    /// </summary>
    /// <param name="engineerLevel">הרמה של המהנדס</param>
    /// <returns>רשימת משימות לפי הסינון</returns>
    // public IEnumerable<Task> GetTasksByEngineerLevel(int engineerLevel);

    /// <summary>
    /// בקשת פרטי משימה
    /// </summary>
    /// <param name="taskId">מזהה המשימה</param>
    /// <returns>אובייקט משימה שלפי מזהה</returns>
    public BO.Task Read(int taskId);
    /// <summary>
    /// הוספת משימה
    /// </summary>
    /// <param name="newTask">אובייקט משימה להוספה</param>
    public int Create(BO.Task newTask);

    /// <summary>
    /// עדכון משימה
    /// </summary>
    /// <param name="updatedTask">אובייקט משימה עם הנתונים המעודנים</param>
    public void Update(BO.Task updatedTask);

    /// <summary>
    /// מחיקת משימה
    /// </summary>
    /// <param name="taskId">מזהה המשימה למחיקה</param>
    public void Delete(int taskId);

}
