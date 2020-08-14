//-----------------------------------------------------------------------
// <copyright file="Work.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel;
using Prism.Mvvm;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Represents a Touhou work.
    /// </summary>
    public sealed class Work : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Work"/> class.
        /// </summary>
        public Work()
        {
            LocalizeDictionary.Instance.PropertyChanged += this.LocalizeDictionaryPropertyChanged;
        }

        /// <summary>
        /// Gets or sets a number string. Should be a property name of <see cref="Properties.Resources"/>.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets a title string.
        /// </summary>
        public string Title => string.IsNullOrEmpty(this.Number)
            ? string.Empty : Utils.GetLocalizedValues<string>(this.Number);

        /// <summary>
        /// Gets or sets a value indicating whether the work is supported by this tool.
        /// </summary>
        public bool IsSupported { get; set; }

        private void LocalizeDictionaryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LocalizeDictionary.Instance.Culture))
            {
                this.RaisePropertyChanged(nameof(this.Title));
            }
        }
    }
}
