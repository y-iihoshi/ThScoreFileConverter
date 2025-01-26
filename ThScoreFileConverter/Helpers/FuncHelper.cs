//-----------------------------------------------------------------------
// <copyright file="FuncHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Provides helper functions for <see cref="Func{T, TResult}"/>.
/// </summary>
public static class FuncHelper
{
    /// <summary>
    /// Always returns <see langword="true"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="arg">An argument. Not used.</param>
    /// <returns>Always <see langword="true"/>.</returns>
    public static bool True<T>(T arg)
    {
        _ = arg;
        return true;
    }

    /// <summary>
    /// Makes a logical-and predicate by one or more predicates.
    /// </summary>
    /// <typeparam name="T">The type of the instance to evaluate.</typeparam>
    /// <param name="predicates">The predicates combined with logical-and operators.</param>
    /// <returns>A logical-and predicate.</returns>
    public static Func<T, bool> MakeAndPredicate<T>(params Func<T, bool>[] predicates)
    {
        return arg => predicates.All(pred => pred(arg));
    }
}
