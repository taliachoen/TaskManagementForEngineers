using BO;

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
    public void Update(BO.Task updatedTask, bool DuringScheduled =false);

    /// <summary>
    /// Delete a task.
    /// </summary>
    /// <param name="taskId">Task's identifier for deletion</param>
    public void Delete(int taskId);

    /// <summary>
    /// Retrieves engineers based on their experience level.
    /// </summary>
    /// <param name="level">The experience level to filter by.</param>
    /// <returns>A collection of task with the specified Copmlexity.</returns>
    public IEnumerable<BO.Task> GeTaskByCopmlexity(int copmlexity);

    /// <summary>
    /// Filter by status
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public IEnumerable<BO.Task> GeTaskByStatus(int status);

    public IEnumerable<BO.Task> FilterTasksById(int taskId);

    public bool IsCurrentTask(int engineerId);

    public IEnumerable<TaskInList> ReadAllTaskInList(Func<BO.Task, bool>? filter = null);


    public IEnumerable<TaskInList> AllTaskForEngineer(BO.EngineerExperience? engineerExperience);


    public IEnumerable<TaskInList> AllTaskDependency(int idTask);


    public List<TaskScheduleDays> GetAllScheduleTasks();


}
