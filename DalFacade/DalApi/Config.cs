namespace DalApi;
using System;
using System.Xml.Linq;


/// <summary>
/// Static class containing DAL configuration information.
/// </summary>
static class Config
{
    /// <summary>
    /// Internal PDS class for DAL implementation details.
    /// </summary>
    internal record DalImplementation
    (
        string Package,   // Package/DLL name
        string Namespace, // Namespace where DAL implementation class is contained
        string Class   // DAL implementation class name
    );
    internal static string s_dalName;
    internal static Dictionary<string, DalImplementation> s_dalPackages;



    /// <summary>
    /// Static constructor to load DAL configuration from XML.
    /// </summary>
    static Config()
    {
        XElement dalConfig = XElement.Load(@"..\xml\dal-config.xml") ??
            throw new DalConfigException("dal-config.xml file is not found");

        s_dalName = dalConfig.Element("dal")?.Value ?? throw new DalConfigException("<dal> element is missing");

        var packages = dalConfig.Element("dal-packages")?.Elements() ??
            throw new DalConfigException("<dal-packages> element is missing");

        s_dalPackages = (from item in packages
                         let pkg = item.Value
                         let ns = item.Attribute("namespace")?.Value ?? "Dal"
                         let cls = item.Attribute("class")?.Value ?? pkg
                         select (item.Name, new DalImplementation(pkg, ns, cls))
                        ).ToDictionary(p => "" + p.Name, p => p.Item2);
    }
}

/// <summary>
/// Exception thrown for DAL configuration issues.
/// </summary>
[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}
