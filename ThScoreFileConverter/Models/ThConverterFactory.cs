//-----------------------------------------------------------------------
// <copyright file="ThConverterFactory.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
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
            { Resources.keyTh06,  typeof(Th06Converter)  },
            { Resources.keyTh07,  typeof(Th07Converter)  },
            { Resources.keyTh08,  typeof(Th08Converter)  },
            { Resources.keyTh09,  typeof(Th09Converter)  },
            { Resources.keyTh095, typeof(Th095Converter) },
            { Resources.keyTh10,  typeof(Th10Converter)  },
            { Resources.keyTh11,  typeof(Th11Converter)  },
            { Resources.keyTh12,  typeof(Th12Converter)  },
            { Resources.keyTh125, typeof(Th125Converter) },
            { Resources.keyTh128, typeof(Th128Converter) },
            { Resources.keyTh13,  typeof(Th13Converter)  },
            { Resources.keyTh14,  typeof(Th14Converter)  },
            { Resources.keyTh143, typeof(Th143Converter) },
            { Resources.keyTh15,  typeof(Th15Converter)  },
            { Resources.keyTh16,  typeof(Th16Converter)  },
            { Resources.keyTh165, typeof(Th165Converter) },
            { Resources.keyTh17,  typeof(Th17Converter)  },
            { Resources.keyTh075, typeof(Th075Converter) },
            { Resources.keyTh105, typeof(Th105Converter) },
            { Resources.keyTh123, typeof(Th123Converter) },
            { Resources.keyTh135, typeof(Th135Converter) },
            { Resources.keyTh145, typeof(Th145Converter) },
            { Resources.keyTh155, typeof(Th155Converter) },
        };

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
}
