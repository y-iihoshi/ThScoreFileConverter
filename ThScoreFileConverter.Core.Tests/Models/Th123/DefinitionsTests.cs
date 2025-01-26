using ThScoreFileConverter.Core.Models.Th123;

namespace ThScoreFileConverter.Core.Tests.Models.Th123;

[TestClass]
public class DefinitionsTests
{
    [TestMethod]
    public void HasStoryTestValidChara()
    {
        Definitions.HasStory(Chara.Reimu).ShouldBeFalse();
        Definitions.HasStory(Chara.Marisa).ShouldBeFalse();
        Definitions.HasStory(Chara.Sakuya).ShouldBeFalse();
        Definitions.HasStory(Chara.Alice).ShouldBeFalse();
        Definitions.HasStory(Chara.Patchouli).ShouldBeFalse();
        Definitions.HasStory(Chara.Youmu).ShouldBeFalse();
        Definitions.HasStory(Chara.Remilia).ShouldBeFalse();
        Definitions.HasStory(Chara.Yuyuko).ShouldBeFalse();
        Definitions.HasStory(Chara.Yukari).ShouldBeFalse();
        Definitions.HasStory(Chara.Suika).ShouldBeFalse();
        Definitions.HasStory(Chara.Reisen).ShouldBeFalse();
        Definitions.HasStory(Chara.Aya).ShouldBeFalse();
        Definitions.HasStory(Chara.Komachi).ShouldBeFalse();
        Definitions.HasStory(Chara.Iku).ShouldBeFalse();
        Definitions.HasStory(Chara.Tenshi).ShouldBeFalse();
        Definitions.HasStory(Chara.Sanae).ShouldBeTrue();
        Definitions.HasStory(Chara.Cirno).ShouldBeTrue();
        Definitions.HasStory(Chara.Meiling).ShouldBeTrue();
        Definitions.HasStory(Chara.Utsuho).ShouldBeFalse();
        Definitions.HasStory(Chara.Suwako).ShouldBeFalse();
        Definitions.HasStory(Chara.Catfish).ShouldBeFalse();
    }

    public static IEnumerable<object[]> InvalidCharacters => TestHelper.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void CanPracticeTestInvalidChara(int chara)
    {
        Definitions.HasStory((Chara)chara).ShouldBeFalse();
    }
}
