//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter
{
    /// <summary>
    /// Represents the settings of this application.
    /// </summary>
    [DataContract]
    public sealed class Settings : ISettings, INotifyPropertyChanged
    {
        private string lastTitle;
        private string fontFamilyName;
        private double? fontSize;
        private bool? outputNumberGroupSeparator;
        private int? inputCodePageId;
        private int? outputCodePageId;
        private string? language;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            this.lastTitle = string.Empty;
            this.Dictionary = new Dictionary<string, SettingsPerTitle>();
            this.fontFamilyName = SystemFonts.MessageFontFamily.Source;
            this.fontSize = SystemFonts.MessageFontSize;
            this.outputNumberGroupSeparator = true;
            this.inputCodePageId = 65001;
            this.outputCodePageId = 65001;
            this.language = CultureInfo.InvariantCulture.Name;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the valid code page identifiers for this application.
        /// </summary>
        public static IEnumerable<int> ValidCodePageIds { get; } = new[] { 65001, 932, 51932 };

        /// <summary>
        /// Gets the maximum font size for this application.
        /// </summary>
        public static double MaxFontSize { get; } = 72;

        /// <inheritdoc/>
        [DataMember(Order = 0)]
        public string LastTitle
        {
            get => this.lastTitle;
            set => this.SetProperty(ref this.lastTitle, value);
        }

        /// <inheritdoc/>
        [DataMember(Order = 1)]
        public Dictionary<string, SettingsPerTitle> Dictionary { get; private set; }

        /// <inheritdoc/>
        [DataMember(Order = 2)]
        public string FontFamilyName
        {
            get => this.fontFamilyName;
            set => this.SetProperty(ref this.fontFamilyName, value);
        }

        /// <inheritdoc/>
        [DataMember(Order = 3)]
        public double? FontSize
        {
            get => this.fontSize;
            set => this.SetProperty(ref this.fontSize, value);
        }

        /// <inheritdoc/>
        [DataMember(Order = 4)]
        public bool? OutputNumberGroupSeparator
        {
            get => this.outputNumberGroupSeparator;
            set => this.SetProperty(ref this.outputNumberGroupSeparator, value);
        }

        /// <inheritdoc/>
        [DataMember(Order = 5)]
        public int? InputCodePageId
        {
            get => this.inputCodePageId;
            set => this.SetProperty(ref this.inputCodePageId, value);
        }

        /// <inheritdoc/>
        [DataMember(Order = 6)]
        public int? OutputCodePageId
        {
            get => this.outputCodePageId;
            set => this.SetProperty(ref this.outputCodePageId, value);
        }

        /// <inheritdoc/>
        [DataMember(Order = 7)]
        public string? Language
        {
            get => this.language;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                try
                {
                    _ = CultureInfo.GetCultureInfo(value);
                    this.SetProperty(ref this.language, value);
                }
                catch (CultureNotFoundException)
                {
                }
            }
        }

        /// <inheritdoc/>
        public void Load(string path)
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using var reader = XmlReader.Create(stream, new XmlReaderSettings { CloseInput = false });
                var serializer = new DataContractSerializer(typeof(Settings));

                if (serializer.ReadObject(reader) is not Settings settings)
                    throw NewFileMayBeBrokenException(path);
                if (settings.LastTitle is null)
                    throw NewFileMayBeBrokenException(path);
                if (!ThConverterFactory.CanCreate(settings.LastTitle))
                    throw NewFileMayBeBrokenException(path);
                if (settings.Dictionary is null)
                    throw NewFileMayBeBrokenException(path);
                if (!settings.Dictionary.ContainsKey(settings.LastTitle))
                    throw NewFileMayBeBrokenException(path);

                this.LastTitle = settings.LastTitle;
                this.Dictionary = settings.Dictionary;

                if (!string.IsNullOrEmpty(settings.FontFamilyName))
                    this.FontFamilyName = settings.FontFamilyName;

                if (settings.FontSize.HasValue)
                {
                    if ((settings.FontSize.Value <= 0) || (settings.FontSize.Value > MaxFontSize))
                        throw NewFileMayBeBrokenException(path);

                    this.FontSize = settings.FontSize.Value;
                }

                if (settings.OutputNumberGroupSeparator.HasValue)
                    this.OutputNumberGroupSeparator = settings.OutputNumberGroupSeparator.Value;
                if (settings.InputCodePageId.HasValue &&
                    ValidCodePageIds.Any(id => id == settings.InputCodePageId.Value))
                    this.InputCodePageId = settings.InputCodePageId.Value;
                if (settings.OutputCodePageId.HasValue &&
                    ValidCodePageIds.Any(id => id == settings.OutputCodePageId.Value))
                    this.OutputCodePageId = settings.OutputCodePageId.Value;
                if (settings.Language is not null)
                    this.Language = settings.Language;
            }
            catch (FileNotFoundException)
            {
                // It's OK, do nothing.
            }
            catch (SerializationException e)
            {
                throw NewFileMayBeBrokenException(path, e);
            }
        }

        /// <inheritdoc/>
        public void Save(string path)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var settings = new XmlWriterSettings { CloseOutput = false, Indent = true };
            using var writer = XmlWriter.Create(stream, settings);
            var serializer = new DataContractSerializer(typeof(Settings));

            serializer.WriteObject(writer, this);
            writer.WriteWhitespace(settings.NewLineChars);
            writer.Flush();
        }

        /// <summary>
        /// Creates a new exception object indicating the file may be broken.
        /// </summary>
        /// <param name="file">A path of the file that may be broken.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <returns>A new <see cref="Exception"/> object.</returns>
        private static Exception NewFileMayBeBrokenException(string file, Exception? innerException = null)
        {
            return new InvalidDataException(
                Utils.Format(Resources.InvalidDataExceptionFileMayBeBroken, file), innerException);
        }

        /// <summary>
        /// Sets a value to a property.
        /// </summary>
        /// <typeparam name="T">The type of a value.</typeparam>
        /// <param name="storage">A backing field of the property to be set a value.</param>
        /// <param name="value">A value to set.</param>
        /// <param name="propertyName">The name of the property.</param>
        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (EqualityComparer<T>.Default.Equals(storage, value))
                return;

            storage = value;
            this.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
