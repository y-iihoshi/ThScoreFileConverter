//-----------------------------------------------------------------------
// <copyright file="EnumHelper{T}.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Frozen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Core.Helpers;

/// <summary>
/// Provides helper functions for <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">An enum type.</typeparam>
public static class EnumHelper<T>
    where T : struct, Enum
{
    private static readonly T[] Values = Enum.GetValues<T>();
    private static readonly Member[] MembersImpl = [.. Enum.GetNames<T>().Select(static name => new Member(name))];

#pragma warning disable CA1000 // Do not declare static members on generic types
    /// <summary>
    /// Gets the <see cref="IEnumerable{T}"/> instance to enumerate values of the <typeparamref name="T"/> type.
    /// </summary>
    public static IEnumerable<T> Enumerable { get; } = Values;

    /// <summary>
    /// Gets the number of values of the <typeparamref name="T"/> type.
    /// </summary>
    public static int NumValues { get; } = Values.Length;

    /// <summary>
    /// Gets the <see cref="Member"/> instances of the fields in the <typeparamref name="T"/> type.
    /// </summary>
    internal static FrozenDictionary<T, Member> Members { get; } =
        MembersImpl.ToFrozenDictionary(static member => member.Value);
#pragma warning restore CA1000 // Do not declare static members on generic types

    /// <summary>
    /// Provides additional information of a field in <typeparamref name="T"/> enum.
    /// Inspired from https://github.com/xin9le/FastEnum.
    /// </summary>
    internal sealed class Member
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Member"/> class.
        /// </summary>
        /// <param name="name">The name of a field in <typeparamref name="T"/>.</param>
        public Member(string name)
        {
            Guard.IsTrue(Enum.IsDefined(typeof(T), name));

            var fieldInfo = typeof(T).GetField(name);
            Guard.IsNotNull(fieldInfo);

            this.Name = name;
            this.Value = Enum.Parse<T>(name);
            this.Pattern = fieldInfo.GetCustomAttribute<PatternAttribute>()?.Pattern ?? string.Empty;
            this.DisplayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
            this.CharacterAttributes = fieldInfo.GetCustomAttributes<CharacterAttribute>()
                .ToFrozenDictionary(static attr => attr.Index);
            this.ShotTypeAttribute = fieldInfo.GetCustomAttribute<ShotTypeAttribute<T>>();
        }

        /// <summary>
        /// Gets the name of the field in <typeparamref name="T"/> that value is <see cref="Value"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value of the field named <see cref="Name"/> in <typeparamref name="T"/>.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the pattern string of the field in <typeparamref name="T"/> if defined;
        /// otherwise <see cref="string.Empty"/>.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Gets the <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> instance
        /// of the field in <typeparamref name="T"/>, if defined; otherwise, <see langword="null"/>.
        /// </summary>
        public DisplayAttribute? DisplayAttribute { get; }

        /// <summary>
        /// Gets the <see cref="CharacterAttribute"/> instances of the field in <typeparamref name="T"/>
        /// as a dictionary keyed by <see cref="CharacterAttribute.Index"/>.
        /// </summary>
        public FrozenDictionary<int, CharacterAttribute> CharacterAttributes { get; }

        /// <summary>
        /// Gets the <see cref="Models.ShotTypeAttribute{T}"/> instance of the field in
        /// <typeparamref name="T"/>, if defined; otherwise, <see langword="null"/>.
        /// </summary>
        public ShotTypeAttribute<T>? ShotTypeAttribute { get; }
    }
}
