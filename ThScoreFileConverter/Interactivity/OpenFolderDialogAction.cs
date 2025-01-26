//-----------------------------------------------------------------------
// <copyright file="OpenFolderDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using DependencyPropertyGenerator;
using Microsoft.Win32;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Encapsulates the handling of <see cref="OpenFolderDialog"/>.
/// </summary>
[DependencyProperty<string>(nameof(OpenFolderDialog.FolderName))]
[DependencyProperty<bool>(nameof(OpenFolderDialog.Multiselect))]
public sealed partial class OpenFolderDialogAction : CommonItemDialogAction
{
    /// <summary>
    /// Creates a new <see cref="OpenFolderDialog"/> instance.
    /// </summary>
    /// <returns>A created <see cref="OpenFolderDialog"/> instance.</returns>
    internal OpenFolderDialog CreateDialog()
    {
        return new OpenFolderDialog()
        {
            AddToRecent = this.AddToRecent,
            ClientGuid = this.ClientGuid,
            CustomPlaces = this.CustomPlaces,
            DefaultDirectory = this.DefaultDirectory,
            DereferenceLinks = this.DereferenceLinks,
            FolderName = this.FolderName,
            InitialDirectory = this.InitialDirectory,
            Multiselect = this.Multiselect,
            RootDirectory = this.RootDirectory,
            ShowHiddenItems = this.ShowHiddenItems,
            Tag = this.Tag,
            Title = this.Title,
            ValidateNames = this.ValidateNames,
        };
    }

    /// <inheritdoc/>
    protected override void Invoke(object parameter)
    {
        var dialog = this.CreateDialog();
        var dialogResult = dialog.ShowDialog(this.Owner);

        switch (dialogResult)
        {
            case true:
                if (this.OkCommand is not null)
                {
                    var result = new OpenFolderDialogActionResult(dialog.FolderName);
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
