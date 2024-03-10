using Bogus;
using Bogus.DataSets;

namespace AspNet.Module.Test.Extensions;

public static class PersonExtensions
{
    public static string MiddleName(this Person person, Name.Gender gender) =>
        Data.MiddleNameFor(person.Random, gender);

    public static string MiddleName(this Name name, Name.Gender gender) =>
        Data.MiddleNameFor(name.Random, gender);

    public static class Data
    {
        public static readonly string[] InnList =
        {
            "500100732259",
            "504203674831"
        };

        public static readonly string[] SnilsList =
        {
            "11697338589",
            "16567072895",
            "19557237212"
        };

        private static readonly string[] MaleMiddleNamesList =
        {
            "Петрович",
            "Александрович",
            "Викторович"
        };

        private static readonly string[] FemaleMiddleNamesList =
        {
            "Петровна",
            "Александровна",
            "Викторовна"
        };

        public static string MiddleNameFor(Randomizer randomizer, Name.Gender gender) =>
            gender switch
            {
                Name.Gender.Male => randomizer.ListItem(MaleMiddleNamesList),
                Name.Gender.Female => randomizer.ListItem(FemaleMiddleNamesList),
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };
    }
}