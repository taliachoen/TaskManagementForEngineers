
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
        string[] taskAliases =
    {
        "Task1", "Task2", "Task3", "Task4", "Task5",
        "Task6", "Task7", "Task8", "Task9", "Task10",
        "Task11", "Task12", "Task13", "Task14", "Task15",
        "Task16", "Task17", "Task18", "Task19", "Task20"
    };

        foreach (var taskAlias in taskAliases)
        {
            string description = "Description for " + taskAlias;
            DateTime createdAtDate = DateTime.Now;
            TimeSpan requiredEffortTime = TimeSpan.FromDays(s_rand.Next(1, 10));
            DateTime scheduledDate = createdAtDate.AddDays(s_rand.Next(5, 10));
            DateTime startDate = scheduledDate.AddDays(s_rand.Next(1, 5));
            DateTime completeDate = startDate.AddDays(s_rand.Next(1, 5));
            completeDate = completeDate.Add(requiredEffortTime);

            DO.EngineerExperience complexity = (DO.EngineerExperience)s_rand.Next(1, 4);
            string deliverables = "Deliverables for " + taskAlias;
            string remarks = "Remarks for " + taskAlias;
            int idE = RandomIdEngineer();
            Task newTask = new(
                0, taskAlias, description, createdAtDate, requiredEffortTime,
                false, complexity, startDate, scheduledDate, null,
                completeDate, deliverables, remarks, idE 
                );
            s_dalTask!.Create(newTask);
        }

    }

    private static void CreateEngineers()
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
            DO.EngineerExperience Level = (DO.EngineerExperience)s_rand.Next(1, 4);
            Engineer engineer = new(id, email, cost, name, Level);
            s_dalEngineer!.Create(engineer);    
        }
    }

    private static void CreateDependencies()
    {
        int randomT1;
        int randomT2;
        int dependencyId;

        for(int i=0;i<50;i++)
        {
            do
                dependencyId = s_rand.Next(10, 99);
            while (s_dalDependensy!.Read(dependencyId) != null);
           
            randomT1 = RandomIdTask();
            do
                randomT2 = RandomIdTask();
            while (randomT2 == randomT1);

            Dependensy newDependency = new (
                dependencyId, randomT1, randomT2
            );

            s_dalDependensy!.Create(newDependency);
        }
    }
     
    private static int RandomIdEngineer()
    {
        var engineers = s_dalEngineer!.ReadAll().ToArray();
        Engineer randomEngineer = engineers[s_rand.Next(0, engineers.Length)];
        return randomEngineer.Id;
    }

    private static int RandomIdTask()
    {
        var task = s_dalTask!.ReadAll().ToArray();
        Task randomTask = task[s_rand.Next(0, task.Length)];
        return randomTask.Id;
    }

    public static void Do(ITask? dalTask, IEngineer dalEngineer, IDependensy dalDependensy)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependensy = dalDependensy ?? throw new NullReferenceException("DAL can not be null!");
        CreateTask();
        CreateEngineers();
        CreateDependencies();
    }

}

