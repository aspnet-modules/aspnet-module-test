using Bogus;

namespace AspNet.Module.Test.Extensions;

public static class RandomizerExtensions
{
    private const string Chars = "0123456789";

    public static string StringNumber(this Randomizer random, int length) => random.String2(length, Chars);

    public static string StringNumber(this Randomizer random, int minLength, int maxLength) =>
        random.String2(minLength, maxLength, Chars);

    public static string MirNumber(this Randomizer randomizer, DateTime date)
    {
        var dateStr = date.ToString("yyyyMM");
        return $"REQ/TEST-{dateStr}-{randomizer.StringNumber(12)}";
    }

    public static string PolicyNumber(this Randomizer randomizer, DateTime date)
    {
        var dateStr = date.ToString("yyyy");
        return $"{dateStr}/TEST/{randomizer.StringNumber(12)}";
    }
}