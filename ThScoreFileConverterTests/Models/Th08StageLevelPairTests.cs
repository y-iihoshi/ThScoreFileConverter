using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th08StageLevelPairTests
    {
        [TestMethod()]
        public void Th08StageLevelPairTest()
        {
            try
            {
                var stage = Th08Converter.Stage.St6B;
                var level = ThConverter.Level.Lunatic;
                var pair = new Th08StageLevelPairWrapper(stage, level);

                Assert.AreEqual(stage, pair.Stage.Value);
                Assert.AreEqual(level, pair.Level.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
