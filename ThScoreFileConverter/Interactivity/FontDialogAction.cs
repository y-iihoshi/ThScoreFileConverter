//-----------------------------------------------------------------------
// <copyright file="FontDialogAction.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DependencyPropertyGenerator;
using ThScoreFileConverter.Core.Resources;
using SysDraw = System.Drawing;

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Encapsulates the handling of <see cref="FontDialog"/>.
/// </summary>
[DependencyProperty<ICommand>("ApplyCommand")]
[DependencyProperty<bool>(nameof(FontDialog.AllowScriptChange), DefaultValue = true)]
[DependencyProperty<bool>(nameof(FontDialog.AllowSimulations), DefaultValue = true)]
[DependencyProperty<bool>(nameof(FontDialog.AllowVectorFonts), DefaultValue = true)]
[DependencyProperty<bool>(nameof(FontDialog.AllowVerticalFonts), DefaultValue = true)]
[DependencyProperty<SysDraw.Color>(nameof(FontDialog.Color), DefaultValueExpression = "System.Drawing.Color.Black")]
[DependencyProperty<bool>(nameof(FontDialog.FixedPitchOnly), DefaultValue = false)]
[DependencyProperty<SysDraw.Font>(nameof(FontDialog.Font), DefaultValueExpression = "System.Drawing.SystemFonts.DefaultFont")]
[DependencyProperty<bool>(nameof(FontDialog.FontMustExist), DefaultValue = false)]
[DependencyProperty<int>(nameof(FontDialog.MaxSize), DefaultValue = 0)]
[DependencyProperty<int>(nameof(FontDialog.MinSize), DefaultValue = 0)]
[DependencyProperty<bool>(nameof(FontDialog.ScriptsOnly), DefaultValue = false)]
[DependencyProperty<bool>(nameof(FontDialog.ShowApply), DefaultValue = false)]
[DependencyProperty<bool>(nameof(FontDialog.ShowColor), DefaultValue = false)]
[DependencyProperty<bool>(nameof(FontDialog.ShowEffects), DefaultValue = true)]
[DependencyProperty<bool>(nameof(FontDialog.ShowHelp), DefaultValue = false)]
public partial class FontDialogAction : CommonDialogAction
{
    /// <summary>
    /// Creates a new <see cref="FontDialog"/> instance.
    /// </summary>
    /// <returns>A created <see cref="FontDialog"/> instance.</returns>
    internal FontDialog CreateDialog()
    {
        return new FontDialog
        {
            AllowScriptChange = this.AllowScriptChange,
            AllowSimulations = this.AllowSimulations,
            AllowVectorFonts = this.AllowVectorFonts,
            AllowVerticalFonts = this.AllowVerticalFonts,
            Color = this.Color,
            FixedPitchOnly = this.FixedPitchOnly,
            Font = this.Font ?? SysDraw.SystemFonts.DefaultFont,
            FontMustExist = this.FontMustExist,
            MaxSize = this.MaxSize,
            MinSize = this.MinSize,
            ScriptsOnly = this.ScriptsOnly,
            ShowApply = this.ShowApply,
            ShowColor = this.ShowColor,
            ShowEffects = this.ShowEffects,
            ShowHelp = this.ShowHelp,
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
        static void ExecuteCommand(ICommand command, SysDraw.Font font, SysDraw.Color color)
        {
            var result = new FontDialogActionResult(font, color);
            if (command.CanExecute(result))
                command.Execute(result);
        }

        using var dialog = this.CreateDialog();
        using var disposable = new SingleAssignmentDisposable();

        if (this.ShowApply && (this.ApplyCommand is not null))
        {
            disposable.Disposable = Observable
                .FromEvent<EventHandler, EventArgs>(
                    h => (sender, e) => h(e), h => dialog.Apply += h, h => dialog.Apply -= h)
                .Subscribe(_ => ExecuteCommand(this.ApplyCommand, dialog.Font, dialog.Color));
        }

        var oldFont = dialog.Font;
        var oldColor = dialog.Color;
        var dialogResult = dialog.ShowDialog(new Win32Window(this.Owner));

#pragma warning disable IDE0010 // Add missing cases to switch statement
        switch (dialogResult)
        {
            case DialogResult.OK:
                if (this.OkCommand is not null)
                    ExecuteCommand(this.OkCommand, dialog.Font, dialog.Color);
                break;

            case DialogResult.Cancel:
                if (this.CancelCommand is not null)
                    ExecuteCommand(this.CancelCommand, oldFont, oldColor);
                break;

            default:
                throw new NotImplementedException(ExceptionMessages.NotImplementedExceptionShouldNotReachHere);
        }
#pragma warning restore IDE0010 // Add missing cases to switch statement
    }
}
