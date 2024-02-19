namespace BlApi;

public interface ITask
{
    /// <summary>
    /// Request a list of tasks.
    /// </summary>
    /// <returns>List of tasks</returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);

    /// <summary>
    /// Request details of a task based on the identifier.
    /// </summary>
    /// <param name="taskId">Task's identifier</param>
    /// <returns>Task object based on the identifier</returns>
    public BO.Task Read(int taskId);

    /// <summary>
    /// Add a new task.
    /// </summary>
    /// <param name="newTask">Task object to add</param>
    public int Create(BO.Task newTask);

    /// <summary>
    /// Update task information.
    /// </summary>
    /// <param name="updatedTask">Task object with updated data</param>
    public void Update(BO.Task updatedTask);

    /// <summary>
    /// Delete a task.
    /// </summary>
    /// <param name="taskId">Task's identifier for deletion</param>
    public void Delete(int taskId);
}
