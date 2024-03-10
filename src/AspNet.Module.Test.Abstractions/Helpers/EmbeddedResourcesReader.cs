using System.Reflection;
using System.Text;

namespace AspNet.Module.Test.Helpers;

/// <summary>
///     Получить данные с ресурсов
/// </summary>
public static class EmbeddedResourcesReader
{
    /// <summary>
    ///     Прочитать <see cref="Stream" />
    /// </summary>
    public static byte[]? ReadByteArrayResource(Assembly assembly, string fileName)
    {
        using var stream = ReadStreamResource(assembly, fileName);
        if (stream == null)
        {
            return null;
        }

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    ///     Прочитать <see cref="Stream" />
    /// </summary>
    public static Stream? ReadStreamResource(Assembly assembly, string fileName)
    {
        fileName = ManifestResourceNames(assembly).FirstOrDefault(x => x.Contains(fileName))!;
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        return assembly.GetManifestResourceStream(fileName);
    }

    /// <summary>
    ///     Прочитать <see cref="string" />
    /// </summary>
    public static string? ReadTextResource(Assembly assembly, string fileName)
    {
        using var stream = ReadStreamResource(assembly, fileName);
        if (stream == null)
        {
            return null;
        }

        // ReSharper disable once AssignNullToNotNullAttribute
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    private static IEnumerable<string> ManifestResourceNames(Assembly assembly) => assembly.GetManifestResourceNames();
}