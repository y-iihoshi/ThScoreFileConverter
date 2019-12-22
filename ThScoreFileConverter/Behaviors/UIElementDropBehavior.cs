//-----------------------------------------------------------------------
// <copyright file="UIElementDropBehavior.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Behaviors
{
    /// <summary>
    /// Encapsulates state information and drag-and-drop related <see cref="ICommand"/>s into a
    /// <see cref="UIElement"/> object.
    /// </summary>
    public class UIElementDropBehavior : Behavior<UIElement>
    {
        #region Dependency properties

        /// <summary>Identifies the <see cref="DragEnterCommand"/> dependency property.</summary>
        public static readonly DependencyProperty DragEnterCommandProperty =
            DependencyProperty.Register(
                nameof(DragEnterCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        /// <summary>Identifies the <see cref="DragLeaveCommand"/> dependency property.</summary>
        public static readonly DependencyProperty DragLeaveCommandProperty =
            DependencyProperty.Register(
                nameof(DragLeaveCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        /// <summary>Identifies the <see cref="DragOverCommand"/> dependency property.</summary>
        public static readonly DependencyProperty DragOverCommandProperty =
            DependencyProperty.Register(
                nameof(DragOverCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        /// <summary>Identifies the <see cref="DropCommand"/> dependency property.</summary>
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register(
                nameof(DropCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        /// <summary>Identifies the <see cref="PreviewDragEnterCommand"/> dependency property.</summary>
        public static readonly DependencyProperty PreviewDragEnterCommandProperty =
            DependencyProperty.Register(
                nameof(PreviewDragEnterCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        /// <summary>Identifies the <see cref="PreviewDragLeaveCommand"/> dependency property.</summary>
        public static readonly DependencyProperty PreviewDragLeaveCommandProperty =
            DependencyProperty.Register(
                nameof(PreviewDragLeaveCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        /// <summary>Identifies the <see cref="PreviewDragOverCommand"/> dependency property.</summary>
        public static readonly DependencyProperty PreviewDragOverCommandProperty =
            DependencyProperty.Register(
                nameof(PreviewDragOverCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        /// <summary>Identifies the <see cref="PreviewDropCommand"/> dependency property.</summary>
        public static readonly DependencyProperty PreviewDropCommandProperty =
            DependencyProperty.Register(
                nameof(PreviewDropCommand), typeof(ICommand), typeof(UIElementDropBehavior));

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.DragEnter"/> event is occurred.
        /// </summary>
        public ICommand DragEnterCommand
        {
            get => this.GetValue(DragEnterCommandProperty) as ICommand;
            set => this.SetValue(DragEnterCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.DragLeave"/> event is occurred.
        /// </summary>
        public ICommand DragLeaveCommand
        {
            get => this.GetValue(DragLeaveCommandProperty) as ICommand;
            set => this.SetValue(DragLeaveCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.DragOver"/> event is occurred.
        /// </summary>
        public ICommand DragOverCommand
        {
            get => this.GetValue(DragOverCommandProperty) as ICommand;
            set => this.SetValue(DragOverCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.Drop"/> event is occurred.
        /// </summary>
        public ICommand DropCommand
        {
            get => this.GetValue(DropCommandProperty) as ICommand;
            set => this.SetValue(DropCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.PreviewDragEnter"/> event is
        /// occurred.
        /// </summary>
        public ICommand PreviewDragEnterCommand
        {
            get => this.GetValue(PreviewDragEnterCommandProperty) as ICommand;
            set => this.SetValue(PreviewDragEnterCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.PreviewDragLeave"/> event is
        /// occurred.
        /// </summary>
        public ICommand PreviewDragLeaveCommand
        {
            get => this.GetValue(PreviewDragLeaveCommandProperty) as ICommand;
            set => this.SetValue(PreviewDragLeaveCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.PreviewDragOver"/> event is
        /// occurred.
        /// </summary>
        public ICommand PreviewDragOverCommand
        {
            get => this.GetValue(PreviewDragOverCommandProperty) as ICommand;
            set => this.SetValue(PreviewDragOverCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command invoked when the <see cref="UIElement.PreviewDrop"/> event is occurred.
        /// </summary>
        public ICommand PreviewDropCommand
        {
            get => this.GetValue(PreviewDropCommandProperty) as ICommand;
            set => this.SetValue(PreviewDropCommandProperty, value);
        }

        #endregion

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
        private static void OnDragDropEvent(ICommand command, object sender, DragEventArgs e)
        {
            if (command != null)
            {
                if (!(sender is UIElement))
                    throw new ArgumentException(Resources.ArgumentExceptionWrongType, nameof(sender));

                if (e == null)
                    throw new ArgumentNullException(nameof(e));

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
}
