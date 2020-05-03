using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xaml.Behaviors;
using ThScoreFileConverter.Behaviors;

namespace ThScoreFileConverterTests.Interactivity
{
    internal class Command : ICommand
    {
        private readonly Action action;

        public Command(Action action)
        {
            this.action = action;
        }

#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            Assert.IsTrue(parameter is DragEventArgs);
            this.action.Invoke();
        }
    }

    [TestClass]
    public class UIElementDropBehaviorTests
    {
        private static DragEventArgs? CreateDragEventArgs(DependencyObject target, RoutedEvent routedEvent)
        {
            var types = new[]
            {
                typeof(IDataObject),
                typeof(DragDropKeyStates),
                typeof(DragDropEffects),
                typeof(DependencyObject),
                typeof(Point),
            };
            var parameters = new object[]
            {
                new DataObject(),
                DragDropKeyStates.None,
                DragDropEffects.None,
                target,
                default(Point),
            };

            var constructor = typeof(DragEventArgs)
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
            if (constructor is null)
                return null;

            var dragEventArgs = (DragEventArgs)constructor.Invoke(parameters);
            dragEventArgs.RoutedEvent = routedEvent;

            return dragEventArgs;
        }

        [TestMethod]
        public void DragEnterTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnDragEnter"));
            });
            var behavior = new UIElementDropBehavior
            {
                DragEnterCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.DragEnterEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void DragLeaveTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnDragLeave"));
            });
            var behavior = new UIElementDropBehavior
            {
                DragLeaveCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.DragLeaveEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void DragOverTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnDragOver"));
            });
            var behavior = new UIElementDropBehavior
            {
                DragOverCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.DragOverEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void DropTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnDrop"));
            });
            var behavior = new UIElementDropBehavior
            {
                DropCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.DropEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void PreviewDragEnterTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnPreviewDragEnter"));
            });
            var behavior = new UIElementDropBehavior
            {
                PreviewDragEnterCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.PreviewDragEnterEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void PreviewDragLeaveTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnPreviewDragLeave"));
            });
            var behavior = new UIElementDropBehavior
            {
                PreviewDragLeaveCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.PreviewDragLeaveEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void PreviewDragOverTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnPreviewDragOver"));
            });
            var behavior = new UIElementDropBehavior
            {
                PreviewDragOverCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.PreviewDragOverEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void PreviewDropTest()
        {
            var element = new UIElement();
            var command = new Command(() =>
            {
                Assert.IsTrue(Environment.StackTrace.Contains(
                    $"{typeof(UIElementDropBehavior).FullName}.OnPreviewDrop"));
            });
            var behavior = new UIElementDropBehavior
            {
                PreviewDropCommand = command,
            };
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.PreviewDropEvent));

            _ = behaviors.Remove(behavior);
        }

        [TestMethod]
        public void NullCommandTest()
        {
            var element = new UIElement();
            var behavior = new UIElementDropBehavior();
            var behaviors = Interaction.GetBehaviors(element);

            behaviors.Add(behavior);

            element.RaiseEvent(CreateDragEventArgs(element, UIElement.DragEnterEvent));
            Assert.IsTrue(true);  // Should be reached here.

            _ = behaviors.Remove(behavior);
        }
    }
}
