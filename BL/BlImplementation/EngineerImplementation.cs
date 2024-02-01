using BlApi;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private readonly DalApi.IDal engineer_dal = DalApi.Factory.Get;

    /// <summary>
    /// בקשת רשימת מהנדסים
    /// </summary>
    /// <returns>רשימת מהנדסים</returns>
    public IEnumerable<BO.Engineer> GetEngineersList()
    {
        return (from DO.Engineer doEngineer in engineer_dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Email = doEngineer.Email,
                    Cost = (double?)doEngineer.Cost,
                    Name = doEngineer.Name,
                    Level = (BO.EngineerExperience?)doEngineer.Level,
                }).ToList();
    }


    /// <summary>
    /// בקשת רשימת מהנדסים עם סינון על פי רמה מסוימת
    /// </summary>
    /// <param name="level">הרמה לסינון</param>
    /// <returns>רשימת מהנדסים לפי הסינון</returns>
    //public IEnumerable<BO.Engineer> GetEngineersByLevel(int level)
    //{
    //    IEnumerable<DO.Engineer> e = engineer_dal.Engineer.Where(e => e.Level == level).ToList();

    //    return (from DO.Engineer doEngineer in engineer_dal.Engineer.ReadAll()
    //            select new BO.Engineer
    //            {
    //                Id = doEngineer.Id,
    //                Email = doEngineer.Email,
    //                Cost = (double?)doEngineer.Cost,
    //                Name = doEngineer.Name,
    //                Level = (BO.EngineerExperience?)doEngineer.Level,
    //            }).ToList();
    //}

    /// <summary>
    /// בקשת פרטי מהנדס על פי מזהה 
    /// </summary>
    /// <param name="engineerId">מזהה המהנדס</param>
    /// <returns>אובייקט מהנדס שלפי מזהה</returns>
    public BO.Engineer GetEngineerDetails(int engineerId)
    {
        DO.Engineer doEngineer = engineer_dal.Engineer.Read(engineerId);

      //  if (doEngineer == null)
          //  throw new BlDoesNotExistException($"Engineer with ID={engineerId} does not exist");

        return new BO.Engineer
        {
            Id = doEngineer.Id,
            Email = doEngineer.Email,
            Cost = (double?)doEngineer.Cost,
            Name = doEngineer.Name,
            Level = (BO.EngineerExperience?)doEngineer.Level,
        };
    }

    /// <summary>
    /// הוספת מהנדס 
    /// </summary>
    /// <param name="newEngineer">אובייקט מהנדס להוספה</param>
    public void AddEngineer(BO.Engineer newEngineer)
    {
        //אם בדיקת התקינות לא טובה אז צריך להיות זריקת חריגה וכן עקב כפילות מזהה בשכבת נתונים - תפיסת חריגה 
        if (ValidateEngineer(newEngineer)) { 
            DO.Engineer doEngineer = new()
        {
            Id = newEngineer.Id,
            Email = newEngineer.Email,
            Cost = newEngineer.Cost,
            Name = newEngineer.Name,
            Level = (DO.EngineerExperience?)newEngineer.Level,
        };

      //  try
            engineer_dal.Engineer.Create(doEngineer);
       // catch (DalApi.DalAlreadyExistsException ex)
       //     throw new BlAlreadyExistsException($"Engineer with ID={newEngineer.Id} already exists", ex);
       }
       
    }

    /// <summary>
    /// מחיקת מהנדס 
    /// </summary>
    /// <param name="engineerId">מזהה המהנדס למחיקה</param>
    public void DeleteEngineer(int engineerId)
    {
      //  try
            engineer_dal.Engineer.Delete(engineerId);
       // catch (DalApi.DalNotFoundException ex)
      //      throw new BlDoesNotExistException($"Engineer with ID={engineerId} does not exist", ex);
    }

    /// <summary>
    /// עדכון נתונים של מהנדס
    /// </summary>
    /// <param name="updatedEngineer">אובייקט מהנדס עם הנתונים המעודכנים</param>
    public void UpdateEngineerDetails(BO.Engineer updatedEngineer)
    {
       if(ValidateEngineer(updatedEngineer)) { 
        DO.Engineer doEngineer = new ()
        {
            Id = updatedEngineer.Id,
            Email = updatedEngineer.Email,
            Cost = updatedEngineer.Cost,
            Name = updatedEngineer.Name,
            Level = (DO.EngineerExperience?)updatedEngineer.Level,
        };

        //try
            engineer_dal.Engineer.Update(doEngineer);
        
            }
        //catch (DalApi.DalNotFoundException ex)
            //throw new BlDoesNotExistException($"Engineer with ID={updatedEngineer.Id} does not exist", ex);
    }


    //בדיקות תקינות
    public bool ValidateEngineer(BO.Engineer newEngineer)
    {
        // Check if Id is valid (assuming it should be a positive integer)
        if (newEngineer.Id <= 0)
        {
            Console.WriteLine("Invalid Id. It should be a positive integer.");
            return false;
        }

        // Check if Email is not null or empty
        if (string.IsNullOrEmpty(newEngineer.Email))
        {
            Console.WriteLine("Email cannot be null or empty.");
            return false;
        }

        // Check if Cost is a non-negative value
        if (newEngineer.Cost < 0)
        {
            Console.WriteLine("Invalid Cost. It should be a non-negative value.");
            return false;
        }

        // Check if Name is not null or empty
        if (string.IsNullOrEmpty(newEngineer.Name))
        {
            Console.WriteLine("Name cannot be null or empty.");
            return false;
        }

        // Check if Level is within the valid range of DO.EngineerExperience enum
        if (!Enum.IsDefined(typeof(DO.EngineerExperience), newEngineer.Level))
        {
            Console.WriteLine("Invalid Level. It should be a valid EngineerExperience enum value.");
            return false;
        }

        // All checks passed, the engineer is valid
        return true;
    }

}

