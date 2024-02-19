using System.Collections;
using System.Reflection;
using System.Text;

namespace BO;


public static class Tools
{
    /// <summary>
    /// Generates a string representation of an object and its properties.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object for which to generate the string representation.</param>
    /// <returns>A string representation of the object and its properties.</returns>
    public static string ToStringProperty<T>(this T obj)
    {
        // Get the type of the object
        Type type = obj!.GetType();

        // Get all properties of the object
        PropertyInfo[] properties = type.GetProperties();

        // StringBuilder to build the result string
        StringBuilder result = new ();

        // Iterate through each property
        foreach (var property in properties)
        {
            // Get the value of the property
            object? value = property.GetValue(obj);

            // Check if the value is not null
            if (value != null)
            {
                // Check if the property type is a class (not string)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    // If the property is IEnumerable, handle its elements
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        result.Append($"{property.Name}=[ ");

                        // Recursively call ToStringProperty for each element in IEnumerable
                        foreach (var item in (IEnumerable)value)
                        {
                            result.Append($"{ToStringProperty(item)}, ");
                        }

                        result.Append("] ");
                    }
                    else
                    {
                        // If the property is a class (not string), recursively call ToStringProperty
                        result.Append($"{property.Name}={{ {ToStringProperty(value)} }} ");
                    }
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
