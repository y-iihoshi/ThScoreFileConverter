//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter
{
    /// <summary>
    /// Represents the settings of this application.
    /// </summary>
    [DataContract]
    public sealed class Settings
    {
        private string fontFamilyName;
        private double? fontSize;
        private bool? outputNumberGroupSeparator;
        private int? inputCodePageId;
        private int? outputCodePageId;

        /// <summary>
        /// Prevents a default instance of the <see cref="Settings" /> class from being created.
        /// </summary>
        private Settings()
        {
            this.LastTitle = string.Empty;
            this.Dictionary = new Dictionary<string, SettingsPerTitle>();
            this.fontFamilyName = SystemFonts.MessageFontFamily.Source;
            this.fontSize = SystemFonts.MessageFontSize;
            this.outputNumberGroupSeparator = true;
            this.inputCodePageId = 65001;
            this.outputCodePageId = 65001;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Settings Instance { get; } = new Settings();

        /// <summary>
        /// Gets the valid code page identifiers for this application.
        /// </summary>
        public static IEnumerable<int> ValidCodePageIds { get; } = new[] { 65001, 932, 51932 };

        /// <summary>
        /// Gets or sets the last selected work.
        /// </summary>
        [DataMember(Order = 0)]
        public string LastTitle { get; set; }

        /// <summary>
        /// Gets the dictionary of <see cref="SettingsPerTitle"/> instances.
        /// </summary>
        [DataMember(Order = 1)]
        public Dictionary<string, SettingsPerTitle> Dictionary { get; private set; }

        /// <summary>
        /// Gets or sets the font family name used for the UI of this application.
        /// </summary>
        [DataMember(Order = 2)]
        public string FontFamilyName
        {
            get => this.fontFamilyName;
            set
            {
                this.OnNullablePropertyChanging(value);
                this.fontFamilyName = value;
            }
        }

        /// <summary>
        /// Gets or sets the font size used for the UI of this application.
        /// </summary>
        [DataMember(Order = 3)]
        public double? FontSize
        {
            get => this.fontSize;
            set
            {
                this.OnNullablePropertyChanging(value);
                this.fontSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether numeric values is output with thousand separator
        /// characters.
        /// </summary>
        [DataMember(Order = 4)]
        public bool? OutputNumberGroupSeparator
        {
            get => this.outputNumberGroupSeparator;
            set
            {
                this.OnNullablePropertyChanging(value);
                this.outputNumberGroupSeparator = value;
            }
        }

        /// <summary>
        /// Gets or sets the code page identifier for input files.
        /// </summary>
        [DataMember(Order = 5)]
        public int? InputCodePageId
        {
            get => this.inputCodePageId;
            set
            {
                this.OnNullablePropertyChanging(value);
                this.inputCodePageId = value;
            }
        }

        /// <summary>
        /// Gets or sets the code page identifier for output files.
        /// </summary>
        [DataMember(Order = 6)]
        public int? OutputCodePageId
        {
            get => this.outputCodePageId;
            set
            {
                this.OnNullablePropertyChanging(value);
                this.outputCodePageId = value;
            }
        }

        /// <summary>
        /// Loads the settings from the specified XML file.
        /// </summary>
        /// <param name="path">The path of the XML file to load.</param>
        public void Load(string path)
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using var reader = XmlReader.Create(stream, new XmlReaderSettings { CloseInput = false });
                var serializer = new DataContractSerializer(typeof(Settings));

                if (serializer.ReadObject(reader) is Settings settings)
                {
                    this.LastTitle = settings.LastTitle;
                    this.Dictionary = settings.Dictionary;

                    if (!string.IsNullOrEmpty(settings.FontFamilyName))
                        this.FontFamilyName = settings.FontFamilyName;
                    if (settings.FontSize.HasValue)
                        this.FontSize = settings.FontSize.Value;
                    if (settings.OutputNumberGroupSeparator.HasValue)
                        this.OutputNumberGroupSeparator = settings.OutputNumberGroupSeparator.Value;
                    if (settings.InputCodePageId.HasValue &&
                        ValidCodePageIds.Any(id => id == settings.InputCodePageId.Value))
                        this.InputCodePageId = settings.InputCodePageId.Value;
                    if (settings.OutputCodePageId.HasValue &&
                        ValidCodePageIds.Any(id => id == settings.OutputCodePageId.Value))
                        this.OutputCodePageId = settings.OutputCodePageId.Value;
                }
            }
            catch (FileNotFoundException)
            {
                // It's OK, do nothing.
            }
            catch (SerializationException e)
            {
                throw new InvalidDataException(Utils.Format("{0} may be broken.", path), e);
            }
            catch (XmlException e)
            {
                throw new InvalidDataException(Utils.Format("{0} may be broken.", path), e);
            }
        }

        /// <summary>
        /// Saves the settings to the specified XML file.
        /// </summary>
        /// <param name="path">The path of the XML file to save.</param>
        public void Save(string path)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = XmlWriter.Create(stream, new XmlWriterSettings { CloseOutput = false, Indent = true });
            var serializer = new DataContractSerializer(typeof(Settings));

            serializer.WriteObject(writer, this);
            writer.WriteWhitespace(writer.Settings.NewLineChars);
            writer.Flush();
        }

        private void OnNullablePropertyChanging<T>(T value)
        {
            if (ReferenceEquals(this, Instance) && (value is null))
                throw new ArgumentNullException(nameof(value));
        }
    }
}
