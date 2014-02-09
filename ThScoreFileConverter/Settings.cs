//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName",
    Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.MaintainabilityRules",
    "SA1402:FileMayOnlyContainASingleClass",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Xml;

    /// <summary>
    /// Represents settings per work.
    /// </summary>
    public class SettingsPerTitle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPerTitle"/> class.
        /// </summary>
        public SettingsPerTitle()
        {
            this.ScoreFile = string.Empty;
            this.BestShotDirectory = string.Empty;
            this.TemplateFiles = null;
            this.OutputDirectory = string.Empty;
            this.ImageOutputDirectory = string.Empty;
        }

        /// <summary>
        /// Gets or sets the path of the score file.
        /// </summary>
        public string ScoreFile { get; set; }

        /// <summary>
        /// Gets or sets the path of the best shot directory.
        /// </summary>
        public string BestShotDirectory { get; set; }

        /// <summary>
        /// Gets or sets the array of the paths of template files.
        /// </summary>
        public IEnumerable<string> TemplateFiles { get; set; }

        /// <summary>
        /// Gets or sets the path of the output directory.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the path of the output directory of the image files.
        /// </summary>
        public string ImageOutputDirectory { get; set; }
    }

    /// <summary>
    /// Represents the settings of this application.
    /// </summary>
    [DataContract]
    public class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            this.LastTitle = string.Empty;
            this.Dictionary = new Dictionary<string, SettingsPerTitle>();
            this.FontFamilyName = SystemFonts.MessageFontFamily.Source;
            this.FontSize = SystemFonts.MessageFontSize;
            this.OutputNumberGroupSeparator = true;
        }

        /// <summary>
        /// Gets or sets the last selected work.
        /// </summary>
        [DataMember(Order = 0)]
        public string LastTitle { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of <see cref="SettingsPerTitle"/> instances.
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
                    var settings = (Settings)serializer.ReadObject(reader);
                    this.LastTitle = settings.LastTitle;
                    this.Dictionary = settings.Dictionary;
                    this.FontFamilyName = settings.FontFamilyName ?? SystemFonts.MessageFontFamily.Source;
                    this.FontSize = settings.FontSize ?? SystemFonts.MessageFontSize;
                    this.OutputNumberGroupSeparator = settings.OutputNumberGroupSeparator ?? true;
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
