//-----------------------------------------------------------------------
// <copyright file="ThConverterFactory.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using ThScoreFileConverter.Properties;

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
        { nameof(Resources.TH06),  typeof(Th06Converter)  },
        { nameof(Resources.TH07),  typeof(Th07Converter)  },
        { nameof(Resources.TH08),  typeof(Th08Converter)  },
        { nameof(Resources.TH09),  typeof(Th09Converter)  },
        { nameof(Resources.TH095), typeof(Th095Converter) },
        { nameof(Resources.TH10),  typeof(Th10Converter)  },
        { nameof(Resources.TH11),  typeof(Th11Converter)  },
        { nameof(Resources.TH12),  typeof(Th12Converter)  },
        { nameof(Resources.TH125), typeof(Th125Converter) },
        { nameof(Resources.TH128), typeof(Th128Converter) },
        { nameof(Resources.TH13),  typeof(Th13Converter)  },
        { nameof(Resources.TH14),  typeof(Th14Converter)  },
        { nameof(Resources.TH143), typeof(Th143Converter) },
        { nameof(Resources.TH15),  typeof(Th15Converter)  },
        { nameof(Resources.TH16),  typeof(Th16Converter)  },
        { nameof(Resources.TH165), typeof(Th165Converter) },
        { nameof(Resources.TH17),  typeof(Th17Converter)  },
        { nameof(Resources.TH18),  typeof(Th18Converter)  },
        { nameof(Resources.TH075), typeof(Th075Converter) },
        { nameof(Resources.TH105), typeof(Th105Converter) },
        { nameof(Resources.TH123), typeof(Th123Converter) },
        { nameof(Resources.TH135), typeof(Th135Converter) },
        { nameof(Resources.TH145), typeof(Th145Converter) },
        { nameof(Resources.TH155), typeof(Th155Converter) },
        { nameof(Resources.TH175), typeof(Th175Converter) },
    };

    /// <summary>
    /// Gets whether a new instance of the subclass of <see cref="ThConverter"/> can be create.
    /// </summary>
    /// <param name="key">The string to specify the subclass.</param>
    /// <returns><c>true</c> if the instance can be create, otherwise <c>false</c>.</returns>
    public static bool CanCreate(string key)
    {
        return ConverterTypes.ContainsKey(key);
    }

    /// <summary>
    /// Creates a new instance of the subclass of the <see cref="ThConverter"/> class.
    /// </summary>
    /// <param name="key">The string to specify the subclass.</param>
    /// <returns>An instance of the subclass specified by <paramref name="key"/>, otherwise <c>null</c>.</returns>
    public static ThConverter? Create(string key)
    {
        return ConverterTypes.TryGetValue(key, out var type) ? Activator.CreateInstance(type) as ThConverter : null;
    }
}
