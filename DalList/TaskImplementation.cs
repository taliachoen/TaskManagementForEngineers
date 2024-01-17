//namespace Dal;
//using DalApi;
//using DO;
////Creating CRUD operations for the task
//internal class TaskImplementation : ITask
//{
//    /// <summary>
//    ///Creates new entity object in DAL 
//    /// </summary>
//    /// <param name="item"></param>
//    /// <returns></returns>
//    public int Create(Task item)
//    {
//        int newId = DataSource.Config.NextTaskId;
//        Task newItem = new()
//        {
//            Id = newId,
//            Alias = item.Alias,
//            Description = item.Description,
//            CreatedAtDate = item.CreatedAtDate,
//            RequiredEffortTime = item.RequiredEffortTime,
//            IsMilestone = item.IsMilestone,
//            Copmlexity = item.Copmlexity,
//            StartDate = item.StartDate,
//            ScheduledDate = item.ScheduledDate,
//            DeadlineDate = item.DeadlineDate,
//            CompleteDate = item.CompleteDate,
//            Deliverables = item.Deliverables,
//            Remarks = item.Remarks,
//            EngineerId = item.EngineerId
//        };
//        DataSource.Tasks.Add(newItem);
//        return newId;
//    }

//    /// <summary>
//    ///Deletes an object by its Id 
//    /// </summary>
//    /// <param name="id"></param>
//    /// <exception cref="DalDoesNotExistException"></exception>
//    public void Delete(int id)
//    {
//        Task? taskToDelete = DataSource.Tasks.FirstOrDefault(task => task.Id == id) ?? throw new DalDoesNotExistException("Object of type Task with such Id does not exist.");

//        //Checking whether this task has a dependency on another task
//        if (DataSource.Dependensies.Any(dependency => dependency.DependsOnTask == id))
//        {
//            throw new DalDeletionImpossible("The task cannot be deleted because it has a dependency on another task");
//        }

//        DataSource.Tasks.Remove(taskToDelete);
//    }

//    /// <summary>
//    ///Reads entity object by its ID  
//    /// </summary>
//    /// <param name="id"></param>
//    /// <returns></returns>
//    /// <exception cref="DalDoesNotExistException"></exception>
//    public Task? Read(int id)
//    {
//        return DataSource.Tasks.FirstOrDefault(task => task.Id == id);
//    }

//    /// <summary>
//    ///Updates entity object  
//    /// </summary>
//    /// <param name="item"></param>
//    /// <exception cref="DalDoesNotExistException"></exception>
//    public void Update(Task item)
//    {
//        Task? existingTask = DataSource.Tasks.FirstOrDefault(task => task.Id == item.Id) ?? throw new DalDoesNotExistException("Object of type Task with such Id does not exist.");

//        // Your existing code for updating a task
//        DataSource.Tasks.Remove(existingTask);
//        DataSource.Tasks.Add(item);

//    }

//    public void Update(Task item)
//    {
//        if (filter != null)
//        {
//            throw new Exception("Object of type Task with such Id does not exist.");
//        }
//        // הסרת הפריט הקיים
//        DataSource.Tasks.Remove(existingTask);

//        // הוספת הפריט המעודכן
//        DataSource.Tasks.Add(item);
//    }


//};

namespace Dal;
using DalApi;
using DO;
//Creating CRUD operations for the task
internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newItem = new()
        {
            Id = newId,
            Alias = item.Alias,
            Description = item.Description,
            CreatedAtDate = item.CreatedAtDate,
            RequiredEffortTime = item.RequiredEffortTime,
            IsMilestone = item.IsMilestone,
            Copmlexity = item.Copmlexity,
            StartDate = item.StartDate,
            ScheduledDate = item.ScheduledDate,
            DeadlineDate = item.DeadlineDate,
            CompleteDate = item.CompleteDate,
            Deliverables = item.Deliverables,
            Remarks = item.Remarks,
            EngineerId = item.EngineerId
        };
        DataSource.Tasks.Add(newItem);
        return newId;
    }

    public void Delete(int id)
    {
        Task? taskToDelete = DataSource.Tasks.FirstOrDefault(task => task.Id == id) ?? throw new DalDoesNotExistException("Object of type Task with such Id does not exist.");

        //Checking whether this task has a dependency on another task
        if (DataSource.Dependensies.Any(dependency => dependency.DependsOnTask == id))
        {
            throw new DalDeletionImpossible("The task cannot be deleted because it has a dependency on another task");
        }

        DataSource.Tasks.Remove(taskToDelete);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(task => task.Id == id);
    }

    public void Update(Task item)
    {
        Task? existingTask = DataSource.Tasks.FirstOrDefault(task => task.Id == item.Id) ?? throw new DalDoesNotExistException("Object of type Task with such Id does not exist.");

        // Your existing code for updating a task
        DataSource.Tasks.Remove(existingTask);
        DataSource.Tasks.Add(item);

    }

    // Reads all entity objects
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    // A read operation that receives a function
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }
};