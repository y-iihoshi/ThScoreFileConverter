//-----------------------------------------------------------------------
// <copyright file="SettingWindowViewModel.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.Practices.Prism.Mvvm;
    using ThScoreFileConverter.Models;

    /// <summary>
    /// The view model class for <see cref="ThScoreFileConverter.Views.SettingWindow"/>.
    /// </summary>
    internal class SettingWindowViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingWindowViewModel"/> class.
        /// </summary>
        public SettingWindowViewModel()
        {
            this.Title = "Settings";    // FIXME

            var encodings = Settings.ValidCodePageIds
                .ToDictionary(id => id, id => Utils.GetEncoding(id).EncodingName);
            this.InputEncodings = encodings;
            this.OutputEncodings = encodings;
        }

        /// <summary>
        /// Gets a title of the Settings window.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public string Title { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether numeric values is output with thousand separator
        /// characters.
        /// </summary>
        public bool OutputNumberGroupSeparator
        {
            get
            {
                return Settings.Instance.OutputNumberGroupSeparator.Value;
            }

            set
            {
                if (Settings.Instance.OutputNumberGroupSeparator != value)
                {
                    Settings.Instance.OutputNumberGroupSeparator = value;
                    this.OnPropertyChanged(() => this.OutputNumberGroupSeparator);
                }
            }
        }

        /// <summary>
        /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
        /// input files.
        /// </summary>
        public IDictionary<int, string> InputEncodings { get; private set; }

        /// <summary>
        /// Gets or sets the code page identifier for input files.
        /// </summary>
        public int InputCodePageId
        {
            get
            {
                return Settings.Instance.InputCodePageId.Value;
            }

            set
            {
                if (Settings.Instance.InputCodePageId != value)
                {
                    Settings.Instance.InputCodePageId = value;
                    this.OnPropertyChanged(() => this.InputCodePageId);
                }
            }
        }

        /// <summary>
        /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
        /// output files.
        /// </summary>
        public IDictionary<int, string> OutputEncodings { get; private set; }

        /// <summary>
        /// Gets or sets the code page identifier for output files.
        /// </summary>
        public int OutputCodePageId
        {
            get
            {
                return Settings.Instance.OutputCodePageId.Value;
            }

            set
            {
                if (Settings.Instance.OutputCodePageId != value)
                {
                    Settings.Instance.OutputCodePageId = value;
                    this.OnPropertyChanged(() => this.OutputCodePageId);
                }
            }
        }
    }
}
