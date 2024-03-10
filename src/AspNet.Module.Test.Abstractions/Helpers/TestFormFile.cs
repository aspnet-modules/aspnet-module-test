using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace AspNet.Module.Test.Helpers;

public static class TestFormFile
{
    public static IFormFile CreateByteFile(string fileName, byte[] byteArray)
    {
        //Arrange
        var fileMock = Substitute.For<IFormFile>();
        //Setup mock file using a memory stream
        var ms = new MemoryStream(byteArray);
        var writer = new StreamWriter(ms);
        writer.Write(byteArray);
        writer.Flush();
        ms.Position = 0;
        fileMock.OpenReadStream().Returns(ms);
        fileMock.FileName.Returns(fileName);
        fileMock.Length.Returns(ms.Length);

        return fileMock;
    }

    public static IFormFile CreateImage(string fileName, Stream imageStream)
    {
        var fileMock = Substitute.For<IFormFile>();
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(imageStream);
        writer.Flush();
        stream.Position = 0;
        fileMock.OpenReadStream().Returns(stream);
        fileMock.FileName.Returns(fileName);
        fileMock.Length.Returns(stream.Length);

        return fileMock;
    }

    public static IFormFile CreateImageByBase64(string fileName, string base64EncodedStr)
    {
        var bytes = Convert.FromBase64String(base64EncodedStr);
        return CreateImage(fileName, new MemoryStream(bytes));
    }

    public static IFormFile CreateImageByFile(string fileName, string imagePath)
    {
        var sourceImg = File.OpenRead(imagePath);
        return CreateImage(fileName, sourceImg);
    }

    public static IFormFile CreateTextFile(string fileName = "test.txt", string content = "Test content")
    {
        //Arrange
        var fileMock = Substitute.For<IFormFile>();
        var ms = FileFactory.CreateText(content);
        fileMock.OpenReadStream().Returns(ms);
        fileMock.FileName.Returns(fileName);
        fileMock.Name.Returns(fileName);
        fileMock.Length.Returns(ms.Length);

        return fileMock;
    }
}