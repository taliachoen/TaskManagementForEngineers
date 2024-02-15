
using BlApi;
using System.Linq;

namespace BlImplementation
{
    internal class EngineerImplementation : IEngineer
    {
        private readonly DalApi.IDal dal = DalApi.Factory.Get;

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

        //linqToObject
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

        //linqToObject
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

        public BO.Engineer Read(int engineerId)
        {
            try
            {
                DO.Engineer? doEngineer = dal.Engineer.Read(engineerId);
                DO.Task? engTask = null;

                try { engTask = dal.Task.Read(x => x.EngineerId == engineerId && x.CompleteDate == null); }
                catch { }

                return doEngineer == null
                    ? throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist")
                    : new BO.Engineer
                    {
                        Id = doEngineer.Id,
                        Email = doEngineer.Email,
                        Cost = (double?)doEngineer.Cost,
                        Name = doEngineer.Name,
                        Level = (BO.EngineerExperience?)doEngineer.Level,
                        Task = engTask != null ? new BO.TaskInEngineer { Id = engTask.Id, Alias = engTask.Alias } : null
                    };
            }
            catch (BO.BlDoesNotExistException ex)
            {
                throw new BO.BlReadImpossibleException($"Error while reading engineer with ID={engineerId}", ex);
            }
        }

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

        public void Update(BO.Engineer updatedEngineer)
        {
            BO.Engineer engineer = Read(updatedEngineer.Id);

            if (updatedEngineer.Level < engineer.Level || !ValidateEngineer(updatedEngineer))
            {
                throw new BO.BlInvalidDataException("Engineer data is invalid.");
            }

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
                    if (updatedEngineer.Task != null && Factory.Get().StartProject != null)
                    {
                    DO.Task? NewTask = dal.Task.Read(e => e.Alias == updatedEngineer.Task.Alias);
                    DO.Task? oldTask = new();

                    try { oldTask = dal.Task.Read(x => x.EngineerId == updatedEngineer.Id && x.CompleteDate == null); }
                    catch (DO.DalDoesNotExistException) { }

                    if (NewTask != null && NewTask.EngineerId != null && NewTask.EngineerId != updatedEngineer.Id)
                    {
                        throw new BO.BlAlreadyExistsException("A different engineer is already assigned to the task.");
                    }

                    if (NewTask != null && NewTask.EngineerId != null && NewTask.EngineerId == updatedEngineer.Id)
                    {
                        throw new BO.BlNoUpdateWasMadeException("No changes were made to the task.");
                    }

                    if (oldTask?.CompleteDate == null)
                    {
                        throw new BO.BlUnableToUpdateException("Cannot update task while another task is in progress.");
                    }

                    if (NewTask != null)
                    {
                        var taskToUpdate = NewTask! with { EngineerId = updatedEngineer.Id, StartDate = DateTime.Now };
                        dal.Task.Update(taskToUpdate);
                    }
                }
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Engineer with ID={updatedEngineer.Id} does not exist", ex);
            }
        }
    }
}