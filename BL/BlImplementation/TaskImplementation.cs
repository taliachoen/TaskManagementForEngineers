using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlImplementation
{

    internal class TaskImplementation : ITask
    {
        private readonly DalApi.IDal dal = DalApi.Factory.Get;

        /// <summary>
        /// Validates the data of a task.
        /// </summary>
        /// <param name="newTask">The task to validate.</param>
        /// <returns>True if the task data is valid; otherwise, false.</returns>
        private static bool ValidateTask(BO.Task newTask)
        {
            if (string.IsNullOrEmpty(newTask.Alias))
                throw new BO.BlInvalidDataException("Alias cannot be null or empty.");

            if (string.IsNullOrEmpty(newTask.Description))
                throw new BO.BlInvalidDataException("Description cannot be null or empty.");

            if (string.IsNullOrEmpty(newTask.Deliverables))
                throw new BO.BlInvalidDataException("Deliverables must be provided and cannot be empty.");

            if (newTask.Engineer == null)
                throw new BO.BlInvalidDataException("An engineer must be assigned to the task.");

            if (newTask.Copmlexity == null)
                throw new BO.BlInvalidDataException("Complexity must be provided.");

            return true;
        }

        /// <summary>
        /// Retrieves the status of a task based on its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The status of the task.</returns>
        private BO.Status TipeOfStatus(int taskId)
        {
            try
            {
                DO.Task? doTask = dal.Task.Read(taskId);
                return doTask == null
                    ? throw new BO.BlDoesNotExistException("Task ID not found")
                    : (BO.Status)(doTask!.StartDate is null ? 0
                                    : doTask!.ScheduledDate is null ? 1
                                    : doTask.CompleteDate is null ? 2
                                    : 3);
            }
            catch (Exception ex)
            {
                throw new BO.BlReadImpossibleException("Error occurred while fetching task status", ex);
            }
        }

        /// <summary>
        /// Retrieves the list of dependencies for a task based on its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The list of dependencies as TaskInList objects.</returns>
        private List<BO.TaskInList> Dependencies(int taskId)
        {
            try
            {
                var v = dal.Dependency.ReadAll(e => e.DependentTask == taskId);
                List<BO.TaskInList>? taskInLists = new(); ;
                foreach (var dep in v)
                {
                    var task = dal.Task.Read((int)dep!.DependsOnTask!) ?? throw new BO.BlDoesNotExistException("מזהה משימה לא נמצא");
                    BO.TaskInList taskInList = new()
                    {
                        Id = task!.Id,
                        Alias = task.Alias,
                        Description = task.Description,
                        Status = TipeOfStatus(task.Id)
                    };
                    taskInLists?.Add(taskInList);
                }
                return taskInLists ?? new List<BO.TaskInList>();

            }
            catch (Exception ex)
            {
                throw new BO.BlReadImpossibleException("Error occurred while fetching dependencies", ex);
            }
        }

        /// <summary>
        /// Retrieves information about the engineer assigned to a task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The information about the engineer in the task as EngineerInTask object.</returns>
        private BO.EngineerInTask EngineerInTask(int taskId)
        {
            try
            {
                DO.Task? task = dal.Task.Read(taskId);
                DO.Engineer? engineer = null;
                BO.EngineerInTask? engineerInTask = null;
                if (task!.EngineerId != null && task?.CompleteDate == null)
                {
                    engineer = dal.Engineer.Read((int)task!.EngineerId);
                    if (engineer != null)
                    {
                        engineerInTask = new() { Id = engineer.Id, Name = engineer.Name };
                    }
                }
                return engineerInTask ?? new BO.EngineerInTask();
            }
            catch (Exception ex)
            {
                throw new BO.BlCreateImpossibleException("Error occurred while fetching engineer task", ex);
            }
        }

        /// <summary>
        /// Checks whether a task can be deleted based on its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>True if the task can be deleted; otherwise, false.</returns>
        private bool CanDeleteTask(int taskId)
        {
            try
            {
                IEnumerable<DO.Dependency?> v = dal.Dependency.ReadAll(e => e.DependsOnTask == taskId);
                return !v.Any();
            }
            catch (Exception ex)
            {
                throw new BO.BlReadImpossibleException($"Error occurred while checking if task can be deleted: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all tasks based on an optional filter.
        /// </summary>
        /// <param name="filter">Optional filter for tasks.</param>
        /// <returns>The collection of tasks that match the filter.</returns>
        public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
        {
            try
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
            catch (Exception ex)
            {
                throw new BO.BlReadImpossibleException("Error occurred while fetching all tasks", ex);
            }
        }

        /// <summary>
        /// Retrieves information about a task based on its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The information about the task as a Task object.</returns>
        public BO.Task Read(int taskId)
        {
            try
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
                       ForecastDate = null,
                       CompleteDate = doTask.CompleteDate,
                       Deliverables = doTask.Deliverables,
                       Remarks = doTask.Remarks,
                       Engineer = EngineerInTask(doTask.Id),
                       Copmlexity = (BO.EngineerExperience?)doTask.Copmlexity
                   };
            }
            catch (Exception ex)
            {
                throw new BO.BlReadImpossibleException($"Task with ID={taskId} does Succeeded to Read", ex);
            }
        }

        /// <summary>
        /// Creates a new task based on the provided data.
        /// </summary>
        /// <param name="newTask">The data for the new task.</param>
        /// <returns>The ID of the newly created task.</returns>
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
                    StartDate = null,
                    ScheduledDate = newTask.ScheduledDate,
                    CompleteDate = null,
                    Deliverables = newTask.Deliverables,
                    Remarks = newTask.Remarks,
                    EngineerId = newTask?.Engineer?.Id
                });
                Dependencies(idTask);
                return idTask;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Task with ID={newTask.Id} already exists", ex);
            }
            catch (Exception ex)
            {
                throw new BO.BlCreateImpossibleException("Error occurred while creating task", ex);
            }
        }

        /// <summary>
        /// Updates an existing task based on the provided data.
        /// </summary>
        /// <param name="updatedTask">The updated data for the task.</param>
        public void Update(BO.Task updatedTask)
        {
            if (ValidateTask(updatedTask) && updatedTask != null)
            {
                DO.Task? task = new();
                try
                {
                    task = dal.Task.Read(updatedTask.Id);
                }
                catch
                {
                    throw new BO.BlDoesNotExistException($"Task with ID={updatedTask.Id} does not exist");
                }
                try
                {
                    // Before creating the schedule
                    if (Factory.Get().StartProject == null)
                    {
                        dal.Task.Update(new DO.Task
                        {
                            Id = updatedTask.Id,
                            Alias = updatedTask.Alias,
                            Description = updatedTask.Description,
                            CreatedAtDate = updatedTask.CreatedAtDate,
                            RequiredEffortTime = updatedTask.RequiredEffortTime,
                            Copmlexity = (DO.EngineerExperience?)updatedTask.Copmlexity,
                            StartDate = null,
                            ScheduledDate = updatedTask.ScheduledDate,
                            CompleteDate = updatedTask.CompleteDate,
                            Deliverables = updatedTask.Deliverables,
                            Remarks = updatedTask.Remarks,
                            EngineerId = null
                        });
                    }
                    // After creating the schedule
                    else
                    {
                        dal.Task.Update(new DO.Task
                        {
                            Id = task!.Id,
                            Alias = updatedTask.Alias,
                            Description = updatedTask.Description,
                            CreatedAtDate = task.CreatedAtDate,
                            RequiredEffortTime = task.RequiredEffortTime,
                            Copmlexity = (DO.EngineerExperience?)task.Copmlexity,
                            StartDate = task.StartDate,
                            ScheduledDate = task.ScheduledDate,
                            CompleteDate = task.CompleteDate,
                            Deliverables = updatedTask.Deliverables,
                            Remarks = updatedTask.Remarks,
                            EngineerId = updatedTask?.Engineer?.Id
                        });
                    }

                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BO.BlDoesNotExistException($"Task with ID={updatedTask.Id} does not exist", ex);
                }
                catch (Exception ex)
                {
                    throw new BO.BlUnableToUpdateException($"Error occurred while updating task", ex);
                }
            }

        }

        /// <summary>
        /// Deletes a task based on its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task to delete.</param>
        public void Delete(int taskId)
        {
            try
            {
                if (CanDeleteTask(taskId))
                {
                    dal.Task.Delete(taskId);
                    var dependencies = dal.Dependency.ReadAll().Where(dep => dep!.DependentTask != taskId).ToList();
                    foreach (var dependency in dependencies)
                    {
                        dal.Dependency.Delete(dependency!.Id);
                    }
                }
                else
                {
                    Console.WriteLine($"Task with ID={taskId} cannot be deleted");
                }
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={taskId} does not exist", ex);
            }
            catch (Exception ex)
            {
                throw new BO.BlDeletionImpossibleException($"Error occurred while deleting task with ID={taskId}", ex);
            }
        }

        /// <summary>
        /// Updates or adds the scheduled start date for a task based on its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="plannedStartDate">The planned start date for the task.</param>
        public void UpdateOrAddStartDate(int taskId, DateTime? plannedStartDate)
        {
            try
            {
                // Get all tasks preceding the current task
                var dependencies = dal.Dependency.ReadAll(dep => dep.DependsOnTask == taskId);

                // Check if the planned start date is not earlier than the latest estimated end date of preceding tasks
                var maxPreviousEndDate = dependencies.Max(dep => dal.Task.Read((int)dep!.DependentTask!)?.StartDate);
                if (maxPreviousEndDate != null && plannedStartDate < maxPreviousEndDate)
                {
                    throw new BO.BlInvalidDataException($"Planned start date for task {taskId} is earlier than the latest estimated end date of its preceding tasks.");
                }

                // Check if the task has no dependencies and there is a project start date
                if (!dependencies.Any() && Factory.Get().StartProject != null)
                {
                    var projectStartDate = Factory.Get().StartProject;
                    if (plannedStartDate < projectStartDate)
                    {
                        throw new BO.BlInvalidDataException($"Planned start date for task {taskId} is before the project's start date.");
                    }
                }

                // Perform the update request to the data layer
                var taskToUpdate = dal.Task.Read(taskId);
                if (taskToUpdate != null)
                {
                    var task = taskToUpdate! with { ScheduledDate = plannedStartDate };
                    dal.Task.Update(task);
                }
                else
                {
                    throw new BO.BlDoesNotExistException($"Task with ID={taskId} does not exist.");
                }

            }
            catch (BO.BlInvalidDataException)
            {
                throw; // The exception is already identified, no need to wrap again
            }
            catch (Exception ex)
            {
                throw new BO.BlUnableToUpdateException($"Error occurred while updating or adding scheduled start date for task with ID={taskId}.", ex);
            }
        }

    }
}
