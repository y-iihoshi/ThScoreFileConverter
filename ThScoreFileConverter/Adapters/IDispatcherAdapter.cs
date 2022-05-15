//-----------------------------------------------------------------------
// <copyright file="IDispatcherAdapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Windows.Threading;

namespace ThScoreFileConverter.Adapters;

/// <summary>
/// Defines the interface of a wrapper of <see cref="Dispatcher"/>.
/// </summary>
internal interface IDispatcherAdapter
{
    /// <summary>
    /// Executes the specified <see cref="Action"/> synchronously on the thread the underlying
    /// <see cref="Dispatcher"/> is associated with.
    /// </summary>
    /// <param name="callback">A delegate to invoke through the dispatcher.</param>
    public void Invoke(Action callback);
}
