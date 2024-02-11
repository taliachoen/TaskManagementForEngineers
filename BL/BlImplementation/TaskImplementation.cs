using BlApi;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private readonly DalApi.IDal dal = DalApi.Factory.Get;

    private static bool ValidateTask(BO.Task newTask)
    {
        if (newTask.Id <= 0)
            throw new BO.BlInvalidDataException("Task ID must be a positive integer.");

        if (string.IsNullOrEmpty(newTask.Alias))
            throw new BO.BlInvalidDataException("Alias cannot be null or empty.");

        return true;
    }

    private static bool CanDeleteTask(int taskId) {
        return taskId == 0;
        //בדיקה האם המשימה לא קודמת למשימות אחרות או האם אנחנו לאחר יצירת לוז
    }

    //private  static BO.Status Status() {}
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)
        {
            return dal.Task.ReadAll().Select(task => Read(task!.Id));
           // return (IEnumerable<BO.Task>)dal.Task.ReadAll();
        }
        else
        {
            return ReadAll().Where(filter);
        }
    }

    //הוספת פונקציות פרטיות להשלמת הפרטים ב-דו
    public BO.Task Read(int taskId) 
    { 
    
        DO.Task? doTask = dal.Task.Read(taskId);
        return doTask == null
            ? throw new BO.BlDoesNotExistException($"Task with ID={taskId} does Not exist")
            : new BO.Task
            {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = (DateTime?)doTask.CreatedAtDate,
            //Status = Status(),
            //Dependencies = doTask.Dependencies,
            RequiredEffortTime = doTask.RequiredEffortTime, 
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            //ForecastDate = doTask.ForecastDate,
            DeadlineDate = doTask.DeadlineDate ,
            CompleteDate = doTask.CompleteDate ,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks ,
           // EngineerId = doTask.EngineerId ,
            //Copmlexity = doTask.Copmlexity
            };
    }
   
    public int Create(BO.Task newTask)
    {
        
        if (!ValidateTask(newTask))
        {
            throw new BO.BlInvalidDataException("Invalid engineer data.");
        }
        //AddDependencies(newTask); // תוסיף תלויות עבור משימות קודמות מתוך רשימת המשימות הקיימת

        try
        {
          int idTask = dal.Task.Create( new DO.Task { 
            Id = newTask.Id,
            Alias = newTask.Alias,
            Description = newTask.Description,
            CreatedAtDate = newTask.CreatedAtDate,
            //Status = doTask.Status,
            //Dependencies = doTask.Dependencies,
            RequiredEffortTime = newTask.RequiredEffortTime,
            Copmlexity = (DO.EngineerExperience?)newTask.Copmlexity,
            StartDate = newTask.StartDate,
            ScheduledDate = newTask.ScheduledDate,
            DeadlineDate = newTask.DeadlineDate,
            CompleteDate = newTask.CompleteDate,
            Deliverables = newTask.Deliverables,
            Remarks = newTask.Remarks,
            EngineerId = newTask?.Engineer?.Id
            //ForecastDate = doTask.ForecastDate,
        } );
             
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={newTask.Id} already exists", ex);
        }

    }

    public void Update(BO.Task updatedTask)
    {
         ValidateTask(updatedTask);
        //UpdateDependencies(updatedTask); // עדכון תלויות
        try
        {
             DO.Task? existingTask = dal.Task.Read(updatedTask.Id);
             dal.Task.Update(existingTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={updatedTask.Id} does not exist", ex);
        }

        //{
        //    // קבלת המהנדס הקיים ממקור הנתונים
        //    DO.Engineer? existingEngineer = dal.Engineer.Read(updatedEngineer.Id);

        //    // עדכון פרטי המהנדס
        //    //existingEngineer.Email = updatedEngineer.Email;
        //    //existingEngineer.Cost = updatedEngineer.Cost;
        //    //existingEngineer.Name = updatedEngineer.Name;
        //    //existingEngineer.Level = (DO.EngineerExperience?)updatedEngineer.Level;

        //    // אם המהנדס רוצה לעדכן גם את המשימה
        //    if (updatedEngineer.Task != null)
        //    {
        //        // קבלת המשימה הקיימת
        //        DO.Task? existingTask = dal.Task.Read(updatedEngineer.Task.Id);

        //        // בדיקה האם המהנדס כרגע עובד על משימה שונה ורוצה לעבוד על משימה אחרת
        //        if (existingTask != null && existingTask.EngineerId != updatedEngineer.Id)
        //        {
        //            //בדיקה האם המשימה הישנה נגמרה
        //            if (existingTask.Status = (Status)5)
        //            {
        //                //המהנדס כבר לא עובד על המשימה
        //                existingTask.EngineerId = null;
        //                dal.Task.Update(existingTask);

        //                //עדכון המשימה החדשה 
        //                BO.Task? newTask = BO.Task.Read(x => x.Id == updatedEngineer.Task.Id);
        //                newTask.Status = "Scheduled";
        //                newTask.EngineerId = updatedEngineer.Id;
        //                dal.Task.Update(newTask);
        //            }
        //        }
        //        else
        //        {
        //            throw new BO.BlInvalidDataException("Engineer cannot change task during an active task.");
        //        }
        //    }
        //    //אם המהנדס לא עבד על משימה עד העדכון הנוכחי
        //    else
        //    {
        //        //חיצוני
        //        //עדכון המשימה החדשה 
        //        BO.Task? newTask = BO.Task.Read(x => x.Id == updatedEngineer.Task.Id);
        //        newTask.Status = "Scheduled";
        //        newTask.EngineerId = updatedEngineer.Id;
        //        dal.Task.Update(newTask);

        //        // אם המהנדס לא היה משוייך למשימה, אז נשייך לו את המשימה החדשה
        //        DO.Task newTask = new DO.Task
        //        {
        //            Id = updatedEngineer.Task.Id,
        //            Status = "Scheduled",
        //            EngineerId = updatedEngineer.Id
        //            // הגדרת שדות נוספים לפי הצורך
        //        };
        //        dal.Task.Create(newTask);
        //        // עדכון המהנדס במקור הנתונים
        //        dal.Engineer.Update(existingEngineer);
        //    }

        //}
    }

    public void Delete(int taskId)
    {
         if (CanDeleteTask(taskId))
        {
            dal.Task.Delete(taskId);
        }
        else
        {
            throw new BO.BlDeletionImpossible($"Task with ID={taskId} cannot be deleted");
        }
    }

    //public IEnumerable<Task> GetTasksByEngineerLevel(int engineerLevel)
    //{
        //    return dal.ReadAll().Where(task => task.EngineerLevel == engineerLevel);
    //}

}


