using System.Collections;
using System.Reflection;
using System.Text;

namespace BO;

public static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        Type type = obj!.GetType();
        PropertyInfo[] properties = type.GetProperties();
        StringBuilder result = new StringBuilder();
        foreach (var property in properties)
        {
            object ?value = property.GetValue(obj);
            if (value != null)
            {
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    // If the property is a class (not string), recursively call ToStringProperty
                    result.Append($"{property.Name}={{ {ToStringProperty(value)} }} ");
                }
                else if (property.PropertyType.IsGenericType &&
                         property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // If the property is a generic list, handle its elements
                    result.Append($"{property.Name}=[ ");
                    foreach (var item in (IEnumerable)value)
                    {
                        result.Append($"{ToStringProperty(item)}, ");
                    }
                    result.Append("] ");
                }
                else
                {
                    // For other types, just append the property name and value
                    result.Append($"{property.Name}={value.ToString()} ");
                }
            }
        }

        return result.ToString().Trim();
    }
}
