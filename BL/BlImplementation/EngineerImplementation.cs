//using BlApi;
//namespace BlImplementation;

//internal class EngineerImplementation : IEngineer
//{
//    private readonly DalApi.IDal engineer_dal = DalApi.Factory.Get;

//    //בדיקות תקינות
//    public static bool ValidateEngineer(BO.Engineer newEngineer)
//    {
//        // Check if Id is valid (assuming it should be a positive integer)
//        if (newEngineer.Id <= 0)
//        {
//            Console.WriteLine("Invalid Id. It should be a positive integer.");
//            return false;
//        }

//        // Check if Email is not null or empty
//        if (string.IsNullOrEmpty(newEngineer.Email))
//        {
//            Console.WriteLine("Email cannot be null or empty.");
//            return false;
//        }

//        // Check if Cost is a non-negative value
//        if (newEngineer.Cost < 0)
//        {
//            Console.WriteLine("Invalid Cost. It should be a non-negative value.");
//            return false;
//        }

//        // Check if Name is not null or empty
//        if (string.IsNullOrEmpty(newEngineer.Name))
//        {
//            Console.WriteLine("Name cannot be null or empty.");
//            return false;
//        }

//        //// Check if Level is within the valid range of DO.EngineerExperience enum
//        //if (!Enum.IsDefined(typeof(DO.EngineerExperience), newEngineer.Level))
//        //{
//        //    Console.WriteLine("Invalid Level. It should be a valid EngineerExperience enum value.");
//        //    return false;
//        //}

//        // All checks passed, the engineer is valid
//        return true;
//    }

//    /// <summary>
//    /// בקשת רשימת מהנדסים
//    /// </summary>
//    /// <returns>רשימת מהנדסים</returns>
//    public IEnumerable<BO.Engineer> ReadAll()
//    {
//        return (from DO.Engineer doEngineer in engineer_dal.Engineer.ReadAll()
//                select new BO.Engineer
//                {
//                    Id = doEngineer.Id,
//                    Email = doEngineer.Email,
//                    Cost = (double?)doEngineer.Cost,
//                    Name = doEngineer.Name,
//                    Level = (BO.EngineerExperience?)doEngineer.Level,
//                }).ToList();
//    }

//    /// <summary>
//    /// בקשת רשימת מהנדסים עם סינון על פי רמה מסוימת
//    /// </summary>
//    /// <param name="level">הרמה לסינון</param>
//    /// <returns>רשימת מהנדסים לפי הסינון</returns>
//    public IEnumerable<BO.Engineer> GetEngineersByLevel(int level)
//    {
//        IEnumerable<DO.Engineer?> engineers = engineer_dal.Engineer.ReadAll().Where(e => (int?)e?.Level == level);

//        return engineers.Select(doEngineer => new BO.Engineer
//        {
//            Id = doEngineer.Id,
//            Email = doEngineer.Email,
//            Cost = (double?)doEngineer.Cost,
//            Name = doEngineer.Name,
//            Level = (BO.EngineerExperience?)doEngineer.Level
//        }).ToList();
//    }

//    /// <summary>
//    /// בקשת פרטי מהנדס על פי מזהה 
//    /// </summary>
//    /// <param name="engineerId">מזהה המהנדס</param>
//    /// <returns>אובייקט מהנדס שלפי מזהה</returns>
//    public BO.Engineer Read(int engineerId)
//    {
//        DO.Engineer? doEngineer = engineer_dal.Engineer.Read(engineerId);

//        return doEngineer == null
//            ? throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist")
//            : new BO.Engineer
//             {
//            Id = doEngineer.Id,
//            Email = doEngineer.Email,
//            Cost = (double?)doEngineer.Cost,
//            Name = doEngineer.Name,
//            Level = (BO.EngineerExperience?)doEngineer.Level,
//             };
//    }

//    /// <summary>
//    /// הוספת מהנדס 
//    /// </summary>
//    /// <param name="newEngineer">אובייקט מהנדס להוספה</param>
//    public int Create(BO.Engineer newEngineer)
//    {
//        //אם בדיקת התקינות לא טובה אז צריך להיות זריקת חריגה וכן עקב כפילות מזהה בשכבת נתונים - תפיסת חריגה 
//        if (ValidateEngineer(newEngineer))
//        {
//            DO.Engineer doEngineer = new()
//            {
//                Id = newEngineer.Id,
//                Email = newEngineer.Email,
//                Cost = newEngineer.Cost,
//                Name = newEngineer.Name,
//                Level = (DO.EngineerExperience?)newEngineer.Level,
//            };

//            try { engineer_dal.Engineer.Create(doEngineer); }

//            catch (DO.DalAlreadyExistsException ex)
//            {
//                throw new BO.BlAlreadyExistsException($"Engineer with ID={newEngineer.Id} already exists", ex);
//            }

//        }
//        return newEngineer.Id;
//    }

//    /// <summary>
//    /// מחיקת מהנדס 
//    /// </summary>
//    /// <param name="engineerId">מזהה המהנדס למחיקה</param>
//    public void Delete(int engineerId)
//    {
//        try { engineer_dal.Engineer.Delete(engineerId); }

//        catch (DO.DalDoesNotExistException ex)
//        {
//            throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist", ex);
//        }

//    }

//    /// <summary>
//    /// עדכון נתונים של מהנדס
//    /// </summary>
//    /// <param name="updatedEngineer">אובייקט מהנדס עם הנתונים המעודכנים</param>
//    public void Update(BO.Engineer updatedEngineer)
//    {
//        if (ValidateEngineer(updatedEngineer))
//        {
//            DO.Engineer doEngineer = new()
//            {
//                Id = updatedEngineer.Id,
//                Email = updatedEngineer.Email,
//                Cost = updatedEngineer.Cost,
//                Name = updatedEngineer.Name,
//                Level = (DO.EngineerExperience?)updatedEngineer.Level,
//            };

//            try { engineer_dal.Engineer.Update(doEngineer); }

//            catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException($"Engineer with ID={updatedEngineer.Id} does not exist", ex); }

//        }
//    }

//}

using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BlImplementation
{
    internal class EngineerImplementation : IEngineer
    {
        private readonly DalApi.IDal engineer_dal = DalApi.Factory.Get;

        //לתקן את בדיקות התקינות
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
           
            if(newEngineer.Level<=0)
                throw new BO.BlInvalidDataException("Name cannot be null or empty.");

            return true;
        }

        //Func<T, bool>? filter = null
        public IEnumerable<BO.Engineer> ReadAll()
        {
            return engineer_dal.Engineer.ReadAll()
                .Select(doEngineer => new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Email = doEngineer.Email,
                    Cost = (double?)doEngineer.Cost,
                    Name = doEngineer.Name,
                    Level = (BO.EngineerExperience?)doEngineer.Level,
                }).ToList();
        }

        public BO.Engineer Read(int engineerId)
        {
            DO.Engineer? doEngineer = engineer_dal.Engineer.Read(engineerId);

            return doEngineer == null
                ? throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist")
                : new BO.Engineer
                {
                Id = doEngineer.Id,
                Email = doEngineer.Email,
                Cost = (double?)doEngineer.Cost,
                Name = doEngineer.Name,
                Level = (BO.EngineerExperience?)doEngineer.Level,
                };
        }

        //public IEnumerable<BO.Engineer> GetEngineersByLevel(int level)
        //{
        //    return engineer_dal.Engineer.ReadAll()
        //        .Where(e => (int?)e?.Level == level)
        //        .Select(doEngineer => new BO.Engineer
        //        {
        //            Id = doEngineer.Id,
        //            Email = doEngineer.Email,
        //            Cost = (double?)doEngineer.Cost,
        //            Name = doEngineer.Name,
        //            Level = (BO.EngineerExperience?)doEngineer.Level
        //        }).ToList();
        //}

        public IEnumerable<BO.Engineer> GetEngineersByLevel(int level)
        {
            var groupedEngineers = engineer_dal.Engineer.ReadAll()
                                    .GroupBy(e => e.Level)
                                    .FirstOrDefault(group => (int)group.Key == level);

            if (groupedEngineers != null)
            {
                return groupedEngineers.Select(doEngineer => new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Email = doEngineer.Email,
                    Cost = (double?)doEngineer.Cost,
                    Name = doEngineer.Name,
                    Level = (BO.EngineerExperience?)doEngineer.Level
                }).ToList();
            }

            return Enumerable.Empty<BO.Engineer>();
        }

        public int Create(BO.Engineer newEngineer)
        {
            if (!ValidateEngineer(newEngineer))
            {
                throw new BO.BlInvalidDataException("Invalid engineer data.");
            }

            try
            {
                engineer_dal.Engineer.Create(new DO.Engineer
                {
                    Id = newEngineer.Id,
                    Email = newEngineer.Email,
                    Cost = newEngineer.Cost,
                    Name = newEngineer.Name,
                    Level = (DO.EngineerExperience?)newEngineer.Level,
                });
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Engineer with ID={newEngineer.Id} already exists", ex);
            }

            return newEngineer.Id;
        }

        public void Delete(int engineerId)
        {
        //var tasks = engineer_dal.Task.ReadAll().Where(t => t?.EngineerId == engineerId);

           var doEngineer = engineer_dal.Engineer.Read(engineerId);
           // if (doEngineer.Task != null && doEngineer.Task.EngineerId == engineerId)
            {
           //     throw new BO.BlDeletionImpossible($"Engineer with ID={engineerId} is assigned to a task and cannot be deleted.");
            }

            try
            {
                engineer_dal.Engineer.Delete(engineerId);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist", ex);
            }
        }

        public void Update(BO.Engineer updatedEngineer)
        {
            if (!ValidateEngineer(updatedEngineer))
            {
                throw new BO.BlInvalidDataException("Invalid engineer data.");
            }

            try
            {
                engineer_dal.Engineer.Update(new DO.Engineer
                {
                    Id = updatedEngineer.Id,
                    Email = updatedEngineer.Email,
                    Cost = updatedEngineer.Cost,
                    Name = updatedEngineer.Name,
                    Level = (DO.EngineerExperience?)updatedEngineer.Level,
                });
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Engineer with ID={updatedEngineer.Id} does not exist", ex);
            }
        }
    }
}

