//-----------------------------------------------------------------------
// <copyright file="TextBoxBaseScrollBehavior.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

namespace ThScoreFileConverter.Behaviors
{
    /// <summary>
    /// Encapsulates state information into a <see cref="TextBoxBase"/> object.
    /// </summary>
    public class TextBoxBaseScrollBehavior : Behavior<TextBoxBase>
    {
        /// <summary>Identifies the <see cref="AutoScrollToEnd"/> dependency property.</summary>
        public static readonly DependencyProperty AutoScrollToEndProperty =
            DependencyProperty.Register(
                nameof(AutoScrollToEnd), typeof(bool), typeof(TextBoxBaseScrollBehavior));

        /// <summary>
        /// Gets or sets a value indicating whether it scrolls to end automatically.
        /// </summary>
        public bool AutoScrollToEnd
        {
            get => (bool)this.GetValue(AutoScrollToEndProperty);
            set => this.SetValue(AutoScrollToEndProperty, value);
        }

        /// <summary>
        /// Called after the behavior is attached to a <see cref="Behavior{T}.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.TargetUpdated += this.OnTargetUpdated;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior{T}.AssociatedObject"/>,
        /// but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            this.AssociatedObject.TargetUpdated -= this.OnTargetUpdated;

            base.OnDetaching();
        }

        /// <summary>
        /// Handles a <see cref="FrameworkElement.TargetUpdated"/> event.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnTargetUpdated(object? sender, DataTransferEventArgs e)
        {
            if (this.AutoScrollToEnd)
                this.AssociatedObject.ScrollToEnd();
        }
    }
}
