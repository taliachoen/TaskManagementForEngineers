using BlApi;
namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    //Action to update the project start date 
    public DateTime? StartProject { get; set; } = null;
    //Return action to the project end date
    public DateTime? EndProject { get; set; } = null;




/// <summary>
/// Action to delete entity data from the files. 
/// </summary>
public void Reset()
    {
        DalApi.Factory.Get.Reset();
    }


    public void UpdateProjectSchedule(DateTime plannedStartDate)
    {
        // קבל את כל המשימות
        var allTasks = Task.ReadAll();

        // בדוק אם כל תאריכי התחילה מתוכננים של המשימות הקודמות
        foreach (var task in allTasks)
        {
            // אם למשימה יש תלות, בדוק אם תאריך ההתחלה של המשימה הקודמת קיים ואם הוא לא מוקדם מהתאריך הנוכחי
            if (task!.Dependencies!.Any())
            {
                var dependenciesStartDates = task.Dependencies.Select(dep => dep.Status == BO.Status.Done ? dep.CompleteDate : null);
                var maxPreviousStartDate = dependenciesStartDates.Max();
                if (maxPreviousStartDate != null && plannedStartDate < maxPreviousStartDate)
                {
                    throw new InvalidOperationException($"Planned start date for task {task.Id} is earlier than the latest completion date of its preceding tasks.");
                }
            }
            // אם למשימה אין תלות, בדוק שהתאריך המתוכנן גדול או שווה לתאריך ההתחלה של הפרויקט
            else
            {
                if (task.ScheduledDate != null && plannedStartDate < task.ScheduledDate)
                {
                    throw new InvalidOperationException($"Planned start date for task {task.Id} is earlier than the project's planned start date.");
                }
            }
        }

        // אם הבדיקות עברו בהצלחה, עדכן את תאריך ההתחלה המתוכנן לפרויקט
        StartProject = plannedStartDate;

        // עדכן את תאריכי המשימות
        foreach (var task in allTasks)
        {
            // אם המשימה עדיין לא התחילה, עדכן את תאריך ההתחלה שלה לתאריך המתוכנן לפרויקט
            if (task.Status == BO.Status.Unscheduled)
            {
                task.ScheduledDate = plannedStartDate;
                Task.Update(task);
            }
        }
    }

}