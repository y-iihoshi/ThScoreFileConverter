//-----------------------------------------------------------------------
// <copyright file="ThConverterFactory.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Generates an instance that executes the conversion of a score file.
    /// </summary>
    internal static class ThConverterFactory
    {
        /// <summary>
        /// The dictionary of the types of the subclasses of the <see cref="ThConverter"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
            Justification = "Reviewed.")]
        private static readonly Dictionary<string, Type> ConverterTypes = new Dictionary<string, Type>
        {
            { Properties.Resources.keyTh06,  typeof(Th06Converter)  },
            { Properties.Resources.keyTh07,  typeof(Th07Converter)  },
            { Properties.Resources.keyTh08,  typeof(Th08Converter)  },
            { Properties.Resources.keyTh09,  typeof(Th09Converter)  },
            { Properties.Resources.keyTh095, typeof(Th095Converter) },
            { Properties.Resources.keyTh10,  typeof(Th10Converter)  },
            { Properties.Resources.keyTh11,  typeof(Th11Converter)  },
            { Properties.Resources.keyTh12,  typeof(Th12Converter)  },
            { Properties.Resources.keyTh125, typeof(Th125Converter) },
            { Properties.Resources.keyTh128, typeof(Th128Converter) },
            { Properties.Resources.keyTh13,  typeof(Th13Converter)  },
            { Properties.Resources.keyTh14,  typeof(Th14Converter)  }
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
