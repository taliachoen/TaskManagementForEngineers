

namespace BlApi
{
    public interface IBl
    {
        /// <summary>
        /// Gets the interface for managing engineers.
        /// </summary>
        public IEngineer Engineer { get; }

        /// <summary>
        /// Gets the interface for managing tasks.
        /// </summary>
        public ITask Task { get; }

        /// <summary>
        /// Action to update the project start date.
        /// </summary>
       //public DateTime? StartProject { get; set; }

        /// <summary>
        /// Return action to the project end date.
        /// </summary>
       //public DateTime? EndProject { get; set; }

        /// <summary>
        /// Resets the state of the Business Logic layer.
        /// </summary>
        void Reset();

        /// <summary>
        /// Updates the project schedule based on the planned start date.
        /// </summary>
        /// <param name="plannedStartDate">The planned start date for the project.</param>
        public void UpdateProjectSchedule(DateTime plannedStartDate);

        public DateTime? ReturnStartProject();
        
        public void UpdateStartProject(DateTime date);

        public void InitializeDB();

        public void ResetDB();

        #region System Clock Management

        public DateTime Clock { get; }
        public void AdvanceDay(int days);
        public void AdvanceHour(int hours);
        public void InitializeTime();

        #endregion
    }
}
