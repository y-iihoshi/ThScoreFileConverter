//-----------------------------------------------------------------------
// <copyright file="CommonItemDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using DependencyPropertyGenerator;
using Microsoft.Xaml.Behaviors;

#if NET8_0_OR_GREATER
using Microsoft.Win32;
#else
using System.Windows.Forms;
#endif

namespace ThScoreFileConverter.Interactivity;

#if NET8_0_OR_GREATER
/// <summary>
/// Encapsulates the handling of <see cref="CommonItemDialog"/>.
/// </summary>
[DependencyProperty<bool>(nameof(CommonItemDialog.AddToRecent))]
[DependencyProperty<Guid?>(nameof(CommonItemDialog.ClientGuid))]
[DependencyProperty<IList<FileDialogCustomPlace>>(nameof(CommonItemDialog.CustomPlaces))]
[DependencyProperty<string>(nameof(CommonItemDialog.DefaultDirectory))]
[DependencyProperty<bool>(nameof(CommonItemDialog.DereferenceLinks))]
[DependencyProperty<string>(nameof(CommonItemDialog.InitialDirectory))]
[DependencyProperty<string>(nameof(CommonItemDialog.RootDirectory))]
[DependencyProperty<bool>(nameof(CommonItemDialog.ShowHiddenItems))]
[DependencyProperty<object>(nameof(CommonItemDialog.Tag))]
[DependencyProperty<string>(nameof(CommonItemDialog.Title))]
[DependencyProperty<bool>(nameof(CommonItemDialog.ValidateNames))]
#else
/// <summary>
/// Encapsulates the handling of <see cref="CommonDialog"/>.
/// </summary>
[DependencyProperty<bool>("AddToRecent")]
[DependencyProperty<Guid?>("ClientGuid")]
[DependencyProperty<IList<FileDialogCustomPlace>>("CustomPlaces")]
[DependencyProperty<string>("DefaultDirectory")]
[DependencyProperty<bool>("DereferenceLinks")]
[DependencyProperty<string>("InitialDirectory")]
[DependencyProperty<string>("RootDirectory")]
[DependencyProperty<bool>("ShowHiddenItems")]
[DependencyProperty<object>(nameof(CommonDialog.Tag))]
[DependencyProperty<string>("Title")]
[DependencyProperty<bool>("ValidateNames")]
#endif
[DependencyProperty<ICommand>("OkCommand")]
[DependencyProperty<ICommand>("CancelCommand")]
[DependencyProperty<Window>("Owner")]
public abstract partial class CommonItemDialogAction : TriggerAction<UIElement>
{
}
