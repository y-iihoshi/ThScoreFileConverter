﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

internal sealed class Logger : INotifyPropertyChanged
{
    private string log = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Log
    {
        get => this.log;
        set
        {
            this.log = value;
            this.RaisePropertyChanged();
        }
    }

    private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

[TestClass]
public class TextBoxBaseScrollBehaviorTests
{
    [STATestMethod]
    public void AutoScrollToEndTest()
    {
        var logger = new Logger();
        var binding = new Binding(nameof(logger.Log))
        {
            NotifyOnTargetUpdated = true,
            Source = logger,
        };
        var textbox = new TextBox();
        var window = new Window
        {
            Content = textbox,
        };
        var behavior = new TextBoxBaseScrollBehavior
        {
            AutoScrollToEnd = true,
        };
        var behaviors = Interaction.GetBehaviors(textbox);

        static void onLayoutUpdated(object? sender, EventArgs eventArgs)
        {
            Environment.StackTrace.ShouldContain($"{typeof(TextBoxBaseScrollBehavior).FullName}.OnTargetUpdated");
            Environment.StackTrace.ShouldContain($"{typeof(TextBoxBase).FullName}.{nameof(TextBoxBase.ScrollToEnd)}");
        }

        _ = BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);
        behaviors.Add(behavior);
        textbox.LayoutUpdated += onLayoutUpdated;

        logger.Log = "abc";

        textbox.LayoutUpdated -= onLayoutUpdated;
        _ = behaviors.Remove(behavior);
        BindingOperations.ClearBinding(textbox, TextBox.TextProperty);
    }

    [STATestMethod]
    public void NotAutoScrollToEndTest()
    {
        var logger = new Logger();
        var binding = new Binding(nameof(logger.Log))
        {
            NotifyOnTargetUpdated = true,
            Source = logger,
        };
        var textbox = new TextBox();
        var window = new Window
        {
            Content = textbox,
        };
        var behavior = new TextBoxBaseScrollBehavior
        {
            AutoScrollToEnd = false,
        };
        var behaviors = Interaction.GetBehaviors(textbox);

        static void onLayoutUpdated(object? sender, EventArgs eventArgs)
        {
            TestHelper.ShouldNotReachHere();
        }

        _ = BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);
        behaviors.Add(behavior);
        textbox.LayoutUpdated += onLayoutUpdated;

        logger.Log = "abc";

        textbox.LayoutUpdated -= onLayoutUpdated;
        _ = behaviors.Remove(behavior);
        BindingOperations.ClearBinding(textbox, TextBox.TextProperty);
    }
}
