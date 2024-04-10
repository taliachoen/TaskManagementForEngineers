using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation
{

    internal class TaskImplementation : ITask
    {
        private readonly DalApi.IDal dal = DalApi.Factory.Get;

        private readonly IBl _bl;
        internal TaskImplementation(IBl bl) => _bl = bl;

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
                                    : doTask.CompleteDate is null ? 1
                                    : 2);
            }
            catch (Exception ex)
            {
                throw new BO.BlReadImpossibleException("Error occurred while fetching task status", ex);
            }
        }

        /// <summary>
        /// Calculates the forecasted completion date for a task based on its start date or scheduled date and the required effort time.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The forecasted completion date for the task.</returns>
        /// <remarks>
        /// If the task has a start date, the forecast date will be calculated by adding the required effort time to the start date.
        /// If the task has no start date, the scheduled date will be used for the calculation.
        /// Throws a BlDoesNotExistException if the task with the specified ID does not exist or if there is an error during the calculation.
        /// </remarks>
        private DateTime? ForecastDate(int taskId)
        {
            try
            {
                DO.Task? task = dal.Task.Read(taskId);

                if (task == null)
                {
                    throw new BO.BlDoesNotExistException($"Task with ID={taskId} does not exist");
                }

                // Calculate forecast date based on the start date or scheduled date and required effort time
                DateTime? baseDate = (task.StartDate ?? task.ScheduledDate ?? null);
                if (baseDate == null)
                    return null;
                // Add required effort time to the base date
                DateTime forecastDate = baseDate.Value + (task.RequiredEffortTime ?? TimeSpan.Zero);
                return forecastDate;
            }
            catch (Exception ex)
            {
                throw new BO.BlDoesNotExistException($"Error occurred while calculating forecast date for task with ID={taskId}", ex);
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
                return dal.Dependency.ReadAll(e => e.DependentTask == taskId)
                    .Select(dep =>
                    {
                        var task = dal.Task.Read((int)dep!.DependsOnTask!) ?? throw new BO.BlDoesNotExistException("מזהה משימה לא נמצא");
                        return new BO.TaskInList
                        {
                            Id = task!.Id,
                            Alias = task.Alias,
                            Description = task.Description,
                            Status = TipeOfStatus(task.Id)
                        };
                    })
                    .ToList();
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
                       ForecastDate = ForecastDate(doTask.Id),
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
                // Get project start date
                DateTime? projectStartDate = Factory.Get().ReturnStartProject();

                // Check if project start date exists and if the task's scheduled date is after the project start date
                if (projectStartDate != null && newTask.ScheduledDate < projectStartDate)
                {
                    throw new BO.BlInvalidDataException("Scheduled date for the task is before the project's start date.");
                }
                DO.Task? task = new();
                try
                {
                    task = dal.Task.Read(newTask.Id);
                }
                catch
                {
                    throw new BO.BlDoesNotExistException($"Task with ID={newTask.Id} does not exist");
                }
                // If the project start date exists and the task's scheduled date is after or equal to the project start date,
                // proceed with creating the task
                int idTask = dal.Task.Create(new DO.Task
                {
                    Id = newTask.Id,
                    Alias = newTask.Alias,
                    Description = newTask.Description,
                    CreatedAtDate = task!.CreatedAtDate,
                    RequiredEffortTime = newTask.RequiredEffortTime,
                    Copmlexity = (DO.EngineerExperience?)newTask.Copmlexity,
                    StartDate = null,
                    ScheduledDate = null,
                    CompleteDate = null,
                    Deliverables = newTask.Deliverables,
                    Remarks = newTask.Remarks,
                    EngineerId = newTask?.Engineer?.Id
                });

                // If the task is created after scheduling, update the scheduled date
                if (projectStartDate != null)
                {
                    UpdateOrAddStartDate(idTask, newTask!.ScheduledDate);
                }

                // Add dependencies
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
        public void Update(BO.Task updatedTask, bool DuringScheduled)
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
                    //אופציה שקורת במהלך יצירת הלו"ז
                    if (DuringScheduled)
                    {
                        dal.Task.Update(new DO.Task
                        {
                            Id = task!.Id,
                            Alias = task.Alias,
                            Description = task.Description,
                            CreatedAtDate = task.CreatedAtDate,
                            RequiredEffortTime = task.RequiredEffortTime,
                            Copmlexity = (DO.EngineerExperience?)task.Copmlexity,
                            StartDate = task.StartDate,
                            ScheduledDate = updatedTask.ScheduledDate,
                            CompleteDate = task.CompleteDate,
                            Deliverables = task.Deliverables,
                            Remarks = task.Remarks,
                            EngineerId = updatedTask?.Engineer?.Id
                        });

                    }
                    // Before creating the schedule
                    else if (Factory.Get().ReturnStartProject() == null)
                    {
                        dal.Task.Update(new DO.Task
                        {
                            Id = updatedTask.Id,
                            Alias = updatedTask.Alias,
                            Description = updatedTask.Description,
                            CreatedAtDate = updatedTask.CreatedAtDate,
                            RequiredEffortTime = updatedTask.RequiredEffortTime,
                            Copmlexity = (DO.EngineerExperience?)updatedTask.Copmlexity,
                            StartDate = task.StartDate,
                            ScheduledDate = task.ScheduledDate,
                            CompleteDate = updatedTask.CompleteDate,
                            Deliverables = updatedTask.Deliverables,
                            Remarks = updatedTask.Remarks,
                            EngineerId = null
                        });
                    }
                    // After creating the schedule
                    else if (!DuringScheduled)
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
                            CompleteDate = updatedTask.CompleteDate,
                            Deliverables = updatedTask.Deliverables,
                            Remarks = updatedTask.Remarks,
                            EngineerId = updatedTask?.Engineer?.Id
                        });
                        UpdateOrAddStartDate(task.Id, updatedTask?.StartDate);
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
                    var dependenciesToDelete = dal.Dependency.ReadAll(dep => dep!.DependentTask == taskId).ToList();
                    dependenciesToDelete.ForEach(dependency =>
                    {
                        dal.Dependency.Delete(dependency!.Id);
                    });
                }
                else
                {
                    throw new BO.BlDeletionImpossibleException($"Task with ID={taskId} cannot be deleted");
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
        public void UpdateOrAddStartDate(int taskId, DateTime? plannedDate)
        {
            try
            {
                // Get all tasks preceding the current task
                var dependencies = dal.Dependency.ReadAll(dep => dep.DependsOnTask == taskId);
                DateTime? maxPreviousEndDate = null;
                // Check if the planned  date is not earlier than the latest estimated end date of preceding tasks
                // Check if any dependent task has no start date defined
                var dependentTasksWithNoStartDate = dependencies
                    .Where(dep => dal.Task.Read((int)dep!.DependentTask!)?.StartDate == null)
                    .ToList();
                if (dependentTasksWithNoStartDate.Any())
                {
                    var dependentTaskIds = string.Join(", ", dependentTasksWithNoStartDate.Select(dep => dep?.DependentTask));
                    throw new BO.BlInvalidDataException($"One or more dependent tasks of task {taskId} have no start date defined: {dependentTaskIds}");
                }
                maxPreviousEndDate = dependencies.Max(dep => dal.Task.Read((int)dep!.DependentTask!)?.StartDate);


                if (maxPreviousEndDate != null && plannedDate < maxPreviousEndDate)
                {
                    throw new BO.BlInvalidDataException($"Planned start date for task {taskId} is earlier than the latest estimated end date of its preceding tasks.");
                }

                // Check if the task has no dependencies and there is a project start date
                if (!dependencies.Any() && Factory.Get().ReturnStartProject() != null)
                {
                    var projectStartDate = Factory.Get().ReturnStartProject();
                    if (plannedDate < projectStartDate)
                    {
                        throw new BO.BlInvalidDataException($"Planned start date for task {taskId} is before the project's start date.");
                    }
                }

                // Perform the update request to the data layer
                var taskToUpdate = dal.Task.Read(taskId);
                if (taskToUpdate != null)
                {
                    var task = taskToUpdate! with { StartDate = plannedDate };
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

        /// <summary>
        /// Retrieves engineers based on their experience level.
        /// </summary>
        /// <param name="level">The experience level to filter by.</param>
        /// <returns>A collection of task with the specified Copmlexity.</returns>
        public IEnumerable<BO.Task> GeTaskByCopmlexity(int copmlexity)
        {
            return dal.Task.ReadAll()
                .Where(e => (int?)e?.Copmlexity == copmlexity)
                .Select(doTask => new BO.Task
                {
                    Id = doTask!.Id!,
                    Description = doTask.Description,
                    Alias = doTask.Alias,
                    CreatedAtDate = doTask.CreatedAtDate,
                    Status = TipeOfStatus(doTask.Id),
                    Dependencies = Dependencies(doTask.Id),
                    RequiredEffortTime = doTask.RequiredEffortTime,
                    StartDate = doTask.StartDate,
                    ScheduledDate = doTask.ScheduledDate,
                    ForecastDate = ForecastDate(doTask.Id),
                    CompleteDate = doTask.CompleteDate,
                    Deliverables = doTask.Deliverables,
                    Remarks = doTask.Remarks,
                    Engineer = EngineerInTask(doTask.Id),
                    Copmlexity = (BO.EngineerExperience?)doTask.Copmlexity
                });
        }

        /// <summary>
        /// Filter by status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IEnumerable<BO.Task> GeTaskByStatus(int status)
        {
            return dal.Task.ReadAll()
                .Where(doTask => TipeOfStatus(doTask!.Id) == (BO.Status)status)
                .Select(doTask => new BO.Task
                {
                    Id = doTask!.Id!,
                    Description = doTask.Description,
                    Alias = doTask.Alias,
                    CreatedAtDate = doTask.CreatedAtDate,
                    Status = TipeOfStatus(doTask.Id),
                    Dependencies = Dependencies(doTask.Id),
                    RequiredEffortTime = doTask.RequiredEffortTime,
                    StartDate = doTask.StartDate,
                    ScheduledDate = doTask.ScheduledDate,
                    ForecastDate = ForecastDate(doTask.Id),
                    CompleteDate = doTask.CompleteDate,
                    Deliverables = doTask.Deliverables,
                    Remarks = doTask.Remarks,
                    Engineer = EngineerInTask(doTask.Id),
                    Copmlexity = (BO.EngineerExperience?)doTask.Copmlexity
                });
        }

        public IEnumerable<BO.Task> FilterTasksById(int taskId)
        {
            return dal.Task.ReadAll()
                .Where(doTask => doTask!.EngineerId == taskId)
                .Select(doTask => new BO.Task
                {
                    Id = doTask!.Id!,
                    Description = doTask.Description,
                    Alias = doTask.Alias,
                    CreatedAtDate = doTask.CreatedAtDate,
                    Status = TipeOfStatus(doTask.Id),
                    Dependencies = Dependencies(doTask.Id),
                    RequiredEffortTime = doTask.RequiredEffortTime,
                    StartDate = doTask.StartDate,
                    ScheduledDate = doTask.ScheduledDate,
                    ForecastDate = ForecastDate(doTask.Id),
                    CompleteDate = doTask.CompleteDate,
                    Deliverables = doTask.Deliverables,
                    Remarks = doTask.Remarks,
                    Engineer = EngineerInTask(doTask.Id),
                    Copmlexity = (BO.EngineerExperience?)doTask.Copmlexity
                });
        }

        /// <summary>
        /// בדיקה אם יש למהנדס משימה שהוא עובד עליה כרגע 
        /// </summary>
        /// <param name="engineerId"></param>
        /// <returns></returns>
        /// <exception cref="BO.BlReadImpossibleException"></exception>
        public bool IsCurrentTask(int engineerId)
        {
            try
            {

                return dal.Task.ReadAll(task => task.EngineerId == engineerId && task.StartDate != null && task.CompleteDate == null).Any();
            }
            catch (Exception ex)
            {
                throw new BO.BlReadImpossibleException("Error occurred while checking if engineer has current task", ex);
            }
        }

        public IEnumerable<TaskInList> AllTaskForEngineer(BO.EngineerExperience? engineerExperience)
        {
            // סנן משימות שהן במצב "לא מתוזמנות" ואין להן מהנדס משויך
            var unscheduledTasks = ReadAll(task =>
                //task.Engineer?.Id == 0 &&
                task.Status == Status.Unscheduled &&
                task.Copmlexity <= engineerExperience);// &&
            //(task.Dependencies != null ? task.Dependencies.All(subTask => subTask.Status == BO.Status.Done) : true));

            //החזר רשימה של TaskInList מהמשימות שעברו את הסינון
            return unscheduledTasks.Select(task => new TaskInList
            {
                Id = task.Id,
                Description = task.Description,
                Alias = task.Alias,
                Status = (Status)task.Status!
            });

        }

        public IEnumerable<TaskInList> ReadAllTaskInList(Func<BO.Task, bool>? filter = null)
        {
            var allTasks = ReadAll(filter).Select(task => new TaskInList
            {
                Id = task.Id,
                Description = task.Description,
                Alias = task.Alias,
                Status = (Status)task.Status!
            });

            return allTasks;

        }

        public IEnumerable<TaskInList> AllTaskDependency(int idTask)
        {
            BO.Task currentTask = Read(idTask); // קריאה לפונקציה Read בקלאס TaskImplementation
            var v = Dependencies(idTask);
            var allTasks = ReadAll().Where(task => task.Id != idTask && task.CompleteDate > currentTask.StartDate).Select(task => new TaskInList
            {
                Id = task.Id,
                Description = task.Description,
                Alias = task.Alias,
                Status = (Status)task.Status!
            }).Intersect(v);
            return allTasks;
        }



        public List<BO.TaskScheduleDays> GetAllScheduleTasks()
        {
            try
            {
                var startProject = dal.ReturnStartProject();
                if (startProject == null)
                {
                    throw new ValidationException("תאריך התחלה לא קיים עדיין");
                }
                List<TaskScheduleDays?> result = new();

                foreach (var task in ReadAll())
                {
                    try
                    {
                        int daysFromProjectStart = (int)((task!.ScheduledDate!.Value - dal.ReturnStartProject()!.Value).TotalDays);
                        int daysFromProjectEnd = (int)((dal.ReturnEndProject()!.Value.AddDays(50) - (task.StartDate ?? task.ScheduledDate + task.RequiredEffortTime)!.Value).TotalDays);
                        var taskDays = task.RequiredEffortTime!.Value.Days;
                        string d = "";
                        foreach (var dep in dal.Dependency.ReadAll())
                        {
                            if (dep?.DependentTask == task.Id)
                            {
                                d += dep.DependsOnTask + " , ";
                            }

                        }
                        BO.TaskScheduleDays taskSchedule = new()
                        {
                            TaskId = task.Id,
                            DependencyId = d,
                            TaskName = task.Alias,
                            DaysFromProjectStart = daysFromProjectStart,
                            TaskDays = taskDays,
                            DaysToProjectEnd = daysFromProjectEnd
                        };
                        result.Add(taskSchedule);
                    }
                    catch (Exception ex)
                    {
                        // טיפול בחריגה בתוך הלולאה הפנימיה
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return new List<BO.TaskScheduleDays>();
            }
        }



        //var allTasks = ReadAll().Where(task => task.Id != idTask && task.CompleteDate > currentTask.StartDate && task.Dependencies?.Where(x=>x.Id != currentTask.Dependencies)).Select(task => new TaskInList
    }
}
