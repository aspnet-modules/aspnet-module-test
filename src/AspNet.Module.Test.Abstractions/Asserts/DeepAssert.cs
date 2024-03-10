using KellermanSoftware.CompareNetObjects;
using Shouldly;

namespace AspNet.Module.Test.Asserts;

public static class DeepAssert
{
    public static void Equal(object? obj1, object? obj2, Action<CompareLogic>? configure = null)
    {
        var compareLogic = new CompareLogic();
        configure?.Invoke(compareLogic);
        var result = compareLogic.Compare(obj1, obj2);
        //These will be different, write out the differences
        result.AreEqual.ShouldBeTrue(result.DifferencesString);
    }
}