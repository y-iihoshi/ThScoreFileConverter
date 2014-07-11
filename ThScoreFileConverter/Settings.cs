//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Xml;

    /// <summary>
    /// Represents the settings of this application.
    /// </summary>
    [DataContract]
    public sealed class Settings
    {
        /// <summary>
        /// Only one instance of this class.
        /// </summary>
        private static readonly Settings InstanceImpl = new Settings();

        /// <summary>
        /// Prevents a default instance of the <see cref="Settings" /> class from being created.
        /// </summary>
        private Settings()
        {
            this.LastTitle = string.Empty;
            this.Dictionary = new Dictionary<string, SettingsPerTitle>();
            this.FontFamilyName = SystemFonts.MessageFontFamily.Source;
            this.FontSize = SystemFonts.MessageFontSize;
            this.OutputNumberGroupSeparator = true;
            this.InputCodePageId = 932;
            this.OutputCodePageId = 932;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                return InstanceImpl;
            }
        }

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
        public string FontFamilyName { get; set; }

        /// <summary>
        /// Gets or sets the font size used for the UI of this application.
        /// </summary>
        [DataMember(Order = 3)]
        public double? FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether numeric values is output with thousand separator
        /// characters.
        /// </summary>
        [DataMember(Order = 4)]
        public bool? OutputNumberGroupSeparator { get; set; }

        /// <summary>
        /// Gets or sets the code page identifier for input files.
        /// </summary>
        [DataMember(Order = 5)]
        public int? InputCodePageId { get; set; }

        /// <summary>
        /// Gets or sets the code page identifier for output files.
        /// </summary>
        [DataMember(Order = 6)]
        public int? OutputCodePageId { get; set; }

        /// <summary>
        /// Loads the settings from the specified XML file.
        /// </summary>
        /// <param name="path">The path of the XML file to load.</param>
        public void Load(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (var reader =
                    XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
                {
                    var serializer = new DataContractSerializer(typeof(Settings));
                    var settings = serializer.ReadObject(reader) as Settings;
                    if (settings != null)
                    {
                        this.LastTitle = settings.LastTitle;
                        this.Dictionary = settings.Dictionary;
                        this.FontFamilyName =
                            settings.FontFamilyName ?? SystemFonts.MessageFontFamily.Source;
                        this.FontSize = settings.FontSize ?? SystemFonts.MessageFontSize;
                        this.OutputNumberGroupSeparator = settings.OutputNumberGroupSeparator ?? true;
                        this.InputCodePageId = settings.InputCodePageId ?? 932;
                        this.OutputCodePageId = settings.OutputCodePageId ?? 932;
                    }
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
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true }))
            {
                var serializer = new DataContractSerializer(typeof(Settings));
                serializer.WriteObject(writer, this);
                writer.WriteWhitespace(writer.Settings.NewLineChars);
                stream.Flush();
                stream.SetLength(stream.Position);
            }
        }
    }
}
