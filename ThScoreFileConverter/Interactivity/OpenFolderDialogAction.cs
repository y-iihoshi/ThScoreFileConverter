//-----------------------------------------------------------------------
// <copyright file="OpenFolderDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using DependencyPropertyGenerator;
using ThScoreFileConverter.Core.Resources;

#if NET8_0_OR_GREATER
using Microsoft.Win32;
#else
using System.Windows.Forms;
#endif

namespace ThScoreFileConverter.Interactivity;

#if NET8_0_OR_GREATER
/// <summary>
/// Encapsulates the handling of <see cref="OpenFolderDialog"/>.
/// </summary>
[DependencyProperty<string>(nameof(OpenFolderDialog.FolderName))]
[DependencyProperty<bool>(nameof(OpenFolderDialog.Multiselect))]
#else
/// <summary>
/// Encapsulates the handling of <see cref="FolderBrowserDialog"/>.
/// </summary>
[DependencyProperty<string>("FolderName")]
[DependencyProperty<bool>("Multiselect")]
#endif
public sealed partial class OpenFolderDialogAction : CommonItemDialogAction
{
#if NET8_0_OR_GREATER
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
#else
    /// <summary>
    /// Creates a new <see cref="FolderBrowserDialog"/> instance.
    /// </summary>
    /// <returns>A created <see cref="FolderBrowserDialog"/> instance.</returns>
    internal FolderBrowserDialog CreateDialog()
    {
        return new FolderBrowserDialog
        {
            Description = this.Title,
            SelectedPath = this.FolderName,
            ShowNewFolderButton = false,
            Tag = this.Tag,
        };
    }
#endif

    /// <inheritdoc/>
    protected override void Invoke(object parameter)
    {
#if NET8_0_OR_GREATER
        var dialog = this.CreateDialog();
        var dialogResult = dialog.ShowDialog(this.Owner);
#else
        using var dialog = this.CreateDialog();
        bool? dialogResult = dialog.ShowDialog(new Win32Window(this.Owner)) switch
        {
            DialogResult.OK => true,
            DialogResult.Cancel => false,
            _ => null,
        };
#endif

        switch (dialogResult)
        {
            case true:
                if (this.OkCommand is not null)
                {
#if NET8_0_OR_GREATER
                    var result = new OpenFolderDialogActionResult(dialog.FolderName);
#else
                    var result = new OpenFolderDialogActionResult(dialog.SelectedPath);
#endif
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
