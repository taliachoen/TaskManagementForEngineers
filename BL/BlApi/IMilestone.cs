
using BO;

namespace BlApi;

public interface IMilestone
{

   
        /// <summary>
        /// יצירת לו"ז הפרויקט אבני דרך
        /// </summary>
        public void CreateProjectTimeline();

        /// <summary>
        /// בקשת פרטי אבן דרך 
        /// </summary>
        /// <param name="milestoneId">מזהה האבן דרך</param>
        /// <returns>אובייקט אבן דרך משודך</returns>
        public Milestone GetMilestoneDetails(int milestoneId);

        /// <summary>
        /// עדכון אבן דרך
        /// </summary>
        /// <param name="milestoneId">מזהה האבן דרך</param>
        /// <param name="updatedMilestone">אובייקט אבן דרך עם הנתונים המעודכנים</param>
        /// <returns>אובייקט Milestone מעודכן</returns>
        public Milestone UpdateMilestone(int milestoneId, Milestone updatedMilestone);

    
}
