//-----------------------------------------------------------------------
// <copyright file="ThConverterFactory.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

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
            { "TH06",  typeof(Th06Converter)  },
            { "TH07",  typeof(Th07Converter)  },
            { "TH08",  typeof(Th08Converter)  },
            { "TH09",  typeof(Th09Converter)  },
            { "TH095", typeof(Th095Converter) },
            { "TH10",  typeof(Th10Converter)  },
            { "TH11",  typeof(Th11Converter)  },
            { "TH12",  typeof(Th12Converter)  },
            { "TH125", typeof(Th125Converter) },
            { "TH128", typeof(Th128Converter) },
            { "TH13",  typeof(Th13Converter)  },
            { "TH14",  typeof(Th14Converter)  },
            { "TH143", typeof(Th143Converter) },
            { "TH15",  typeof(Th15Converter)  },
            { "TH16",  typeof(Th16Converter)  },
            { "TH165", typeof(Th165Converter) },
            { "TH17",  typeof(Th17Converter)  },
            { "TH075", typeof(Th075Converter) },
            { "TH105", typeof(Th105Converter) },
            { "TH123", typeof(Th123Converter) },
            { "TH135", typeof(Th135Converter) },
            { "TH145", typeof(Th145Converter) },
            { "TH155", typeof(Th155Converter) },
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
