using Bogus.DataSets;

namespace AspNet.Module.Test.Extensions;

public static class PhoneExtensions
{
    private const string Format = "+7##########";

    public static string NormalizePhoneNumber(this PhoneNumbers phoneNumbers) => phoneNumbers.PhoneNumber(Format);
}