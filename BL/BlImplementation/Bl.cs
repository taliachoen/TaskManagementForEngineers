﻿using BlApi;
using System;
using System.Xml.Linq;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        // Exposes an instance of EngineerImplementation through the IEngineer interface.
        public IEngineer Engineer => new EngineerImplementation();

        // Exposes an instance of TaskImplementation through the ITask interface.
        public ITask Task => new TaskImplementation();


        /// <summary>
        /// Action to delete entity data from the files.
        /// </summary>
        public void Reset()
        {
            // Calls the Reset method in the data access layer (DAL) to delete entity data.
            DalApi.Factory.Get.Reset();
        }

        public DateTime? ReturnStartProject()
        {
            return DalApi.Factory.Get.ReturnStartProject();
        }

        public void UpdateStartProject(DateTime date)
        {
            DalApi.Factory.Get.UpdateStartProject(date);
        }

        /// <summary>
        /// Updates the project schedule based on the planned start date.
        /// </summary>
        /// <param name="plannedStartDate">The planned start date of the project.</param>ד
        public void UpdateProjectSchedule(DateTime plannedStartDate)
        {
            try
            {
                // Set the planned start date of the project
                //  DateTime StartProject = plannedStartDate;

                // Retrieve all tasks from the TaskImplementation
                var allTasks = Task.ReadAll();

                // Find the first task that has no dependencies
                var firstTask = allTasks.FirstOrDefault(task => !task.Dependencies!.Any());

                if (firstTask != null)
                {
                    // Update the start date of the first task to the planned start date of the project
                    firstTask.ScheduledDate = plannedStartDate;
                    Task.Update(firstTask, true);

                    bool hasUnscheduledTasks = true;
                    while (hasUnscheduledTasks)
                    {
                        hasUnscheduledTasks = false;
                        // Update start dates for other tasks based on dependencies
                        foreach (var task in allTasks.Where(t => t != firstTask))
                        {
                            if (task.ScheduledDate == null)
                            {
                                hasUnscheduledTasks = true;
                                BO.Task dependencyTask = new();
                                var maxStartDateOfDependencyTask = task.Dependencies?.Select(dep =>
                                {
                                    dependencyTask = Task.Read(dep.Id);
                                    return dependencyTask.ScheduledDate;
                                }).Max();

                                // Update the start date of the current task based on dependencies
                                if (maxStartDateOfDependencyTask != null && dependencyTask != null)
                                {
                                    task.ScheduledDate = maxStartDateOfDependencyTask.Value.Add(dependencyTask!.RequiredEffortTime ?? TimeSpan.Zero);
                                    Task.Update(task, true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BO.BlUnableToUpdateException("Error occurred while updating project schedule", ex);
            }
            DalApi.Factory.Get.UpdateStartProject(plannedStartDate);
        }

        
    }
}
