//-----------------------------------------------------------------------
// <copyright file="CommonDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using DependencyPropertyGenerator;
using Microsoft.Xaml.Behaviors;

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Encapsulates the handling of <see cref="CommonDialog"/>.
/// </summary>
[DependencyProperty<ICommand>("OkCommand")]
[DependencyProperty<ICommand>("CancelCommand")]
[DependencyProperty<Window>("Owner")]
[DependencyProperty<ISite>(nameof(CommonDialog.Site))]
[DependencyProperty<object>(nameof(CommonDialog.Tag))]
public abstract partial class CommonDialogAction : TriggerAction<UIElement>
{
}
