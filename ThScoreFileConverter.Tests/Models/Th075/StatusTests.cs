using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class StatusTests
{
    internal struct Properties(in Properties properties)
    {
        public byte[] encodedLastName = [.. properties.encodedLastName];
        public string decodedLastName = properties.decodedLastName;
        public IReadOnlyDictionary<(CharaWithReserved player, CharaWithReserved enemy), int> arcadeScores = properties.arcadeScores.ToDictionary();
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        encodedLastName = [15, 37, 26, 50, 30, 43, 53, 103],
        decodedLastName = "Player1 ",
        arcadeScores = EnumHelper.Cartesian<CharaWithReserved, CharaWithReserved>()
            .ToDictionary(pair => pair, pair => ((int)pair.First * 100) + (int)pair.Second),
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.encodedLastName,
            properties.arcadeScores.Values.Select(score => score + 10),
            new byte[0x128]);
    }

    internal static void Validate(in Properties properties, in Status status)
    {
        Assert.AreEqual(properties.decodedLastName, status.LastName);
        CollectionAssert.That.AreEqual(properties.arcadeScores.Values, status.ArcadeScores.Values);
    }

    [TestMethod]
    public void StatusTest()
    {
        var status = new Status();

        Assert.AreEqual(string.Empty, status.LastName);
        Assert.AreEqual(0, status.ArcadeScores.Count);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var status = TestUtils.Create<Status>(MakeByteArray(properties));

        Validate(properties, status);
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var properties = ValidProperties;
        properties.encodedLastName =
            properties.encodedLastName.Take(properties.encodedLastName.Length - 1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<Status>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var properties = ValidProperties;
        properties.encodedLastName = [.. properties.encodedLastName, .. new byte[] { 1 }];

        var status = TestUtils.Create<Status>(MakeByteArray(properties));

        Assert.AreEqual("Player1 ", status.LastName);
        CollectionAssert.That.AreNotEqual(properties.arcadeScores.Values, status.ArcadeScores.Values);
    }

    [TestMethod]
    public void ReadFromTestShortenedArcadeScores()
    {
        var properties = new Properties(ValidProperties);
        var scores = properties.arcadeScores
            .Where(pair => pair.Key != (CharaWithReserved.Meiling, CharaWithReserved.Meiling)).ToDictionary();
        properties.arcadeScores = scores;

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<Status>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestExceededArcadeScores()
    {
        var properties = new Properties(ValidProperties);
        var scores = properties.arcadeScores.ToDictionary();
        scores.Add((CharaWithReserved.Reserved15, (CharaWithReserved)99), 99);
        properties.arcadeScores = scores;

        var status = TestUtils.Create<Status>(MakeByteArray(properties));

        Validate(ValidProperties, status);
    }
}
