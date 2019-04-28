using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Commands;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Commands
{
    [TestClass]
    public class CloseWindowCommandTests
    {
        [TestMethod]
        public void InstanceTest()
        {
            var instance = CloseWindowCommand.Instance;
            Assert.IsTrue(instance is ICommand);
        }

        [TestMethod]
        public void CanExecuteTest()
        {
            var instance = CloseWindowCommand.Instance;
            var window = new Window();
            Assert.IsTrue(instance.CanExecute(window));
        }

        [TestMethod]
        public void CanExecuteTestNull()
        {
            var instance = CloseWindowCommand.Instance;
            Assert.IsFalse(instance.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteTestInvalid()
        {
            var instance = CloseWindowCommand.Instance;
            Assert.IsFalse(instance.CanExecute(5));
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var instance = CloseWindowCommand.Instance;
            instance.Execute(new Window());
        }

        [TestMethod]
        public void ExecuteTestNull()
        {
            var instance = CloseWindowCommand.Instance;
            instance.Execute(null);
        }

        [TestMethod]
        public void ExecuteTestInvalid()
        {
            var instance = CloseWindowCommand.Instance;
            instance.Execute(5);
        }

        [TestMethod]
        public void CanExecuteChangedTest()
        {
            var instance = CloseWindowCommand.Instance;
            instance.CanExecuteChanged += (sender, e) => Assert.Fail(TestUtils.Unreachable);

            instance.Execute(null);
            instance.Execute(new Window());
            instance.Execute(5);
        }
    }
}
