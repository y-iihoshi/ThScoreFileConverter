//-----------------------------------------------------------------------
// <copyright file="UIElementDropBehavior.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using DependencyPropertyGenerator;
using Microsoft.Xaml.Behaviors;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Encapsulates state information and drag-and-drop related <see cref="ICommand"/>s into a
/// <see cref="UIElement"/> object.
/// </summary>
[DependencyProperty<ICommand>("DragEnterCommand")]
[DependencyProperty<ICommand>("DragLeaveCommand")]
[DependencyProperty<ICommand>("DragOverCommand")]
[DependencyProperty<ICommand>("DropCommand")]
[DependencyProperty<ICommand>("PreviewDragEnterCommand")]
[DependencyProperty<ICommand>("PreviewDragLeaveCommand")]
[DependencyProperty<ICommand>("PreviewDragOverCommand")]
[DependencyProperty<ICommand>("PreviewDropCommand")]
public partial class UIElementDropBehavior : Behavior<UIElement>
{
    /// <summary>
    /// Called after the behavior is attached to a <see cref="Behavior{T}.AssociatedObject"/>.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        this.AssociatedObject.DragEnter += this.OnDragEnter;
        this.AssociatedObject.DragLeave += this.OnDragLeave;
        this.AssociatedObject.DragOver += this.OnDragOver;
        this.AssociatedObject.Drop += this.OnDrop;
        this.AssociatedObject.PreviewDragEnter += this.OnPreviewDragEnter;
        this.AssociatedObject.PreviewDragLeave += this.OnPreviewDragLeave;
        this.AssociatedObject.PreviewDragOver += this.OnPreviewDragOver;
        this.AssociatedObject.PreviewDrop += this.OnPreviewDrop;
    }

    /// <summary>
    /// Called when the behavior is being detached from its <see cref="Behavior{T}.AssociatedObject"/>,
    /// but before it has actually occurred.
    /// </summary>
    protected override void OnDetaching()
    {
        this.AssociatedObject.PreviewDrop -= this.OnPreviewDrop;
        this.AssociatedObject.PreviewDragOver -= this.OnPreviewDragOver;
        this.AssociatedObject.PreviewDragLeave -= this.OnPreviewDragLeave;
        this.AssociatedObject.PreviewDragEnter -= this.OnPreviewDragEnter;
        this.AssociatedObject.Drop -= this.OnDrop;
        this.AssociatedObject.DragOver -= this.OnDragOver;
        this.AssociatedObject.DragLeave -= this.OnDragLeave;
        this.AssociatedObject.DragEnter -= this.OnDragEnter;

        base.OnDetaching();
    }

    #region Event handlers

    /// <summary>
    /// Handles drag-and-drop routed events.
    /// </summary>
    /// <param name="command">
    /// The command invoked with the arguments <paramref name="sender"/> and <paramref name="e"/>.
    /// </param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private static void OnDragDropEvent(ICommand? command, object sender, DragEventArgs e)
    {
        if (command is not null)
        {
            Guard.IsTrue(sender is UIElement, nameof(sender), ExceptionMessages.ArgumentExceptionWrongType);
            Guard.IsNotNull(e);

            command.Execute(e);
        }
    }

    /// <summary>
    /// Handles a <see cref="UIElement.DragEnter"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnDragEnter(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.DragEnterCommand, sender, e);
    }

    /// <summary>
    /// Handles a <see cref="UIElement.DragLeave"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnDragLeave(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.DragLeaveCommand, sender, e);
    }

    /// <summary>
    /// Handles a <see cref="UIElement.DragOver"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnDragOver(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.DragOverCommand, sender, e);
    }

    /// <summary>
    /// Handles a <see cref="UIElement.Drop"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnDrop(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.DropCommand, sender, e);
    }

    /// <summary>
    /// Handles a <see cref="UIElement.PreviewDragEnter"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnPreviewDragEnter(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.PreviewDragEnterCommand, sender, e);
    }

    /// <summary>
    /// Handles a <see cref="UIElement.PreviewDragLeave"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnPreviewDragLeave(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.PreviewDragLeaveCommand, sender, e);
    }

    /// <summary>
    /// Handles a <see cref="UIElement.PreviewDragOver"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnPreviewDragOver(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.PreviewDragOverCommand, sender, e);
    }

    /// <summary>
    /// Handles a <see cref="UIElement.PreviewDrop"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnPreviewDrop(object sender, DragEventArgs e)
    {
        OnDragDropEvent(this.PreviewDropCommand, sender, e);
    }

    #endregion
}
