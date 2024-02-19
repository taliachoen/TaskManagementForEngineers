using BlApi;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        // Exposes an instance of EngineerImplementation through the IEngineer interface.
        public IEngineer Engineer => new EngineerImplementation();

        // Exposes an instance of TaskImplementation through the ITask interface.
        public ITask Task => new TaskImplementation();

        // Action to update the project start date.
        public DateTime? StartProject { get; set; } = null;

        // Action to retrieve the project end date.
        public DateTime? EndProject { get; set; } = null;

        /// <summary>
        /// Action to delete entity data from the files.
        /// </summary>
        public void Reset()
        {
            // Calls the Reset method in the data access layer (DAL) to delete entity data.
            DalApi.Factory.Get.Reset();
        }

        /// <summary>
        /// Updates the project schedule based on the planned start date.
        /// </summary>
        /// <param name="plannedStartDate">The planned start date for the project.</param>
        public void UpdateProjectSchedule(DateTime plannedStartDate)
        {
            // Get all tasks from the TaskImplementation.
            var allTasks = Task.ReadAll();
            var maxPreviousStartDate = allTasks.Min(task => task.ScheduledDate ?? DateTime.MinValue);

                if (plannedStartDate > maxPreviousStartDate)
                {
                    throw new InvalidOperationException($"Planned start date for  at least one task is earlier than the latest start date of its preceding tasks.");
                }




            // If checks pass, update the planned start date for the project.
            StartProject = plannedStartDate;

            // Update start dates for tasks.
            var firstTask = allTasks.FirstOrDefault(task => !task.Dependencies!.Any());

            if (firstTask != null)
            {
                // Update the start date of the first task to the planned start date of the project.
                firstTask.StartDate = plannedStartDate;
                Task.Update(firstTask);

                // Update start dates for other tasks based on dependencies.
                foreach (var task in allTasks.Where(t => t != firstTask))
                {
                    DateTime? maxStartDateOfDependencyTask = null;
                    BO.Task? taskWithMaxStartDate = null;

                    if (task.Dependencies != null && task.Dependencies.Any())
                    {
                        var dependenciesStartDates = task.Dependencies
                            .Select(dep =>
                            {
                                var dependencyTask = Task.Read(dep.Id);
                                if (maxStartDateOfDependencyTask < dependencyTask.StartDate)
                                {
                                    maxStartDateOfDependencyTask = dependencyTask.StartDate;
                                    taskWithMaxStartDate = dependencyTask;
                                }
                                return dependencyTask.StartDate;
                            });
                    }

                    if (maxStartDateOfDependencyTask != null)
                    {
                        // Update the start date of the current task based on dependencies.
                        task.StartDate = maxStartDateOfDependencyTask.Value.Add(value: (TimeSpan)taskWithMaxStartDate!.RequiredEffortTime!);
                        Task.Update(task);
                    }
                }
            }
        }
    }
}
