//-----------------------------------------------------------------------
// <copyright file="FolderBrowserDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Windows;
using ThScoreFileConverter.Properties;
using WinForms = System.Windows.Forms;

namespace ThScoreFileConverter.Interactivity
{
    /// <summary>
    /// Encapsulates the handling of <see cref="WinForms.FolderBrowserDialog"/>.
    /// </summary>
    public class FolderBrowserDialogAction : CommonDialogAction
    {
        #region Dependency properties

        /// <summary>Identifies the <see cref="Description"/> dependency property.</summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
                nameof(Description),
                typeof(string),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(string.Empty));

        /// <summary>Identifies the <see cref="RootFolder"/> dependency property.</summary>
        public static readonly DependencyProperty RootFolderProperty =
            DependencyProperty.Register(
                nameof(RootFolder),
                typeof(Environment.SpecialFolder),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(Environment.SpecialFolder.Desktop));

        /// <summary>Identifies the <see cref="SelectedPath"/> dependency property.</summary>
        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register(
                nameof(SelectedPath),
                typeof(string),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(string.Empty));

        /// <summary>Identifies the <see cref="ShowNewFolderButton"/> dependency property.</summary>
        public static readonly DependencyProperty ShowNewFolderButtonProperty =
            DependencyProperty.Register(
                nameof(ShowNewFolderButton),
                typeof(bool),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(true));

        #endregion

        #region CLR properties

        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        public string Description
        {
            get => (string)this.GetValue(DescriptionProperty);
            set => this.SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// Gets or sets the root folder where the browsing starts from.
        /// </summary>
        public Environment.SpecialFolder RootFolder
        {
            get => (Environment.SpecialFolder)this.GetValue(RootFolderProperty);
            set => this.SetValue(RootFolderProperty, value);
        }

        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        public string SelectedPath
        {
            get => (string)this.GetValue(SelectedPathProperty);
            set => this.SetValue(SelectedPathProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <c>New Folder</c> button appears in the folder
        /// browsing dialog box.
        /// </summary>
        public bool ShowNewFolderButton
        {
            get => (bool)this.GetValue(ShowNewFolderButtonProperty);
            set => this.SetValue(ShowNewFolderButtonProperty, value);
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="WinForms.FolderBrowserDialog"/> instance.
        /// </summary>
        /// <returns>A created <see cref="WinForms.FolderBrowserDialog"/> instance.</returns>
        internal WinForms.FolderBrowserDialog CreateDialog()
        {
            return new WinForms.FolderBrowserDialog
            {
                Description = this.Description,
                RootFolder = this.RootFolder,
                SelectedPath = this.SelectedPath,
                ShowNewFolderButton = this.ShowNewFolderButton,
                Site = this.Site,
                Tag = this.Tag,
            };
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action; but not used.</param>
        protected override void Invoke(object parameter)
        {
            using var dialog = this.CreateDialog();
            var dialogResult = dialog.ShowDialog(new Win32Window(this.Owner));

            switch (dialogResult)
            {
                case WinForms.DialogResult.OK:
                    if (this.OkCommand is { })
                    {
                        var result = new FolderBrowserDialogActionResult(dialog.SelectedPath);
                        if (this.OkCommand.CanExecute(result))
                            this.OkCommand.Execute(result);
                    }

                    break;

                case WinForms.DialogResult.Cancel:
                    if (this.CancelCommand is { })
                    {
                        if (this.CancelCommand.CanExecute(null))
                            this.CancelCommand.Execute(null);
                    }

                    break;

                default:
                    throw new NotImplementedException(Resources.NotImplementedExceptionShouldNotReachHere);
            }
        }
    }
}
