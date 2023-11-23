
namespace DalTest;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Xml.Linq;

public static class Initialization
{
    private static ITask? s_dalTask; 
    private static IEngineer? s_dalEngineer;
    private static IDependensy? s_dalDependensy;

    private static readonly Random s_rand = new();

    private static void CreateTask()
    {
        for (int i=0;i<50;i++)
        {

            DateTime createdAtDate = DateTime.Now;
            TimeSpan requiredEffortTime = TimeSpan.FromDays(s_rand.Next(1, 20));
            DO.EngineerExperience complexity = (DO.EngineerExperience)s_rand.Next(1, 4);
            DateTime startDate = DateTime.Now.AddDays(s_rand.Next(1, 30));
            DateTime scheduledDate = startDate.AddDays(s_rand.Next(1, 10));
            DateTime deadlineDate = scheduledDate.AddDays(s_rand.Next(1, 20));
            DateTime completeDate = deadlineDate.AddDays(s_rand.Next(1, 20));

            Task newTask = new Task(
                0, null, null, createdAtDate, requiredEffortTime,
                null, complexity, startDate, scheduledDate, deadlineDate,
                completeDate,null, null, null
            );

            s_dalTask!.Create(newTask);
        }

    }

    private static void createEngineers()
    {
        string[] engineerEmails =
        {
        "MosheMan@gmail.com", "AviSegal@gmail.com", "YeodaShpiler@gmail.com",
        "EliezerDisler@gmail.com", "ChaimSalomon@gmail.com", "IsraelVeber@gmail.com"
    };
        string[] engineerNames =
        {
        "Moshe Man", "Avi Segal", "Yeoda Shpiler", "Eliezer Disler", "Chaim Salomon", "Israel Veber"
    };
        int i = 0;
        foreach(var name in engineerNames)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dalEngineer!.Read(id) != null);
            int cost = s_rand.Next(100, 500);
            string email = engineerEmails[i++];
            Engineer engineer = new(id, email, cost, name, null);

            s_dalEngineer!.Create(engineer);    
        }
    }

    private static void createDependencies()
    {
       

        for(int i=0;i<30;i++)
        {
            int dependencyId;
            do
                dependencyId = s_rand.Next(100, 9999);
            while (s_dalDependensy!.Read(dependencyId) != null);
            int randomE = randomIdEngineer();
            int randomT = randomIdTask();

            Dependensy newDependency = new Dependensy(
                dependencyId,null , null
            );

            s_dalDependensy!.Create(newDependency);
        }
    }
     
    private static int randomIdEngineer()
    {
        var engineers = s_dalEngineer!.ReadAll().ToArray();
        Engineer randomEngineer = engineers[s_rand.Next(0, engineers.Length)];
        return randomEngineer.Id;
    }

    private static int randomIdTask()
    {
        var task = s_dalTask!.ReadAll().ToArray();
        Task randomTask = task[s_rand.Next(0, task.Length)];
        return randomTask.Id;
    }

}

