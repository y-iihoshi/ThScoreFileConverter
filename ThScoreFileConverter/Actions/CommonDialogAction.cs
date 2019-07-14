//-----------------------------------------------------------------------
// <copyright file="CommonDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Actions
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Encapsulates the handling of <see cref="System.Windows.Forms.CommonDialog"/>.
    /// </summary>
    public abstract class CommonDialogAction : TriggerAction<UIElement>
    {
        #region Dependency properties

        /// <summary>Identifies the <see cref="OkCommand"/> dependency property.</summary>
        public static readonly DependencyProperty OkCommandProperty = DependencyProperty.Register(
            nameof(OkCommand), typeof(ICommand), typeof(CommonDialogAction), new UIPropertyMetadata(null));

        /// <summary>Identifies the <see cref="CancelCommand"/> dependency property.</summary>
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
            nameof(CancelCommand), typeof(ICommand), typeof(CommonDialogAction), new UIPropertyMetadata(null));

        /// <summary>Identifies the <see cref="Owner"/> dependency property.</summary>
        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
            nameof(Owner), typeof(Window), typeof(CommonDialogAction), new UIPropertyMetadata(null));

        /// <summary>Identifies the <see cref="Site"/> dependency property.</summary>
        public static readonly DependencyProperty SiteProperty = DependencyProperty.Register(
            nameof(Site), typeof(ISite), typeof(CommonDialogAction), new UIPropertyMetadata(null));

        /// <summary>Identifies the <see cref="Tag"/> dependency property.</summary>
        public static readonly DependencyProperty TagProperty = DependencyProperty.Register(
            nameof(Tag), typeof(object), typeof(CommonDialogAction), new UIPropertyMetadata(null));

        #endregion

        #region CLR properties

        /// <summary>
        /// Gets or sets the command invoked when the user clicks the <c>OK</c> button in the dialog box.
        /// </summary>
        public ICommand OkCommand
        {
            get { return this.GetValue(OkCommandProperty) as ICommand; }
            set { this.SetValue(OkCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command invoked when the user clicks the <c>Cancel</c> button in the dialog box.
        /// </summary>
        public ICommand CancelCommand
        {
            get { return this.GetValue(CancelCommandProperty) as ICommand; }
            set { this.SetValue(CancelCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the window which owns the font dialog box.
        /// </summary>
        public Window Owner
        {
            get { return this.GetValue(OwnerProperty) as Window; }
            set { this.SetValue(OwnerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> of the <see cref="Component"/>.
        /// </summary>
        public ISite Site
        {
            get { return this.GetValue(SiteProperty) as ISite; }
            set { this.SetValue(SiteProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object that contains data about the control.
        /// </summary>
        public object Tag
        {
            get { return this.GetValue(TagProperty); }
            set { this.SetValue(TagProperty, value); }
        }

        #endregion
    }
}
