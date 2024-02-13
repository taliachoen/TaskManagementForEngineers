
namespace DalTest;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Xml.Linq;
public static class Initialization
{
    private static IDal? s_dal;
    private static readonly Random s_rand = new();


    /// <summary>
    ///initialization the tasks 
    /// </summary>
    private static void CreateTask()
    {
        string[] taskAliases =
        {
        "Task1", "Task2", "Task3", "Task4", "Task5",
        "Task6", "Task7", "Task8", "Task9", "Task10",
        "Task11", "Task12", "Task13", "Task14", "Task15",
        "Task16", "Task17", "Task18", "Task19", "Task20",
        "Task21","Task22","Task23","Task24","Task25","Task26",
        "Task27","Task28","Task29","Task30","Task31","Task32",
        "Task33","Task34", "Task35","Task36","Task37","Task38","Task39","Task40"
        };

        List<Engineer> engineers = s_dal.Engineer.ReadAll().ToList();
        foreach (var taskAlias in taskAliases)
        {
            int randomEngineerIndex = s_rand.Next(engineers.Count);
            int? engineerId = null;
            // אם המספר האקראי גדול מ-0.5, נבחר במהנדס
            if (s_rand.NextDouble() > 0.5)
            {
                engineerId = engineers[randomEngineerIndex].Id;
            }
            string description = "Description for " + taskAlias;
            DateTime createdAtDate = DateTime.Now;
            TimeSpan requiredEffortTime = TimeSpan.FromDays(s_rand.Next(1, 10));
            DateTime startDate = createdAtDate.AddDays(s_rand.Next(1, 5));
            /*Updating the end of the task according to the start of the task
            and the estimated time to work on the task and another range of days  by lottery*/
            DateTime ?completeDate = null;
            EngineerExperience complexity = (EngineerExperience)s_rand.Next(1, 4);
            string deliverables = "Deliverables for " + taskAlias;
            string remarks = "Remarks for " + taskAlias;
            Task newTask = new(
                0, taskAlias, description, createdAtDate, requiredEffortTime,
                complexity, startDate, null, 
                completeDate, deliverables, remarks, engineerId
                );

            s_dal?.Task.Create(newTask);
        }

    }
    
     /// <summary>
    ///initialization the engineers
    /// </summary>
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
        foreach (var name in engineerNames)
        {
            int id;
            do
               id = s_rand.Next(200000000, 400000000);
            while (s_dal?.Engineer.Read(id)!=null);


            int? cost = s_rand.Next(100, 500);
            string email = engineerEmails[i++];
            EngineerExperience Level = (EngineerExperience)s_rand.Next(1, 4);
            Engineer engineer = new(id, email, cost, name, Level);
            s_dal!.Engineer.Create(engineer);    
        }

    }

    /// <summary>
    ///initialization the dependencies 
    /// </summary>
    private static void CreateDependencies()
    {
        int randomTaskId1;
        int randomTaskId2;

        for (int i = 0; i < 40; i++)
        {
            randomTaskId1 = RandomIdTask();
            do
                randomTaskId2 = RandomIdTask();
            while (randomTaskId2 == randomTaskId1 || IfDependencySame(randomTaskId1, randomTaskId2));

            Dependency newDependency = new (
                0, randomTaskId1, randomTaskId2
            );
            s_dal!.Dependency.Create(newDependency);
        }

        //Added 3 dependencies on one task
        randomTaskId1 = RandomIdTask();
        for (int i = 0; i < 3; i++)
        {
            do
                randomTaskId2 = RandomIdTask();
            while (randomTaskId2 == randomTaskId1 || IfDependencySame(randomTaskId1, randomTaskId2));

            Dependency newDependency = new(
                0, randomTaskId1, randomTaskId2
            );

            s_dal!.Dependency.Create(newDependency);
        }

        //Function to reating 2 tasks with 3 identical dependencies
        IdentityDependency();

    }

    //Function that checks if the dependency already exists in the inversion
    private static bool IfDependencySame(int randomTaskId1,int randomTaskId2)
    {
        var Dependencys = s_dal!.Dependency.ReadAll().ToArray();
        for(int i = 0; i < Dependencys.Length; i++)
        {
            if (Dependencys[i]?.DependentTask == randomTaskId2 && Dependencys[i]?.DependsOnTask == randomTaskId1)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    ///ID lottery for an existing task ID
    /// </summary>
    /// <returns></returns>
    private static int RandomIdTask()
    {
        Task?[] temp = s_dal!.Task.ReadAll().ToArray();
        Task ?randomTask = temp[s_rand.Next(0, temp.Length)];
        return randomTask!.Id;
    }

    /// <summary>
    ///Creating initial values for entities 
    /// </summary>
    /// <param name="dal"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    {
        s_dal = Factory.Get ?? throw new NullReferenceException("DAL object can not be null!");
        CreateEngineers();
        CreateTask();
        CreateDependencies();

    }

    /// <summary>
    ///Creating 2 tasks with 3 identical dependencies 
    /// </summary>
    private static void IdentityDependency()
    {
        for (int numOfTask = 60; numOfTask < 63; numOfTask++)
        {

               Dependency newDependency1 = new(0,80, numOfTask);
               s_dal!.Dependency.Create(newDependency1);

               Dependency newDependency2 = new(0,90, numOfTask);
               s_dal!.Dependency.Create(newDependency2);
         }
    }

}

