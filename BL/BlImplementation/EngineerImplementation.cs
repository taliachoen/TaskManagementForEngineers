

using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BlImplementation
{
    internal class EngineerImplementation : IEngineer
    {
        private readonly DalApi.IDal dal = DalApi.Factory.Get;

        //לתקן את בדיקות התקינות
        private static bool ValidateEngineer(BO.Engineer newEngineer)
        {
            if (newEngineer.Id <= 0 
                )
                throw new BO.BlInvalidDataException("Engineer ID must be a positive integer.");
            
            if (string.IsNullOrEmpty(newEngineer.Email))
                throw new BO.BlInvalidDataException("Email cannot be null or empty.");

            if (newEngineer.Cost < 0)
                throw new BO.BlInvalidDataException("Cost must be a non-negative value.");

            if (string.IsNullOrEmpty(newEngineer.Name))
                throw new BO.BlInvalidDataException("Name cannot be null or empty.");
           
           
            return true;
        }

        //Func<BO.Engineer, bool>? filter = null
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

        public BO.Engineer Read(int engineerId)
        {
            DO.Engineer? doEngineer = dal.Engineer.Read(engineerId);
            DO.Task? engTask = null;
            try
            {
                engTask = dal.Task.Read(x => x.EngineerId == engineerId);
            }
            catch (Exception){}
            

            return doEngineer == null
                ? throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} does not exist")
                : new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Email = doEngineer.Email,
                    Cost = (double?)doEngineer.Cost,
                    Name = doEngineer.Name,
                    Level = (BO.EngineerExperience?)doEngineer.Level,
                    Task = engTask!=null ? new BO.TaskInEngineer { Id = engTask.Id, Alias = engTask.Alias }:null

                };
          
        }

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

        //public IEnumerable<BO.Engineer> GetEngineersByLevel(int level)
        //{
        //    var groupedEngineers = dal.Engineer.ReadAll()
        //                            .GroupBy(e => e.Level)
        //                            .FirstOrDefault(group => (int)group.Key == level);

        //    if (groupedEngineers != null)
        //    {
        //        return groupedEngineers.Select(doEngineer => new BO.Engineer
        //        {
        //            Id = doEngineer.Id,
        //            Email = doEngineer.Email,
        //            Cost = (double?)doEngineer.Cost,
        //            Name = doEngineer.Name,
        //            Level = (BO.EngineerExperience?)doEngineer.Level
        //        }).ToList();
        //    }

        //    return Enumerable.Empty<BO.Engineer>();
        //}

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
            //var tasks = dal.Task.ReadAll().Where(t => t?.EngineerId == engineerId);

            try
            {
                var boEngineer = Read(engineerId);
               
                if (boEngineer!.Task != null )
                {
                    
                    throw new BO.BlDeletionImpossible($"Engineer with ID={engineerId} is assigned to a task and cannot be deleted.");
                }
                dal.Engineer.Delete(engineerId);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Engineer with ID={engineerId} already exists", ex);
            }
        }

        //public void Update(BO.Engineer updatedEngineer)
        //{
        //    if (!ValidateEngineer(updatedEngineer))
        //    {
        //        throw new BO.BlInvalidDataException("Invalid engineer data.");
        //    }

        //    try
        //    {
        //        DO.Task? engTask = dal.Task.Read(x => x.EngineerId == updatedEngineer.Id);
        //        //אם הוא רוצה לעדכן גם את המשימה של המהנדס
        //        if (updatedEngineer.Task != null)
        //        {
        //            //האם יש משימה שהמהנדס עובד עליה עכשיו
        //            if (engTask)
        //                //אם המשימה שהמהנדס עובד עליה עכשיו זה המשימה החדשה שהוא רוצה לעדכן
        //                if (updatedEngineer.Task.Id ==)
        //                {//אז לא צריך לעשות ולשנות כלום
        //                }
        //                //אם הוא רוצה לעבוד על משימה אחרת ממה שעבד עליה עד עכשיו
        //                else
        //                {//מציג הודעה / שגיאה שהמהנס לא יכול לשנות את המשימה כשהוא באמצע משימה אחרת
        //                }


        //            //אם אין משימה שהמהנדס עובד עליה
        //            else
        //            { //צריך לחפש את המשימה החדשה ולשנות לה את השדות  - את המזהה של המהנס ואת הסטטוס של המשימה 
        //            }


        //                        //הערות לשאול את המורה
        //                        //צריך לחפש את המשימה שהוא עבד עליה עד עכשיו
        //                        //צריך לעדכן את השדות של המשימה הישנה - את הסטטוס ואת מועד הסיום של המשימה ואת המזהה של המהנדס שעבד עליה עד עכשיו


        //                        //הערות נוספות
        //                        //לעבור על הרשימה של המשימות , לחפש אם יש משימה שהמהנדס עבד עליה עד עכשיו
        //                        //אם כן, לעדכן את המשימה שהיא נגמרה ושהמהנדס כבר לא עובד עליה
        //                        //אם לא, תעדכן את המשימה החדשה שהמהנס התחיל לעבוד עליה
        //                        //dal.Task.Update(existingTask);
        //        }

        //        dal.Engineer.Update(new DO.Engineer
        //        {
        //            Id = updatedEngineer.Id,
        //            Email = updatedEngineer.Email,
        //            Cost = updatedEngineer.Cost,
        //            Name = updatedEngineer.Name,
        //            Level = (DO.EngineerExperience?)updatedEngineer.Level,

        //        });
        //    }
        //    catch (DO.DalDoesNotExistException ex)
        //    {
        //        throw new BO.BlDoesNotExistException($"Engineer with ID={updatedEngineer.Id} does not exist", ex);
        //    }
        //}


        //public void Update(BO.Engineer updatedEngineer)
        //{
        //   //בדיקת תקינות
        //    if (!ValidateEngineer(updatedEngineer))
        //    {
        //        throw new BO.BlInvalidDataException("Invalid engineer data.");
        //    }

        //    try
        //    {
        //        DO.Engineer ?existingEngineer = dal.Engineer.Read(updatedEngineer.Id);

        //        // אם המהנדס רוצה לעדכן גם את המשימה 
        //        if (updatedEngineer.Task != null)
        //        {
        //            //הבאת המשימה שקיימת כבר למהנדס הנוכחי
        //            DO.Task? existingTask = dal.Task.Read(x => x.EngineerId == updatedEngineer.Id);
        //            BO.Task? v = TaskImplementation.Read(existingTask.Id);
        //            // אם המהנדס כבר עובד על משימה
        //            if (existingTask != null)
        //            {
        //                    // אם המהנדס רוצה לעבוד על משימה אחרת ממה שעבד עליה עד עכשיו
        //                if (existingTask.Id != updatedEngineer.Task.Id)
        //                {
        //                    //בדיקה האם המשימה הישנה נגמרה
        //                    if (existingTask.Status = "Done")
        //                    {
        //                        //המהנדס כבר לא עובד על המשימה
        //                        existingTask.EngineerId = null;
        //                        dal.Task.Update(existingTask);

        //                        //עדכון המשימה החדשה 
        //                        BO.Task? newTask = BO.Task.Read(x => x.Id == updatedEngineer.Task.Id);
        //                        newTask.Status = "Scheduled"; 
        //                        newTask.EngineerId = updatedEngineer.Id;
        //                        dal.Task.Update(newTask);

        //                    }
        //                    else
        //                    {
        //                        throw new BO.BlInvalidDataException("Engineer cannot change task during an active task.");
        //                    }

        //                }

        //            }

        //            //אם המהנדס לא עבד על משימה עד העדכון הנוכחי
        //            else
        //            {
        //                //פונקציה חיצונית
        //                BO.Task? newTask = BO.Task.Read(x => x.Id == updatedEngineer.Task.Id);
        //                newTask.Status = "Scheduled";
        //                newTask.EngineerId = updatedEngineer.Id;
        //                dal.Task.Update(newTask);
        //            }
        //        }

        //        // עדכון שאר פרטי המהנדס 
        //        else
        //        {
        //        //existingEngineer.Email = updatedEngineer.Email;
        //        //existingEngineer.Cost = updatedEngineer.Cost;
        //        //existingEngineer.Name = updatedEngineer.Name;
        //        //existingEngineer.Level = (DO.EngineerExperience?)updatedEngineer.Level;

        //        }

        //        dal.Engineer.Update(existingEngineer);
        //    }
        //    catch (DO.DalDoesNotExistException ex)
        //    {
        //        throw new BO.BlDoesNotExistException($"Engineer with ID={updatedEngineer.Id} does not exist", ex);
        //    }
        //}

        public void Update(BO.Engineer updatedEngineer)
        {
            // בדיקת תקינות הנתונים של המהנדס
            if (!ValidateEngineer(updatedEngineer))
            {
                throw new BO.BlInvalidDataException("נתוני המהנדס אינם תקינים.");
            }
            try
            {
                int doEngineerId = dal.Engineer.Create(new DO.Engineer
                {
                    Id = updatedEngineer.Id,
                    Email = updatedEngineer.Email,
                    Cost = updatedEngineer.Cost,
                    Name = updatedEngineer.Name,
                    Level = (DO.EngineerExperience?)updatedEngineer.Level,
                });
                DO.Engineer? e =  dal.Engineer.Read(updatedEngineer.Id);

                if (updatedEngineer!.Task != null)
                {
                    DO.Task? newTask =  dal.Task.Read(updatedEngineer.Id);

                    //לעדכן את הנתונים של המטלה של המהנדס
                    //כלומר, לשלוח את הנשימה לעדכון בעמוד המשימות
                    //BO.Task? boTask = new BO.Task { Id = updatedEngineer.Task.Id, Alias = updatedEngineer.Task.Alias };
                    //TaskImplementation.Update(boTask);
                }

                dal.Engineer.Update(e);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"המהנדס עם זיהוי={updatedEngineer.Id} אינו קיים", ex);
            }
        }

    }
}

