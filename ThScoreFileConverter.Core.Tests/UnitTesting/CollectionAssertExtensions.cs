namespace ThScoreFileConverter.Core.Tests.UnitTesting;

public static class CollectionAssertExtensions
{
    public static void AreEqual<T>(this CollectionAssert _, IEnumerable<T> expected, IEnumerable<T> actual)
    {
        var expectedArray = expected?.ToArray();
        var actualArray = actual?.ToArray();
        CollectionAssert.AreEqual(expectedArray, actualArray);
    }

    public static void AreNotEqual<T>(this CollectionAssert _, IEnumerable<T> expected, IEnumerable<T> actual)
    {
        var expectedArray = expected?.ToArray();
        var actualArray = actual?.ToArray();
        CollectionAssert.AreNotEqual(expectedArray, actualArray);
    }
}
