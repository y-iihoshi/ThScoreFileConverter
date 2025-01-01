//-----------------------------------------------------------------------
// <copyright file="OpenFileDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using DependencyPropertyGenerator;
using Microsoft.Win32;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Encapsulates the handling of <see cref="OpenFileDialog"/>.
/// </summary>
[DependencyProperty<bool>(nameof(OpenFileDialog.AddExtension), DefaultValue = true)]
[DependencyProperty<bool>(nameof(OpenFileDialog.CheckFileExists), DefaultValue = true)]
[DependencyProperty<bool>(nameof(OpenFileDialog.CheckPathExists), DefaultValue = true)]
[DependencyProperty<string>(nameof(OpenFileDialog.DefaultExt), DefaultValue = "")]
[DependencyProperty<string>(nameof(OpenFileDialog.FileName), DefaultValue = "")]
[DependencyProperty<string>(nameof(OpenFileDialog.Filter), DefaultValue = "")]
[DependencyProperty<int>(nameof(OpenFileDialog.FilterIndex), DefaultValue = 1)]
[DependencyProperty<bool>(nameof(OpenFileDialog.ForcePreviewPane))]
[DependencyProperty<bool>(nameof(OpenFileDialog.Multiselect), DefaultValue = false)]
[DependencyProperty<bool>(nameof(OpenFileDialog.ReadOnlyChecked), DefaultValue = false)]
[DependencyProperty<bool>(nameof(OpenFileDialog.RestoreDirectory))]
[DependencyProperty<bool>(nameof(OpenFileDialog.ShowReadOnly), DefaultValue = false)]
public partial class OpenFileDialogAction : CommonItemDialogAction
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
            AddToRecent = this.AddToRecent,
            CheckFileExists = this.CheckFileExists,
            CheckPathExists = this.CheckPathExists,
            ClientGuid = this.ClientGuid,
            CustomPlaces = this.CustomPlaces,
            DefaultDirectory = this.DefaultDirectory,
            DefaultExt = this.DefaultExt,
            DereferenceLinks = this.DereferenceLinks,
            FileName = this.FileName,
            Filter = this.Filter,
            FilterIndex = this.FilterIndex,
            ForcePreviewPane = this.ForcePreviewPane,
            InitialDirectory = this.InitialDirectory,
            Multiselect = this.Multiselect,
            ReadOnlyChecked = this.ReadOnlyChecked,
            RestoreDirectory = this.RestoreDirectory,
            RootDirectory = this.RootDirectory,
            ShowHiddenItems = this.ShowHiddenItems,
            ShowReadOnly = this.ShowReadOnly,
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
        var dialog = this.CreateDialog();
        var dialogResult = dialog.ShowDialog(this.Owner);

        switch (dialogResult)
        {
            case true:
                if (this.OkCommand is not null)
                {
                    var result = new OpenFileDialogActionResult(dialog.FileName, dialog.FileNames);
                    if (this.OkCommand.CanExecute(result))
                        this.OkCommand.Execute(result);
                }

                break;

            case false:
                if (this.CancelCommand is not null)
                {
                    if (this.CancelCommand.CanExecute(null))
                        this.CancelCommand.Execute(null);
                }

                break;

            default:
                throw new NotImplementedException(ExceptionMessages.NotImplementedExceptionShouldNotReachHere);
        }
    }
}
