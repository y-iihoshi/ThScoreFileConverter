﻿//-----------------------------------------------------------------------
// <copyright file="CloseWindowCommand.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

namespace ThScoreFileConverter.Commands
{
    /// <summary>
    /// Represents the command to close the specified window.
    /// </summary>
    public class CloseWindowCommand : ICommand
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="CloseWindowCommand"/> class from being created.
        /// </summary>
        private CloseWindowCommand()
        {
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ICommand Instance { get; } = new CloseWindowCommand();

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">A <see cref="Window"/> instance which will be closed.</param>
        /// <returns><c>true</c> if this command can be executed; otherwise, <c>false</c>.</returns>
#if NET5_0_OR_GREATER
        public bool CanExecute([NotNullWhen(true)] object? parameter)
#else
        public bool CanExecute(object parameter)
#endif
        {
            return parameter is Window;
        }

        /// <summary>
        /// Called when the command is invoked.
        /// </summary>
        /// <param name="parameter">A <see cref="Window"/> instance which is closed.</param>
#if NET5_0_OR_GREATER
        public void Execute(object? parameter)
#else
        public void Execute(object parameter)
#endif
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            if (this.CanExecute(parameter))
                ((Window)parameter).Close();
#pragma warning restore CA1062 // Validate arguments of public methods
        }
    }
}
