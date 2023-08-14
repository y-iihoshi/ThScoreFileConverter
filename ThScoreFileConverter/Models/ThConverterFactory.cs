//-----------------------------------------------------------------------
// <copyright file="ThConverterFactory.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Generates an instance that executes the conversion of a score file.
/// </summary>
internal static class ThConverterFactory
{
    /// <summary>
    /// The dictionary of the types of the subclasses of the <see cref="ThConverter"/> class.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, Type> ConverterTypes = new Dictionary<string, Type>
    {
        { nameof(StringResources.TH06),  typeof(Th06.Converter)  },
        { nameof(StringResources.TH07),  typeof(Th07.Converter)  },
        { nameof(StringResources.TH08),  typeof(Th08.Converter)  },
        { nameof(StringResources.TH09),  typeof(Th09.Converter)  },
        { nameof(StringResources.TH095), typeof(Th095.Converter) },
        { nameof(StringResources.TH10),  typeof(Th10.Converter)  },
        { nameof(StringResources.TH11),  typeof(Th11.Converter)  },
        { nameof(StringResources.TH12),  typeof(Th12.Converter)  },
        { nameof(StringResources.TH125), typeof(Th125.Converter) },
        { nameof(StringResources.TH128), typeof(Th128.Converter) },
        { nameof(StringResources.TH13),  typeof(Th13.Converter)  },
        { nameof(StringResources.TH14),  typeof(Th14.Converter)  },
        { nameof(StringResources.TH143), typeof(Th143.Converter) },
        { nameof(StringResources.TH15),  typeof(Th15.Converter)  },
        { nameof(StringResources.TH16),  typeof(Th16.Converter)  },
        { nameof(StringResources.TH165), typeof(Th165.Converter) },
        { nameof(StringResources.TH17),  typeof(Th17.Converter)  },
        { nameof(StringResources.TH18),  typeof(Th18.Converter)  },
        { nameof(StringResources.TH075), typeof(Th075.Converter) },
        { nameof(StringResources.TH105), typeof(Th105.Converter) },
        { nameof(StringResources.TH123), typeof(Th123.Converter) },
        { nameof(StringResources.TH135), typeof(Th135.Converter) },
        { nameof(StringResources.TH145), typeof(Th145.Converter) },
        { nameof(StringResources.TH155), typeof(Th155.Converter) },
        { nameof(StringResources.TH175), typeof(Th175.Converter) },
    };

    /// <summary>
    /// Gets whether a new instance of the subclass of <see cref="ThConverter"/> can be create.
    /// </summary>
    /// <param name="key">The string to specify the subclass.</param>
    /// <returns><see langword="true"/> if the instance can be create, otherwise <see langword="false"/>.</returns>
    public static bool CanCreate(string key)
    {
        return ConverterTypes.ContainsKey(key);
    }

    /// <summary>
    /// Creates a new instance of the subclass of the <see cref="ThConverter"/> class.
    /// </summary>
    /// <param name="key">The string to specify the subclass.</param>
    /// <returns>An instance of the subclass specified by <paramref name="key"/>, otherwise <see langword="null"/>.</returns>
    public static ThConverter? Create(string key)
    {
        return ConverterTypes.TryGetValue(key, out var type) ? Activator.CreateInstance(type) as ThConverter : null;
    }
}
