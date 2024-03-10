using System.Reflection;

namespace AspNet.Module.Test.Helpers;

/// <summary>
///     Фабрика тестовых картинок
/// </summary>
public static class ImageFactory
{
    private static Assembly Assembly => typeof(ImageFactory).Assembly;

    /// <summary>
    ///     Создать тестовую картинку Jpeg
    /// </summary>
    public static byte[] CreateJpegAsByteArray() =>
        EmbeddedResourcesReader.ReadByteArrayResource(Assembly, "image.jpg")!;

    /// <summary>
    ///     Создать тестовую картинку Jpeg
    /// </summary>
    public static Stream CreateJpegAsStream() =>
        EmbeddedResourcesReader.ReadStreamResource(Assembly, "image.jpg")!;

    /// <summary>
    ///     Создать тестовую картинку Png
    /// </summary>
    public static byte[] CreatePngAsByteArray() =>
        EmbeddedResourcesReader.ReadByteArrayResource(Assembly, "image.png")!;

    /// <summary>
    ///     Создать тестовую картинку Png
    /// </summary>
    public static Stream CreatePngAsStream() =>
        EmbeddedResourcesReader.ReadStreamResource(Assembly, "image.png")!;
}