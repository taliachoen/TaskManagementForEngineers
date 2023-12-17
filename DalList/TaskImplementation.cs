
namespace Dal;
using DalApi;
using DO;
//Creating CRUD operations for the task
internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newItem = new ()
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
        Task? taskToDelete = DataSource.Tasks.FirstOrDefault(task => task.Id == id) ?? throw new Exception("Object of type Task with such Id does not exist.");
        //Checking whether this task has a dependency on another task
        if (DataSource.Dependensies.Any(dependency => dependency.DependsOnTask == id))
        {
            throw new Exception("The task cannot be deleted because it has a dependency on another task");
        }

        DataSource.Tasks.Remove(taskToDelete);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(task => task.Id == id);
    }

    public List<Task> ReadAll()
    {
        return DataSource.Tasks.ToList();
    }

    public void Update(Task item)
    {
        Task? existingTask = DataSource.Tasks.FirstOrDefault(task => task.Id == item.Id);

        if (existingTask == null)
        {
            throw new Exception("Object of type Task with such Id does not exist.");
        }
        // הסרת הפריט הקיים
        DataSource.Tasks.Remove(existingTask);

        // הוספת הפריט המעודכן
        DataSource.Tasks.Add(item);
    }


};

