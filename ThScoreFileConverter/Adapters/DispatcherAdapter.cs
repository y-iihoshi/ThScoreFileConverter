//-----------------------------------------------------------------------
// <copyright file="DispatcherAdapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Windows;
using System.Windows.Threading;

namespace ThScoreFileConverter.Adapters;

/// <summary>
/// Wrapper of <see cref="Dispatcher"/>.
/// </summary>
/// <param name="dispatcher">
/// <see cref="Dispatcher"/> to be wrapped;
/// if <see langword="null"/>, <see cref="Application.Current"/>.Dispatcher is used.
/// </param>
#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
internal sealed class DispatcherAdapter(Dispatcher? dispatcher = null) : IDispatcherAdapter
{
    private readonly Dispatcher dispatcher = dispatcher ?? Application.Current.Dispatcher;

    /// <inheritdoc/>
    public void Invoke(Action callback)
    {
        this.dispatcher.Invoke(callback);
    }
}
