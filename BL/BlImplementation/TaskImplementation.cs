

using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal task_dal = DalApi.Factory.Get;
    public void AddTask(Task newTask)
    {
        throw new NotImplementedException();
    }

    public void DeleteTask(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task GetTaskDetails(int taskId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> GetTasksByEngineerLevel(int engineerLevel)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> GetTasksList()
    {
        throw new NotImplementedException();
    }

    public void UpdateTask(Task updatedTask)
    {
        throw new NotImplementedException();
    }
}
