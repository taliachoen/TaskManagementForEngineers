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

    //לבדוק את מצב הסטטוס האמיתי
    private BO.Status TipeOfStatus(int taskId)
    {
        DO.Task? doTask = dal.Task.Read(taskId);
        return doTask == null
            ? throw new BO.BlDoesNotExistException("מזהה משימה לא נמצא")
            : (BO.Status)(doTask!.StartDate is null ? 0
                            : doTask!.ScheduledDate is null ? 1
                            : doTask.CompleteDate is null ? 2
                            : 3);
    }

    private List<BO.TaskInList> Dependencies(int taskId)
    {
        var v = dal.Dependency.ReadAll(e => e.DependsOnTask == taskId);
        List<BO.TaskInList>? taskInLists = null;
        foreach (var dep in v)
        {
            var task = dal.Task.Read(e => e.Id == dep?.DependsOnTask) ?? throw new BO.BlDoesNotExistException("מזהה משימה לא נמצא");
            BO.TaskInList taskInList = new()
            {
                Id = task!.Id,
                Alias = task.Alias,
                Description = task.Description,
                Status = TipeStatus(task.Id)
            };
            taskInLists?.Add(taskInList);
        }
        return taskInLists ?? new List<BO.TaskInList>();
    }

    private BO.EngineerInTask EngineerInTask(int taskId)
    {
        DO.Task? task = dal.Task.Read(x => x.Id == taskId);
        DO.Engineer? engineer;
        BO.EngineerInTask? engineerInTask = null;
        if (task?.CompleteDate == null)
        {
            try {
                engineer = dal.Engineer.Read(x => x.Id == task?.EngineerId);
                if (engineer != null)
                {
                    engineerInTask = new() { Id = engineer.Id, Name = engineer.Name };
                }
            }
            catch(Exception) {
                throw new BO.BlDoesNotExistException("מזהה מהנדס לא נמצא")
            }
        }
        return engineerInTask;
    }
    private static bool CanDeleteTask(int taskId)
    {
        return taskId == 0;
        //בדיקה האם המשימה לא קודמת למשימות אחרות או האם אנחנו לאחר יצירת לוז
    }

    //private  static BO.Status Status() {}
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)
        {
            return dal.Task.ReadAll().Select(task => Read(task!.Id));
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
                CreatedAtDate = doTask.CreatedAtDate,
                Status = TipeOfStatus(doTask.Id),
                Dependencies = Dependencies(doTask.Id),
                RequiredEffortTime = doTask.RequiredEffortTime,
                StartDate = doTask.StartDate,
                ScheduledDate = doTask.ScheduledDate,
                //ForecastDate = doTask.ForecastDate,
                ForecastDate = null,
                CompleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Engineer = EngineerInTask(doTask.Id),
                Copmlexity = (BO.EngineerExperience?)doTask.Copmlexity
            };
    }

    public int Create(BO.Task newTask)
    {

        if (!ValidateTask(newTask))
        {
            throw new BO.BlInvalidDataException("Invalid engineer data.");
        }
        try
        {
            int idTask = dal.Task.Create(new DO.Task
            {
                Id = newTask.Id,
                Alias = newTask.Alias,
                Description = newTask.Description,
                CreatedAtDate = newTask.CreatedAtDate,
                RequiredEffortTime = newTask.RequiredEffortTime,
                Copmlexity = (DO.EngineerExperience?)newTask.Copmlexity,
                StartDate = newTask.StartDate,
                ScheduledDate = newTask.ScheduledDate,
                CompleteDate = newTask.CompleteDate,
                Deliverables = newTask.Deliverables,
                Remarks = newTask.Remarks,
                EngineerId = newTask?.Engineer?.Id
            });

            //להכניס לפונקציית עזר
            //להוסיף פילטר לפי - רשימת המשימות הקיימת
            var tasks = dal.Task.ReadAll();
            foreach(var depTask in tasks)
            {
                DO.Dependency doDep = new(newTask.Id, depTask.Id);
            }
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


