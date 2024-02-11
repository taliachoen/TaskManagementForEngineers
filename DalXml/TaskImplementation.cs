using DalApi;
using DO;

namespace Dal
{
    // TaskImplementation class implements the ITask interface and provides XML-based data operations for Task entities.
    internal class TaskImplementation : ITask
    {
        // Constant string representing the XML file name for storing Task entities.
        const string s_tasks = @"tasks";

        /// <summary>
        /// Implementation of the Create action for adding a new Task entity to the XML.
        /// </summary>
        /// <param name="task">The Task entity to be created.</param>
        /// <returns>The ID of the newly created Task.</returns>
        public int Create(DO.Task task)
        {
            List<DO.Task>? listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks);

            // Check if the ID already exists, throw an exception if found.
            //if (listTasks.FirstOrDefault(t => t?.Id == task.Id) != null)
            //throw new Exception("id already exists");

            int newId = Config.NextTaskId;
            DO.Task t = new (newId,task.Alias, task.Description, task.CreatedAtDate,task.RequiredEffortTime,task.Copmlexity ,task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate, task.Deliverables ,task.Remarks, task.EngineerId );

            listTasks.Add(t);
            XMLTools.SaveListToXMLSerializer(listTasks, s_tasks);
            return t.Id;
        }

        /// <summary>
        /// Implementation of the Delete action for removing a Task entity by its ID from the XML.
        /// </summary>
        /// <param name="id">The ID of the Task entity to be deleted.</param>
        public void Delete(int id)
        {
            List<DO.Task>? listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks);

            // Remove the Task with the specified ID, throw an exception if not found.
            if (listTasks.RemoveAll(t => t?.Id == id) == 0)
                throw new Exception("missing id");

            XMLTools.SaveListToXMLSerializer(listTasks, s_tasks);
        }

        /// <summary>
        /// Implementation of the Read action for retrieving a Task entity by its ID from the XML.
        /// </summary>
        /// <param name="id">The ID of the Task entity to be retrieved.</param>
        /// <returns>The Task entity with the specified ID.</returns>
        public DO.Task? Read(int id)
        {
            List<DO.Task>? listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks);
            // Return the Task with the specified ID, throw an exception if not found.
            return listTasks.FirstOrDefault(t => t?.Id == id);
        }

        /// <summary>
        /// A function to get a task by checking a filter
        /// </summary>
        /// <param name="filter">The filter condition for selecting Task entities.</param>
        /// <returns>Not applicable.</returns>
        public DO.Task? Read(Func<DO.Task, bool> filter)
        {
            List<DO.Task>? listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks);
            return listTasks.FirstOrDefault(filter)?? throw new DalDoesNotExistException("matching task not found");
        }

        /// <summary>
        /// Implementation of the ReadAll action for retrieving all Task entities from the XML, optionally filtered.
        /// </summary>
        /// <param name="filter">Optional filter condition for selecting Task entities.</param>
        /// <returns>Enumerable collection of Task entities.</returns>
        public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
        {
            List<DO.Task>? listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks);

            // Return all Task entities, ordered by ID, with or without filtering.
            if (filter == null)
            {
                return listTasks.Select(t => t).OrderBy(t => t?.Id);
            }
            else
            {
                return listTasks.Where(filter).OrderBy(t => t?.Id);
            }
        }

        /// <summary>
        /// Implementation of the Update action for updating a Task entity in the XML.
        /// </summary>
        /// <param name="task">The Task entity with updated information.</param>
        public void Update(DO.Task task)
        {
            // Delete the existing Task with the specified ID and create a new one with the updated information.
            Delete(task.Id);
            Create(task);
        }
    }
}
