
namespace DalTest;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
public static class Initialization
{
    private static ITask? s_dalTask; 
    private static IEngineer? s_dalEngineer;
    private static IDependensy? s_dalDependensy;
    private static readonly Random s_rand = new();


    //Creating initial tasks
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
            DateTime startDate = createdAtDate.AddDays(s_rand.Next(1, 5));
            /*Updating the end of the task according to the start of the task
            and the estimated time to work on the task and another range of days  by lottery*/
            DateTime completeDate = startDate.AddDays(s_rand.Next(1, 5));
            completeDate = completeDate.Add(requiredEffortTime);

            DO.EngineerExperience complexity = (DO.EngineerExperience)s_rand.Next(1, 4);
            string deliverables = "Deliverables for " + taskAlias;
            string remarks = "Remarks for " + taskAlias;
            //int idE = RandomIdEngineer();
            Task newTask = new(
                0, taskAlias, description, createdAtDate, requiredEffortTime,
                false, complexity, startDate, null, null,
                completeDate, deliverables, remarks, null 
                );
            s_dalTask!.Create(newTask);
        }

    }
    //Creating initial engineers
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
            int? cost = s_rand.Next(100, 500);
            string email = engineerEmails[i++];
            DO.EngineerExperience Level = (DO.EngineerExperience)s_rand.Next(1, 4);
            Engineer engineer = new(id, email, cost, name, Level);
            s_dalEngineer!.Create(engineer);    
        }
    }
    //Creating initial dependencies
    private static void CreateDependencies()
    {
        int randomTaskId1;
        int randomTaskId2;
        int dependencyId;

        for(int i=0;i<40;i++)
        {
            randomTaskId1 = RandomIdTask();
            do
                randomTaskId2 = RandomIdTask();
            while (randomTaskId2 == randomTaskId1 || IfDependensySame(randomTaskId1, randomTaskId2));

            Dependensy newDependency = new (
                0, randomTaskId1, randomTaskId2
            );
            s_dalDependensy!.Create(newDependency);
        }

        //Added 3 dependencies on one task
        randomTaskId1 = RandomIdTask();
        for (int i = 0; i < 3; i++)
        {
            do
                randomTaskId2 = RandomIdTask();
            while (randomTaskId2 == randomTaskId1 || IfDependensySame(randomTaskId1, randomTaskId2));

            Dependensy newDependency = new(
                0, randomTaskId1, randomTaskId2
            );

            s_dalDependensy!.Create(newDependency);
        }

        //Function to reating 2 tasks with 3 identical dependencies
        IdentityDependency();

    }

    //Function that checks if the dependency already exists in the inversion
    private static bool IfDependensySame(int randomTaskId1,int randomTaskId2)
    {
        var dependensys = s_dalDependensy!.ReadAll().ToArray();
        for(int i = 0; i < dependensys.Length; i++)
            if (dependensys[i].DependentTask == randomTaskId2 && dependensys[i].DependsOnTask == randomTaskId1)
                return true;
        return false;
    }
    //Lottery for existing task ID
    private static int RandomIdTask()
    {
        var task = s_dalTask!.ReadAll().ToArray();
        Task randomTask = task[s_rand.Next(0, task.Length)];
        return randomTask.Id;
    }
    //Creating initial values for entities
    public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependensy? dalDependensy)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependensy = dalDependensy ?? throw new NullReferenceException("DAL can not be null!");
        CreateTask();
        CreateEngineers();
        CreateDependencies();
    }
    //Creating 2 tasks with 3 identical dependencies
    private static void IdentityDependency()
    {
         for (int numOfTask = 60; numOfTask < 63; numOfTask++)
         {

               Dependensy newDependency1 = new(0,80, numOfTask);
               s_dalDependensy!.Create(newDependency1);

               Dependensy newDependency2 = new(0,90, numOfTask);
               s_dalDependensy!.Create(newDependency2);
         }
    }

}

