using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal
{
    //DependensyImplementation class implements the IDependensy interface and provides XML-based data operations for Dependensy entities.

    internal class DependensyImplementation : IDependensy
    {
        // Constant string representing the XML element name for dependencies. 
        const string d_dependensies = "dependensies";
        /// <summary>
        /// Action to create a Dependensy from XElement.
        /// </summary>
        /// <param name="d"></param>
        /// <returns new Dependensy></returns>
        /// <exception cref="FormatException"></exception>
        static Dependensy? CreateDependensyfromXElement(XElement d)
        {
            return new Dependensy()
            {
                Id = d.ToIntNullable("ID") ?? throw new DalXmlFormatException("Id"),
                DependsOnTask = d.ToIntNullable("DependsOnTask")
            };
        }

        /// <summary>
        /// Implementation of an action to create a new Dependency entity in XML.
        /// </summary>
        /// <param name="doDependensy"></param>
        /// <returns></returns>
        public int Create(Dependensy doDependensy)
        {
            XElement depdependensiesRootElemnt = XMLTools.LoadListFromXMLElement(d_dependensies);
            int depenId = Config.NextDependencyId;
            XElement dependensyElement = new XElement("Dependensy",
                new XElement("ID", depenId),
                new XElement("DependentTask", doDependensy.DependentTask),
                new XElement("DependsOnTask", doDependensy.DependsOnTask)
            );
            depdependensiesRootElemnt.Add(dependensyElement);
            XMLTools.SaveListToXMLElement(depdependensiesRootElemnt, d_dependensies);
            return depenId;
        }
      
        /// <summary>
        /// Implementation of the Delete method for deleting a Dependensy entity by its ID from the XML. 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void Delete(int id)
        {
            XElement dependensyRootElem = XMLTools.LoadListFromXMLElement(d_dependensies);
            XElement? depen = (from d in dependensyRootElem.Elements()
                               where (int?)d.Element("ID") == id
                               select d).FirstOrDefault() ??
                               throw new Exception("missing id");
            depen.Remove();
            XMLTools.SaveListToXMLElement(dependensyRootElem, d_dependensies);
        }

        /// <summary>
        /// A function to get a dependency by checking a filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dependensy? Read(Func<Dependensy, bool> filter)
        {
            XElement rootDependesy = XMLTools.LoadListFromXMLElement(d_dependensies);
            return (from d in rootDependesy?.Elements()
                    let dependesy = CreateDependensyfromXElement(d)
                    where dependesy != null && filter(dependesy)
                    select (Dependensy?)dependesy).FirstOrDefault() ?? 
                    throw new DalDoesNotExistException("matching dependency not found");
        }

        /// <summary>
        /// Implement the operation of reading and returning a Dependensy entity by its ID from the XML.  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Dependensy? Read(int id)
        {
            XElement dependensyRootElement = XMLTools.LoadListFromXMLElement(d_dependensies);
            return (from d in dependensyRootElement.Elements()
                    where d.ToIntNullable("ID") == id
                    select (Dependensy?)CreateDependensyfromXElement(d)).FirstOrDefault();
        }
        
        /// <summary>
        /// Implementation of the ReadAll method to retrieve all Dependensy entities from the XML. 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Dependensy?> ReadAll(Func<Dependensy, bool>? filter = null)
        {
            XElement? depdependensiesRootElement = XMLTools.LoadListFromXMLElement(d_dependensies);
            if (filter != null)
            {
                return from d in depdependensiesRootElement.Elements()
                       let doDep = CreateDependensyfromXElement(d)
                       where filter(doDep)
                       select (Dependensy?)doDep;
            }
            else
            {
                return from d in depdependensiesRootElement.Elements()
                       select CreateDependensyfromXElement(d);
            }
        }

        /// <summary>
        /// Implementation of the Update action for updating a Dependensy entity in the XML. 
        /// </summary>
        /// <param name="doDependensy"></param>
        public void Update(Dependensy doDependensy)
        {
            Delete(doDependensy.Id);
            Create(doDependensy);
        }
    }
}
