using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Interactivity
{
    internal class DerivedAction : CommonDialogAction
    {
        protected override void Invoke(object parameter) => throw new NotImplementedException();
    }

    internal class Site : ISite
    {
        public IComponent Component => throw new NotImplementedException();

        public IContainer Container => throw new NotImplementedException();

        public bool DesignMode => throw new NotImplementedException();

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object GetService(Type serviceType) => throw new NotImplementedException();
    }

    [TestClass]
    public class CommonDialogActionTests
    {
        [TestMethod]
        public void OkCommandTest()
        {
            var action = new DerivedAction();
            Assert.IsNull(action.OkCommand);

            var command = ApplicationCommands.NotACommand;
            action.OkCommand = command;
            Assert.AreSame(command, action.OkCommand);
        }

        [TestMethod]
        public void CancelCommandTest()
        {
            var action = new DerivedAction();
            Assert.IsNull(action.CancelCommand);

            var command = ApplicationCommands.NotACommand;
            action.CancelCommand = command;
            Assert.AreSame(command, action.CancelCommand);
        }

        [STATestMethod]
        public void OwnerTest()
        {
            var action = new DerivedAction();
            Assert.IsNull(action.Owner);

            var window = new Window();
            action.Owner = window;
            Assert.AreSame(window, action.Owner);
        }

        [TestMethod]
        public void SiteTest()
        {
            var action = new DerivedAction();
            Assert.IsNull(action.Site);

            var site = new Site();
            action.Site = site;
            Assert.AreSame(site, action.Site);
        }

        [TestMethod]
        public void TagTest()
        {
            var action = new DerivedAction();
            Assert.IsNull(action.Tag);

            var tag = new object();
            action.Tag = tag;
            Assert.AreSame(tag, action.Tag);
        }
    }
}
