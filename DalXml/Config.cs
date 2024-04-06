namespace Dal
{
    /// <summary>
    /// Config class contains configuration settings and properties for the data access layer. 
    /// </summary>

    internal static class Config
    {
        //The path to the XML 
        static readonly string s_data_config_xml = "data-config";
        /// <summary>
        /// Gets the next available task ID from the XML configuration and increments it. 
        /// </summary>
        internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
        /// <summary>
        /// Gets the next available dependency ID from the XML configuration and increments it. 
        /// </summary>
        internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
        // The end date of the project.
        internal static DateTime? endProject
        {
            get => XMLTools.GetStartDate(s_data_config_xml, "endProject");
        }
        // The start date of the project. 
        internal static DateTime? startDate
        {
            get => XMLTools.GetStartDate(s_data_config_xml, "startProject");
        }
    }
}
