//-----------------------------------------------------------------------
// <copyright file="FolderBrowserDialogAction.cs" company="None">
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
/// Encapsulates the handling of <see cref="FolderBrowserDialog"/>.
/// </summary>
[DependencyProperty<string>(nameof(FolderBrowserDialog.Description), DefaultValue = "")]
[DependencyProperty<Environment.SpecialFolder>(nameof(FolderBrowserDialog.RootFolder), DefaultValue = Environment.SpecialFolder.Desktop)]
[DependencyProperty<string>(nameof(FolderBrowserDialog.SelectedPath), DefaultValue = "")]
[DependencyProperty<bool>(nameof(FolderBrowserDialog.ShowNewFolderButton), DefaultValue = true)]
public partial class FolderBrowserDialogAction : CommonDialogAction
{
    /// <summary>
    /// Creates a new <see cref="FolderBrowserDialog"/> instance.
    /// </summary>
    /// <returns>A created <see cref="FolderBrowserDialog"/> instance.</returns>
    internal FolderBrowserDialog CreateDialog()
    {
        return new FolderBrowserDialog
        {
            Description = this.Description ?? string.Empty,
            RootFolder = this.RootFolder,
            SelectedPath = this.SelectedPath ?? string.Empty,
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

#pragma warning disable IDE0010 // Add missing cases to switch statement
        switch (dialogResult)
        {
            case DialogResult.OK:
                if (this.OkCommand is not null)
                {
                    var result = new FolderBrowserDialogActionResult(dialog.SelectedPath);
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
