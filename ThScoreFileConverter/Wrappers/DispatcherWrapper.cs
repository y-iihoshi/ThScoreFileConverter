//-----------------------------------------------------------------------
// <copyright file="DispatcherWrapper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Threading;

namespace ThScoreFileConverter.Wrappers
{
    /// <summary>
    /// Wrapper of <see cref="Dispatcher"/>.
    /// </summary>
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    internal class DispatcherWrapper : IDispatcherWrapper
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherWrapper"/> class.
        /// </summary>
        /// <param name="dispatcher">
        /// <see cref="Dispatcher"/> to be wrapped;
        /// if <c>null</c>, <see cref="Application.Current"/>.Dispatcher is used.
        /// </param>
        public DispatcherWrapper(Dispatcher? dispatcher = null)
        {
            this.dispatcher = dispatcher ?? Application.Current.Dispatcher;
        }

        /// <inheritdoc/>
        public void Invoke(Action callback)
        {
            this.dispatcher.Invoke(callback);
        }
    }
}
