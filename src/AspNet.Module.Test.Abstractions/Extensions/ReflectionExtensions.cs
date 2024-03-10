using System.Reflection;

namespace AspNet.Module.Test.Extensions;

/// <summary>
///     <see href="https://stackoverflow.com/a/1565766" />
/// </summary>
public static class ReflectionExtensions
{
    private const BindingFlags PrivateBindingFlags =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    /// <summary>
    ///     Returns a _private_ Property Value from a given Object. Uses Reflection.
    ///     Throws a ArgumentOutOfRangeException if the Property is not found.
    /// </summary>
    /// <typeparam name="T">Type of the Property</typeparam>
    /// <param name="obj">Object from where the Property Value is returned</param>
    /// <param name="propName">Propertyname as string.</param>
    /// <returns>PropertyValue</returns>
    public static T? GetPropertyValue<T>(this object obj, string propName)
    {
        var pi = obj.GetType().GetProperty(propName, PrivateBindingFlags);
        var value = pi.GetValue(obj, null);
        return value != null ? (T)value : default;
    }

    /// <summary>
    ///     Returns a private Property Value from a given Object. Uses Reflection.
    ///     Throws a ArgumentOutOfRangeException if the Property is not found.
    /// </summary>
    /// <typeparam name="T">Type of the Property</typeparam>
    /// <param name="obj">Object from where the Property Value is returned</param>
    /// <param name="propName">Propertyname as string.</param>
    /// <returns>PropertyValue</returns>
    public static T? GetFieldValue<T>(this object obj, string propName)
    {
        var t = obj.GetType();
        FieldInfo? fi = null;
        while (fi == null && t != null)
        {
            fi = t.GetField(propName, PrivateBindingFlags);
            t = t.BaseType;
        }

        var value = fi?.GetValue(obj);
        return value != null ? (T)value : default;
    }

    /// <summary>
    ///     Sets a _private_ Property Value from a given Object. Uses Reflection.
    ///     Throws a ArgumentOutOfRangeException if the Property is not found.
    /// </summary>
    /// <typeparam name="T">Type of the Property</typeparam>
    /// <param name="obj">Object from where the Property Value is set</param>
    /// <param name="propName">Propertyname as string.</param>
    /// <param name="val">Value to set.</param>
    /// <returns>PropertyValue</returns>
    public static void SetPropertyValue<T>(this object obj, string propName, T val)
    {
        var pi = obj.GetType().GetProperty(propName, PrivateBindingFlags | BindingFlags.SetProperty);
        // only go to the declaring type if you need to
        if (!pi.CanWrite)
        {
            pi = pi.DeclaringType?.GetProperty(propName, PrivateBindingFlags | BindingFlags.SetProperty);
        }

        pi?.SetValue(obj, val, null);
    }

    /// <summary>
    ///     Set a private Property Value on a given Object. Uses Reflection.
    /// </summary>
    /// <typeparam name="T">Type of the Property</typeparam>
    /// <param name="obj">Object from where the Property Value is returned</param>
    /// <param name="propName">Propertyname as string.</param>
    /// <param name="val">the value to set</param>
    /// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
    public static void SetFieldValue<T>(this object obj, string propName, T val)
    {
        var t = obj.GetType();
        FieldInfo? fi = null;
        while (fi == null && t != null)
        {
            fi = t.GetField(propName, PrivateBindingFlags);
            t = t.BaseType;
        }

        fi?.SetValue(obj, val);
    }
}