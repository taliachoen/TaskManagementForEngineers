
using BlApi;


namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private readonly DalApi.IDal task_dal = DalApi.Factory.Get;

    public int Create(BO.Task newTask)
    {
        DO.Task task = new(newTask.Id, newTask.Alias, newTask.Description);
        try
        {
            int idTask = task_dal.Task.Create(task);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={newTask.Id} already exists", ex);
        }

    }

    public BO.Task Read(int taskId) 
    { 
    
        DO.Task? doTask = task_dal.Task.Read(taskId);
        return doTask == null
            ? throw new BO.BlDoesNotExistException($"Task with ID={taskId} does Not exist")
            : new BO.Task
            {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            };
    }

    public void Delete(int taskId)
    {
        task_dal.Task.Delete(taskId);
    }

    public IEnumerable<Task> ReadAll()
    {
        return (IEnumerable<Task>)task_dal.Task.ReadAll();
    }
   
    public void Update(DO.Task updatedTask)
    {
        task_dal.Task.Update(updatedTask);
    }
   
    //public IEnumerable<Task> GetTasksByEngineerLevel(int engineerLevel)
    //{
        //    return task_dal.ReadAll().Where(task => task.EngineerLevel == engineerLevel);
    //}

}
