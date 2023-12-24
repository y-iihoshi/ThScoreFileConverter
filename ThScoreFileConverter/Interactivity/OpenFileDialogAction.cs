//-----------------------------------------------------------------------
// <copyright file="OpenFileDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Windows.Forms;
using DependencyPropertyGenerator;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Encapsulates the handling of <see cref="OpenFileDialog"/>.
/// </summary>
[DependencyProperty<bool>(nameof(OpenFileDialog.AddExtension), DefaultValue = true)]
[DependencyProperty<bool>(nameof(OpenFileDialog.AutoUpgradeEnabled), DefaultValue = true)]
[DependencyProperty<bool>(nameof(OpenFileDialog.CheckFileExists), DefaultValue = true)]
[DependencyProperty<bool>(nameof(OpenFileDialog.CheckPathExists), DefaultValue = true)]
[DependencyProperty<string>(nameof(OpenFileDialog.DefaultExt), DefaultValue = "")]
[DependencyProperty<bool>(nameof(OpenFileDialog.DereferenceLinks), DefaultValue = true)]
[DependencyProperty<string>(nameof(OpenFileDialog.FileName), DefaultValue = "")]
[DependencyProperty<string>(nameof(OpenFileDialog.Filter), DefaultValue = "")]
[DependencyProperty<int>(nameof(OpenFileDialog.FilterIndex), DefaultValue = 1)]
[DependencyProperty<string>(nameof(OpenFileDialog.InitialDirectory), DefaultValue = "")]
[DependencyProperty<bool>(nameof(OpenFileDialog.Multiselect), DefaultValue = false)]
[DependencyProperty<bool>(nameof(OpenFileDialog.ReadOnlyChecked), DefaultValue = false)]
[DependencyProperty<bool>(nameof(OpenFileDialog.RestoreDirectory), DefaultValue = false)]
[DependencyProperty<bool>(nameof(OpenFileDialog.ShowHelp), DefaultValue = false)]
[DependencyProperty<bool>(nameof(OpenFileDialog.ShowReadOnly), DefaultValue = false)]
[DependencyProperty<bool>(nameof(OpenFileDialog.SupportMultiDottedExtensions), DefaultValue = false)]
[DependencyProperty<string>(nameof(OpenFileDialog.Title), DefaultValue = "")]
[DependencyProperty<bool>(nameof(OpenFileDialog.ValidateNames), DefaultValue = true)]
public partial class OpenFileDialogAction : CommonDialogAction
{
    /// <summary>
    /// Creates a new <see cref="OpenFileDialog"/> instance.
    /// </summary>
    /// <returns>A created <see cref="OpenFileDialog"/> instance.</returns>
    internal OpenFileDialog CreateDialog()
    {
        return new OpenFileDialog
        {
            AddExtension = this.AddExtension,
            AutoUpgradeEnabled = this.AutoUpgradeEnabled,
            CheckFileExists = this.CheckFileExists,
            CheckPathExists = this.CheckPathExists,
            DefaultExt = this.DefaultExt,
            DereferenceLinks = this.DereferenceLinks,
            FileName = this.FileName,
            Filter = this.Filter,
            FilterIndex = this.FilterIndex,
            InitialDirectory = this.InitialDirectory,
            Multiselect = this.Multiselect,
            ReadOnlyChecked = this.ReadOnlyChecked,
            RestoreDirectory = this.RestoreDirectory,
            ShowHelp = this.ShowHelp,
            ShowReadOnly = this.ShowReadOnly,
            Site = this.Site,
            SupportMultiDottedExtensions = this.SupportMultiDottedExtensions,
            Tag = this.Tag,
            Title = this.Title,
            ValidateNames = this.ValidateNames,
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

#pragma warning disable IDE0010 // Add missing cases to switch statement
        switch (dialogResult)
        {
            case DialogResult.OK:
                if (this.OkCommand is not null)
                {
                    var result = new OpenFileDialogActionResult(dialog.FileName, dialog.FileNames);
                    if (this.OkCommand.CanExecute(result))
                        this.OkCommand.Execute(result);
                }

                break;

            case DialogResult.Cancel:
                if (this.CancelCommand is not null)
                {
                    if (this.CancelCommand.CanExecute(null))
                        this.CancelCommand.Execute(null);
                }

                break;

            default:
                throw new NotImplementedException(ExceptionMessages.NotImplementedExceptionShouldNotReachHere);
        }
#pragma warning restore IDE0010 // Add missing cases to switch statement
    }
}
