using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class StageInfoTests
{
    internal struct Properties<TChara>
        where TChara : struct, Enum
    {
        public Stage stage;
        public TChara enemy;
        public IEnumerable<int> cardIds;
    }

    internal static Properties<TChara> MakeValidProperties<TChara>()
        where TChara : struct, Enum
    {
        return new()
        {
            stage = Stage.Two,
            enemy = TestUtils.Cast<TChara>(2),
            cardIds = [3, 4, 5],
        };
    }

    internal static void Validate<TChara>(in Properties<TChara> expected, in StageInfo<TChara> actual)
        where TChara : struct, Enum
    {
        actual.Stage.ShouldBe(expected.stage);
        actual.Enemy.ShouldBe(expected.enemy);
        actual.CardIds.ShouldBe(expected.cardIds);
    }

    internal static void StageInfoTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var properties = MakeValidProperties<TChara>();

        var spellCardInfo = new StageInfo<TChara>(properties.stage, properties.enemy, properties.cardIds);

        Validate(properties, spellCardInfo);
    }

    [TestMethod]
    public void StageInfoTest()
    {
        StageInfoTestHelper<Chara>();
    }
}
