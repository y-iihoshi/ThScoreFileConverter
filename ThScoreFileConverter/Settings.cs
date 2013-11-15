using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml;

namespace ThScoreFileConverter
{
    /// <summary>
    /// 作品毎の設定を表すクラス
    /// </summary>
    public class SettingsPerTitle
    {
        /// <summary>
        /// スコアファイルのパス
        /// </summary>
        public string ScoreFile { get; set; }

        /// <summary>
        /// ベストショットディレクトリのパス
        /// </summary>
        public string BestShotDirectory { get; set; }

        /// <summary>
        /// テンプレートファイル群のパス
        /// </summary>
        public string[] TemplateFiles { get; set; }

        /// <summary>
        /// 出力先ディレクトリのパス
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// 画像ファイル出力先ディレクトリのパス
        /// </summary>
        public string ImageOutputDirectory { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingsPerTitle()
        {
            this.ScoreFile = "";
            this.BestShotDirectory = "";
            this.TemplateFiles = new string[] { };
            this.OutputDirectory = "";
            this.ImageOutputDirectory = "";
        }
    }

    /// <summary>
    /// 本ツールの設定を扱うクラス
    /// </summary>
    [DataContract()]
    public class Settings
    {
        /// <summary>
        /// 前回終了時に選択していた作品
        /// </summary>
        [DataMember(Order = 0)]
        public string LastTitle { get; set; }

        /// <summary>
        /// 作品毎の設定を保持する dictionary
        /// </summary>
        [DataMember(Order = 1)]
        public Dictionary<string, SettingsPerTitle> Dictionary { get; set; }

        /// <summary>
        /// UI に使うフォントの名前
        /// </summary>
        [DataMember(Order = 2)]
        public string FontFamilyName { get; set; }

        /// <summary>
        /// UI に使うフォントのサイズ
        /// </summary>
        [DataMember(Order = 3)]
        public double? FontSize { get; set; }

        /// <summary>
        /// 数値を桁区切り形式で出力する場合 true
        /// </summary>
        [DataMember(Order = 4)]
        public bool? OutputNumberGroupSeparator { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Settings()
        {
            this.LastTitle = "";
            this.Dictionary = new Dictionary<string, SettingsPerTitle>();
            this.FontFamilyName = SystemFonts.MessageFontFamily.Source;
            this.FontSize = SystemFonts.MessageFontSize;
            this.OutputNumberGroupSeparator = true;
        }

        /// <summary>
        /// XML ファイルからの設定読み込み
        /// </summary>
        /// <param Name="path">読み込み元の XML ファイル</param>
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
                throw new InvalidDataException(string.Format("{0} may be broken.", path), e);
            }
            catch (XmlException e)
            {
                throw new InvalidDataException(string.Format("{0} may be broken.", path), e);
            }
        }

        /// <summary>
        /// XML ファイルへの設定保存
        /// </summary>
        /// <param Name="path">保存先 XML ファイル</param>
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
