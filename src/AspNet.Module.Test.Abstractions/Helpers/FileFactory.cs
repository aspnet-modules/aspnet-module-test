namespace AspNet.Module.Test.Helpers;

public static class FileFactory
{
    public static Stream CreateText(string content = "Test content")
    {
        //Setup mock file using a memory stream
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        return ms;
    }
}