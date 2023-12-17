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
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;

#if NETFRAMEWORK
using ThScoreFileConverter.Extensions;
#endif

namespace ThScoreFileConverter;

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
    private double? windowWidth;
    private double? windowHeight;
    private double? mainContentHeight;
    private double? subContentHeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    public Settings()
    {
        this.lastTitle = string.Empty;
        this.Dictionary = [];
        this.fontFamilyName = SystemFonts.MessageFontFamily.Source;
        this.fontSize = SystemFonts.MessageFontSize;
        this.outputNumberGroupSeparator = true;
        this.inputCodePageId = 65001;
        this.outputCodePageId = 65001;
        this.language = CultureInfo.InvariantCulture.Name;
        this.windowWidth = WindowMinWidth;
        this.windowHeight = WindowMinHeight;
        this.mainContentHeight = MainContentMinHeight;
        this.subContentHeight = SubContentMinHeight;
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

    /// <summary>
    /// Gets the minimum width of the main window.
    /// </summary>
    public static double WindowMinWidth { get; } = 480;

    /// <summary>
    /// Gets the minimum height of the main window.
    /// </summary>
    public static double WindowMinHeight { get; } = 480;

    /// <summary>
    /// Gets the minimum height of the main content area in the main window.
    /// </summary>
    public static double MainContentMinHeight { get; } = 240;

    /// <summary>
    /// Gets the minimum height of the sub content area in the main window.
    /// </summary>
    public static double SubContentMinHeight { get; } = 80;

    /// <inheritdoc/>
    [DataMember(Order = 0)]
    public string LastTitle
    {
        get => this.lastTitle;
        set
        {
            Guard.IsNotNull(value);

            if (ThConverterFactory.CanCreate(value))
            {
                if (this.SetProperty(ref this.lastTitle, value))
                    _ = this.Dictionary?.TryAdd(this.lastTitle, new SettingsPerTitle());
            }
        }
    }

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
            Guard.IsNotNull(value);

            try
            {
                _ = CultureInfo.GetCultureInfo(value);
                _ = this.SetProperty(ref this.language, value);
            }
            catch (CultureNotFoundException)
            {
            }
        }
    }

    /// <inheritdoc/>
    [DataMember(Order = 8)]
    public double? WindowWidth
    {
        get => this.windowWidth;
        set => this.SetProperty(ref this.windowWidth, value);
    }

    /// <inheritdoc/>
    [DataMember(Order = 9)]
    public double? WindowHeight
    {
        get => this.windowHeight;
        set => this.SetProperty(ref this.windowHeight, value);
    }

    /// <inheritdoc/>
    [DataMember(Order = 10)]
    public double? MainContentHeight
    {
        get => this.mainContentHeight;
        set => this.SetProperty(ref this.mainContentHeight, value);
    }

    /// <inheritdoc/>
    [DataMember(Order = 11)]
    public double? SubContentHeight
    {
        get => this.subContentHeight;
        set => this.SetProperty(ref this.subContentHeight, value);
    }

    /// <summary>
    /// Gets the number of <see cref="SettingsPerTitle"/> instances.
    /// </summary>
    /// <returns>The number of <see cref="SettingsPerTitle"/> instances.</returns>
    public int NumTitles => this.Dictionary.Count;

    /// <summary>
    /// Gets or sets the dictionary of <see cref="SettingsPerTitle"/> instances.
    /// </summary>
    [DataMember(Order = 1)]
    private Dictionary<string, SettingsPerTitle> Dictionary { get; set; }

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

            if (settings.WindowWidth.HasValue && (settings.WindowWidth.Value >= WindowMinWidth))
                this.WindowWidth = settings.WindowWidth.Value;
            if (settings.WindowHeight.HasValue && (settings.WindowHeight.Value >= WindowMinHeight))
                this.WindowHeight = settings.WindowHeight.Value;
            if (settings.MainContentHeight.HasValue && (settings.MainContentHeight.Value >= MainContentMinHeight))
                this.MainContentHeight = settings.MainContentHeight.Value;
            if (settings.SubContentHeight.HasValue && (settings.SubContentHeight.Value >= SubContentMinHeight))
                this.SubContentHeight = settings.SubContentHeight.Value;
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
    /// Gets the setting per title.
    /// </summary>
    /// <param name="title">The key of the title.</param>
    /// <returns>The <see cref="SettingsPerTitle"/>instance.</returns>
    public SettingsPerTitle GetSettingsPerTitle(string title)
    {
        return this.Dictionary.TryGetValue(title, out var settings) ? settings : new SettingsPerTitle();
    }

    /// <summary>
    /// Creates a new exception object indicating the file may be broken.
    /// </summary>
    /// <param name="file">A path of the file that may be broken.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <returns>A new <see cref="Exception"/> object.</returns>
    private static InvalidDataException NewFileMayBeBrokenException(string file, Exception? innerException = null)
    {
        return new InvalidDataException(
            StringHelper.Format(ExceptionMessages.InvalidDataExceptionFileMayBeBroken, file), innerException);
    }

    /// <summary>
    /// Sets a value to a property.
    /// </summary>
    /// <typeparam name="T">The type of a value.</typeparam>
    /// <param name="storage">A backing field of the property to be set a value.</param>
    /// <param name="value">A value to set.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns><see langword="true"/> if <paramref name="storage"/> was changed; otherwise <see langword="false"/>.</returns>
    private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
    {
        Guard.IsNotNull(value);

        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;

        storage = value;
        this.RaisePropertyChanged(propertyName);
        return true;
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
