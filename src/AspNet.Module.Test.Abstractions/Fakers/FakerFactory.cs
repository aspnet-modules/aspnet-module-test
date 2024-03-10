using Bogus;

namespace AspNet.Module.Test.Fakers;

public static class FakerFactory
{
    public static Faker<TModel> Create<TModel>() where TModel : class
    {
        return new Faker<TModel>("ru");
    }
}