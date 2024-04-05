using BlApi;

namespace BlImplementation
{
    internal class EngineerImplementation : IEngineer
    {
        private readonly DalApi.IDal dal = DalApi.Factory.Get;

        /// <summary>
        /// Validates the data of an engineer.
        /// </summary>
        /// <param name="newEngineer">The engineer to validate.</param>
        /// <returns>True if the engineer data is valid; otherwise, false.</returns>
        private static bool ValidateEngineer(BO.Engineer newEngineer)
        {
            if (newEngineer.Id <= 0)
                throw new BO.BlInvalidDataException("Engineer ID must be a positive integer.");

            if (string.IsNullOrEmpty(newEngineer.Email))
                throw new BO.BlInvalidDataException("Email cannot be null or empty.");

            if (newEngineer.Cost < 0)
                throw new BO.BlInvalidDataException("Cost must be a non-negative value.");

            if (string.IsNullOrEmpty(newEngineer.Name))
                throw new BO.BlInvalidDataException("Name cannot be null or empty.");

            return true;
        }

        /// <summary>
        /// Reads all engineers based on an optional filter.
        /// </summary>
        /// <param name="filter">Optional filter for engineers.</param>
        /// <returns>A collection of engineers.</returns>
        public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
        {
            if (filter == null)
            {
                return dal.Engineer.ReadAll().Select(doEngineer => Read(doEngineer!.Id));
            }
            else
            {
                return ReadAll().Where(filter);
            }
        }

        /// <summary>
        /// Retrieves engineers based on their experience level.
        /// </summary>
        /// <param name="level">The experience level to filter by.</param>
        /// <returns>A collection of engineers with the specified experience level.</returns>
        public IEnumerable<BO.Engineer> GetEngineersByLevel(int level)
        {
            return dal.Engineer.ReadAll()
                .Where(e => (int?)e?.Level == level)
                .Select(doEngineer => new BO.Engineer
                {
                    Id = doEngineer!.Id,
                    Email = doEngineer.Email,
                    Cost = (double?)doEngineer.Cost,
                    Name = doEngineer.Name,
                    Level = (BO.EngineerExperience?)doEngineer.Level
                });
        }

        /// <summary>
        /// Retrieves an engineer based on their ID.
        /// </summary>
        /// <param name="engineerId">The ID of the engineer to retrieve.</param>
        /// <returns>The engineer with the specified ID.</returns>
        public BO.Engineer Read(int engineerId)
        {
           // try
           // {
                DO.Engineer? doEngineer = dal.Engineer.Read(engineerId);
                if (doEngineer==null)
                {
                    throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist");
                }
                DO.Task? engTask = null;

                try { engTask = dal.Task.Read(x => x.EngineerId == engineerId && x.CompleteDate == null); }
                catch { engTask = null; }
                try
                {
                    
                   return new BO.Engineer
                   {
                       Id = doEngineer.Id,
                       Email = doEngineer.Email,
                       Cost = (double?)doEngineer.Cost,
                       Name = doEngineer.Name,
                       Level = (BO.EngineerExperience?)doEngineer.Level,
                       Task = engTask != null ? new BO.TaskInEngineer { Id = engTask.Id, Alias = engTask.Alias } : null
                   };
                }
                catch {
                throw new BO.BlReadImpossibleException($"Error while reading engineer with ID={engineerId}");
                }

            //}
            //catch (BO.BlDoesNotExistException ex)
            //{
            //    throw new BO.BlReadImpossibleException($"Error while reading engineer with ID={engineerId}", ex);
            //}
        }

        /// <summary>
        /// Creates a new engineer.
        /// </summary>
        /// <param name="newEngineer">The new engineer to create.</param>
        /// <returns>The ID of the newly created engineer.</returns>
        public int Create(BO.Engineer newEngineer)
        {
            if (!ValidateEngineer(newEngineer))
            {
                throw new BO.BlInvalidDataException("Invalid engineer data.");
            }

            try
            {
                int idEngineer = dal.Engineer.Create(new DO.Engineer
                {
                    Id = newEngineer.Id,
                    Email = newEngineer.Email,
                    Cost = newEngineer.Cost,
                    Name = newEngineer.Name,
                    Level = (DO.EngineerExperience?)newEngineer.Level,
                });
                return idEngineer;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Engineer with ID={newEngineer.Id} already exists", ex);
            }
        }

        /// <summary>
        /// Deletes an engineer based on their ID.
        /// </summary>
        /// <param name="engineerId">The ID of the engineer to delete.</param>
        public void Delete(int engineerId)
        {
            try
            {
                DO.Task? doTask = null;

                BO.Engineer? boEngineer = Read(engineerId);

                if (boEngineer != null)
                {
                    try
                    {
                        try { doTask = dal.Task.Read(boEngineer.Task!.Id); }
                        catch { }

                        if (doTask == null || (doTask != null && doTask.CompleteDate != null))
                        {
                            dal.Engineer.Delete(engineerId);
                        }
                        else if (doTask != null && doTask.CompleteDate == null)
                        {
                            throw new BO.BlDeletionImpossibleException($"Engineer with ID={engineerId} is assigned to a task and cannot be deleted.");
                        }
                    }
                    catch (Exception)
                    {
                        throw new BO.BlDeletionImpossibleException($"Engineer with ID={engineerId} is assigned to a task and cannot be deleted.");
                    }
                }
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist", ex);
            }
        }

        /// <summary>
        /// Updates an existing engineer.
        /// </summary>
        /// <param name="updatedEngineer">The updated engineer data.</param>
        public void Update(BO.Engineer updatedEngineer)
        {
            BO.Engineer engineer = Read(updatedEngineer.Id);

            if (updatedEngineer.Level < engineer.Level || !ValidateEngineer(updatedEngineer))
            {
                throw new BO.BlInvalidDataException("Engineer data is invalid.");
            }
            //הוספת בדיקה אם היה שינו כלשהוא
            try
            {
                dal.Engineer.Update(new DO.Engineer
                {
                    Id = updatedEngineer.Id,
                    Email = updatedEngineer.Email,
                    Cost = updatedEngineer.Cost,
                    Name = updatedEngineer.Name,
                    Level = (DO.EngineerExperience?)updatedEngineer.Level,
                });
            }
            catch (Exception)
            {
                throw new BO.BlUnableToUpdateException($"Failed to update engineer with ID={updatedEngineer.Id}.");
            }

            try
            {
                if (Factory.Get().ReturnStartProject() != null && updatedEngineer.Task != null)
                {
                    DO.Task? newTask = dal.Task.Read(e => e.Alias == updatedEngineer.Task.Alias);
                    DO.Task? oldTask = new();

                    try { oldTask = dal.Task.Read(x => x.EngineerId == updatedEngineer.Id && x.CompleteDate == null); }
                    catch (DO.DalDoesNotExistException) { }

                    if (newTask != null && newTask.EngineerId != 0 && newTask.EngineerId != updatedEngineer.Id)
                    {
                        throw new BO.BlAlreadyExistsException("A different engineer is already assigned to the task.");
                    }

                    if (newTask != null && newTask.EngineerId != 0 && newTask.EngineerId == updatedEngineer.Id)
                    {
                        throw new BO.BlNoUpdateWasMadeException("No changes were made to the task.");
                    }

                    if (oldTask?.CompleteDate == null && oldTask?.StartDate != null)
                    {
                        throw new BO.BlUnableToUpdateException("Cannot update task while another task is in progress.");
                    }

                    // Check if all dependent tasks are DONE
                    bool allDependenciesDone = dal.Dependency.ReadAll(dep => dep.DependsOnTask == newTask?.Id)
                        .All(dep => dal.Task.Read((int)dep!.DependentTask!)?.CompleteDate != null);

                    if (allDependenciesDone)
                    {
                        if (newTask != null)
                        {
                            var taskToUpdate = newTask! with { EngineerId = updatedEngineer.Id, StartDate = DateTime.Now };
                            //לשנות את הסטטוס של המשימה
                            //taskToUpdate.Status = BO.Status.Done;
                            dal.Task.Update(taskToUpdate);
                        }
                    }
                    else
                    {
                        throw new BO.BlUnableToUpdateException("Cannot assign the task to the engineer until all dependent tasks are marked as DONE.");
                    }
                }
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Engineer with ID={updatedEngineer.Id} does not exist", ex);
            }
        }

        public void UpdateEngineerTask(int taskId, int engineerId)
        {
            //להוסיף TRY
            // קרא את המהנדס מה DAL
            DO.Engineer doEngineer = dal.Engineer.Read(engineerId)!;
            DO.Task doTask = dal.Task.Read(taskId)!;

            // המר את DO.Engineer ל-BO.Engineer
            BO.Engineer updateEngineer = new()
            {
                Id = doEngineer!.Id,
                Email = doEngineer.Email,
                Cost = doEngineer.Cost,
                Name = doEngineer.Name,
                Level = (BO.EngineerExperience?)doEngineer.Level,
                Task = new BO.TaskInEngineer { Id = doTask!.Id, Alias = doTask.Alias }
                
            };

            // עדכן את המשימה של המהנדס ב BL
            Update(updateEngineer);

            //לקרא למהנדס שוב ולבדוק האם הוא שינה לו את המשימה ולהציג לו הודעה בהתאם - אם כן או לא

        }



        //public void UpdateEngineerTask(int taskId, int engineerId)
        //{
        //    try
        //    {
        //        BO.Engineer updateEngineer = Read(engineerId);
        //        var taskToUpdate = updateEngineer! with { Id = taskId };
        //        dal.Task.Update(taskToUpdate);

        //        //var taskToUpdate = v! with { EngineerId = updatedEngineer.Id, StartDate = DateTime.Now };

        //        // קוד לקריאה של המשימה לפי המזהה taskId מבסיס הנתונים
        //        //DO.Task updatedTask = dal.Task.Read(taskId);


        //        // כאן תוכל לעדכן את הערך Task של המהנדס על פי המשימה שקראת
        //        // לדוגמה:
        //        // BO.Engineer engineer = dal.Engineer.Read(engineerId); // קריאה לפרטי המהנדס מבסיס הנתונים
        //        // engineer.Task = updatedTask; // עדכון המשימה של המהנדס

        //        // כמו כן, תוכל להוסיף כל בדיקות או פעולות נדרשות לפני העדכון של המשימה של המהנדס
        //        // לדוגמה, בדיקה אם המשימה החדשה שקראת עבור המהנדס תואמת לכללי העסקים או אם יש דרישות נוספות לפני העדכון

        //        // כאן תוכל להוסיף הודעת הצלחה או לעדכן משתנה אחר כדי להעביר מידע נוסף לצד המשתמש
        //        //else
        //        //{
        //        //    throw new BlException($"Task with ID {taskId} does not exist.");
        //        //}
        //    }
        //    catch (DalException ex)
        //    {
        //        throw new BlException("Failed to update engineer task.", ex);
        //    }
        //}

    }
}
