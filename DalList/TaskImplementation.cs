
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
        Task? newItem = DataSource.Tasks.Find(x => x.Id == id);
        if (newItem == null)
        {
            throw new Exception("Object of type Task with such Id does not exist.");
        }
        else
        { 
            //Checking whether this task has a dependency on another task
            if (DataSource.Dependensies.Any(x => x.DependsOnTask == id))
                throw new Exception("The task cannot be deleted because it has a dependency on another task");
            DataSource.Tasks.Remove(newItem);
        }
       
    }

    public Task? Read(int id)
    {
        Task? newItem = DataSource.Tasks.Find(x => x.Id == id);
        if (newItem != null)
            return newItem;
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        Task? newItem = DataSource.Tasks.Find(x => x.Id == item.Id);
        if (newItem == null)
            throw new Exception("Objedt of type Task with such Id does not exist.");
        else
        {
            DataSource.Tasks.Remove(newItem);
            DataSource.Tasks.Add(item);
        }

    }
}
