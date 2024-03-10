using Bogus;

namespace AspNet.Module.Test.Extensions;

public static class FakerExtensions
{
    public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class =>
        // https://github.com/bchavez/Bogus/issues/213#issuecomment-476078291
        faker.CustomInstantiator(f => (T)Activator.CreateInstance(typeof(T), true)!);
}