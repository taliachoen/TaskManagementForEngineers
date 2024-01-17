using DalApi;
using DO;

namespace Dal
{
    //EngineerImplementation class implements the IEngineer interface and provides XML-based data operations for Engineer entities.
    internal class EngineerImplementation : IEngineer
    {

        // Constant string representing the XML file name for storing Engineer entities.
        const string s_engineers = @"engineers";
       
        /// <summary>
        /// Create a new engineer entity to XML
        /// </summary>
        /// <param name="engineer"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Create(Engineer engineer)
        {
            List<Engineer>? listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
            // Check if the ID already exists, throw an exception if found.
            if (listEngineers.FirstOrDefault(e => e?.Id == engineer.Id) != null)
                throw new Exception("id already exists");
            listEngineers.Add(engineer);
            XMLTools.SaveListToXMLSerializer(listEngineers, s_engineers);
            return engineer.Id;
        }
        
        /// <summary>
        /// Implementation of the Delete action for removing an Engineer entity by its ID from the XML.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void Delete(int id)
        {
            List<Engineer>? listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
            // Remove the Engineer with the specified ID, throw an exception if not found.
            if (listEngineers.RemoveAll(e => e?.Id == id) == 0)
                throw new Exception("missing id");
            XMLTools.SaveListToXMLSerializer(listEngineers, s_engineers);
        }

        /// <summary>
        /// Implement a READ operation to search for an engineer by its ID from the XML. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Engineer? Read(int id)
        {
            List<Engineer>? listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
            // Return the Engineer with the specified ID, throw an exception if not found.
            return listEngineers.FirstOrDefault(e => e?.Id == id);
        }

        /// <summary>
        /// A function to get a engineer by checking a filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Engineer? Read(Func<Engineer, bool> filter)
        {
            List<Engineer>? listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
            return listEngineers.FirstOrDefault(filter) ?? throw new DalDoesNotExistException("matching engineer not found");
        }
        
        /// <summary>
        /// Implementation of the ReadAll action for retrieving all Engineer entities from the XML, optionally filtered.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
        {
            List<DO.Engineer>? listTasks = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineers);
            // Return all Engineer entities, ordered by ID, with or without filtering.
            if (filter == null)
            {
                return listTasks.Select(e => e).OrderBy(e => e?.Id);
            }
            else
            {
                return listTasks.Where(filter).OrderBy(e => e?.Id);
            }
        }
       
        /// <summary>
        /// Implementation of the Update action for updating an Engineer entity in the XML.
        /// </summary>
        /// <param name="engineer"></param>
        public void Update(Engineer engineer)
        {
            // Delete the existing Engineer with the specified ID and create a new one with the updated information.
            Delete(engineer.Id);
            Create(engineer);
        }
    }
}
