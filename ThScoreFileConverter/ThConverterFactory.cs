//-----------------------------------------------------------------------
// <copyright file="ThConverterFactory.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ThScoreFileConverter.Properties;

    /// <summary>
    /// Generates an instance that executes the conversion of a score file.
    /// </summary>
    internal static class ThConverterFactory
    {
        /// <summary>
        /// The dictionary of the types of the subclasses of the <see cref="ThConverter"/> class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static readonly Dictionary<string, Type> ConverterTypes = new Dictionary<string, Type>
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
            { Resources.keyTh075, typeof(Th075Converter) },
            { Resources.keyTh105, typeof(Th105Converter) }
        };

        /// <summary>
        /// Creates a new instance of the subclass of the <see cref="ThConverter"/> class.
        /// </summary>
        /// <param name="key">The string to specify the subclass.</param>
        /// <returns>An instance of the subclass specified by <paramref name="key"/>.</returns>
        public static ThConverter Create(string key)
        {
            Type type = null;
            return ConverterTypes.TryGetValue(key, out type)
                ? Activator.CreateInstance(type) as ThConverter : null;
        }
    }
}
