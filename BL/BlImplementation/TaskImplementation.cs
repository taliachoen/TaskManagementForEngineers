using BlApi;

namespace BlImplementation
{
    internal class TaskImplementation : ITask
    {
        private readonly DalApi.IDal dal = DalApi.Factory.Get;

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

        private List<BO.TaskInList> Dependencies(int taskId)
        {
            try
            {
                var v = dal.Dependency.ReadAll(e => e.DependentTask == taskId);
                List<BO.TaskInList>? taskInLists = new List<BO.TaskInList>(); ;
                foreach (var dep in v)
                {
                    var task = dal.Task.Read((int)dep.DependsOnTask) ?? throw new BO.BlDoesNotExistException("מזהה משימה לא נמצא");
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

        private BO.EngineerInTask EngineerInTask(int taskId)
        {
            try
            {
                DO.Task? task = dal.Task.Read(taskId);
                DO.Engineer? engineer = null;
                BO.EngineerInTask? engineerInTask = null;
                if (task.EngineerId != null && task?.CompleteDate == null)
                {
                    engineer = dal.Engineer.Read((int)task.EngineerId);
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

        //linqToObject
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
                    //לפני יצירת הלו"ז
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
                            StartDate = updatedTask.StartDate,
                            ScheduledDate = updatedTask.ScheduledDate,
                            CompleteDate = updatedTask.CompleteDate,
                            Deliverables = updatedTask.Deliverables,
                            Remarks = updatedTask.Remarks,
                            EngineerId = updatedTask?.Engineer?.Id
                        });
                    }
                    //לאחר יצירת הלו"ז
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

        public void Delete(int taskId)
        {
            try
            {
                if (CanDeleteTask(taskId))
                {
                    dal.Task.Delete(taskId);
                    var dependencies = dal.Dependency.ReadAll().Where(dep => dep.DependentTask != taskId).ToList();
                    foreach (var dependency in dependencies)
                    {
                        dal.Dependency.Delete(dependency.Id);
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

    }
}