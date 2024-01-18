using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal
{
    //DependencyImplementation class implements the IDependency interface and provides XML-based data operations for Dependency entities.

    internal class DependencyImplementation : IDependency
    {
        // Constant string representing the XML element name for dependencies. 
        const string d_Dependencies = "Dependencies";
        /// <summary>
        /// Action to create a Dependency from XElement.
        /// </summary>
        /// <param name="d"></param>
        /// <returns new Dependency></returns>
        /// <exception cref="FormatException"></exception>
        static Dependency? CreateDependencyfromXElement(XElement d)
        {
            return new Dependency()
            {
                Id = d.ToIntNullable("ID") ?? throw new DalXmlFormatException("Id"),
                DependsOnTask = d.ToIntNullable("DependsOnTask")
            };
        }

        /// <summary>
        /// Implementation of an action to create a new Dependency entity in XML.
        /// </summary>
        /// <param name="doDependency"></param>
        /// <returns></returns>
        public int Create(Dependency doDependency)
        {
            XElement depDependenciesRootElement = XMLTools.LoadListFromXMLElement(d_Dependencies);
            int depenId = Config.NextDependencyId;
            XElement DependencyElement = new ("Dependency",
                new XElement("ID", depenId),
                new XElement("DependentTask", doDependency.DependentTask),
                new XElement("DependsOnTask", doDependency.DependsOnTask)
            );
            depDependenciesRootElement.Add(DependencyElement);
            XMLTools.SaveListToXMLElement(depDependenciesRootElement, d_Dependencies);
            return depenId;
        }
      
        /// <summary>
        /// Implementation of the Delete method for deleting a Dependency entity by its ID from the XML. 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void Delete(int id)
        {
            XElement DependencyRootElem = XMLTools.LoadListFromXMLElement(d_Dependencies);
            XElement? depen = (from d in DependencyRootElem.Elements()
                               where (int?)d.Element("ID") == id
                               select d).FirstOrDefault() ??
                               throw new Exception("missing id");
            depen.Remove();
            XMLTools.SaveListToXMLElement(DependencyRootElem, d_Dependencies);
        }

        /// <summary>
        /// A function to get a dependency by checking a filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dependency? Read(Func<Dependency, bool> filter)
        {
            XElement rootDependesy = XMLTools.LoadListFromXMLElement(d_Dependencies);
            return (from d in rootDependesy?.Elements()
                    let dependesy = CreateDependencyfromXElement(d)
                    where dependesy != null && filter(dependesy)
                    select (Dependency?)dependesy).FirstOrDefault() ?? 
                    throw new DalDoesNotExistException("matching dependency not found");
        }

        /// <summary>
        /// Implement the operation of reading and returning a Dependency entity by its ID from the XML.  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Dependency? Read(int id)
        {
            XElement DependencyRootElement = XMLTools.LoadListFromXMLElement(d_Dependencies);
            return (from d in DependencyRootElement.Elements()
                    where d.ToIntNullable("ID") == id
                    select (Dependency?)CreateDependencyfromXElement(d)).FirstOrDefault();
        }
        
        /// <summary>
        /// Implementation of the ReadAll method to retrieve all Dependency entities from the XML. 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
        {
            XElement? depDependenciesRootElement = XMLTools.LoadListFromXMLElement(d_Dependencies);
            if (filter != null)
            {
                return from d in depDependenciesRootElement.Elements()
                       let doDep = CreateDependencyfromXElement(d)
                       where filter(doDep)
                       select (Dependency?)doDep;
            }
            else
            {
                return from d in depDependenciesRootElement.Elements()
                       select CreateDependencyfromXElement(d);
            }
        }

        /// <summary>
        /// Implementation of the Update action for updating a Dependency entity in the XML. 
        /// </summary>
        /// <param name="doDependency"></param>
        public void Update(Dependency doDependency)
        {
            Delete(doDependency.Id);
            Create(doDependency);
        }
    }
}
