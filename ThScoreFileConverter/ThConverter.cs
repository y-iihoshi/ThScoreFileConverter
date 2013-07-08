using System;
using System.Collections.Generic;
using System.IO;

namespace ThScoreFileConverter
{
    /// <summary>
    /// スコアファイルの変換を行うインスタンスを生成するクラス
    /// </summary>
    public class ThConverterFactory
    {
        /// <summary>
        /// ThConverter クラスのサブクラス群の型
        /// </summary>
        private static readonly Dictionary<string, Type> ConverterTypes = new Dictionary<string, Type>
        {
            { Properties.Resources.keyTh06,  typeof(Th06Converter)  },
            { Properties.Resources.keyTh07,  typeof(Th07Converter)  },
            { Properties.Resources.keyTh08,  typeof(Th08Converter)  },
            { Properties.Resources.keyTh09,  typeof(Th09Converter)  },
            { Properties.Resources.keyTh095, typeof(Th095Converter) },
            { Properties.Resources.keyTh10,  typeof(Th10Converter)  }
        };

        /// <summary>
        /// ThConverter クラスのサブクラスインスタンスを生成する
        /// </summary>
        /// <param Name="key">サブクラスを指定する文字列</param>
        /// <returns>key が示すサブクラスのインスタンス</returns>
        public static ThConverter Create(string key)
        {
            return ConverterTypes.ContainsKey(key)
                ? (ThConverter)Activator.CreateInstance(ConverterTypes[key]) : null;
        }
    }

    /// <summary>
    /// ThConverter クラスが発行するイベントのデータ
    /// </summary>
    public class ThConverterEventArgs : EventArgs
    {
        /// <summary>
        /// 本イベント発行時点で最後に出力したファイルのパス
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 本イベント発行時点で出力済みのファイル数
        /// </summary>
        public int Current { get; private set; }

        /// <summary>
        /// 処理対象ファイルの総数
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// 本イベントのメッセージ
        /// </summary>
        public string Message
        {
            get
            {
                return string.Format("({0}/{1}) {2} ", Current, Total, System.IO.Path.GetFileName(Path));
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param Name="path">最後に出力したファイルのパス</param>
        /// <param Name="current">出力済みファイル数</param>
        /// <param Name="total">処理対象ファイルの総数</param>
        public ThConverterEventArgs(string path, int current, int total)
        {
            this.Path = path;
            this.Current = current;
            this.Total = total;
        }
    }

    public class ExceptionOccurredEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public ExceptionOccurredEventArgs(Exception e)
        {
            this.Exception = e;
        }
    }

    /// <summary>
    /// スコアファイルの変換を行うクラス群の基底クラス
    /// </summary>
    public class ThConverter
    {
        /// <summary>
        /// 変換対象スコアファイルの対応バージョン
        /// サブクラスでの override が必要
        /// </summary>
        public virtual string SupportedVersions { get { return null; } }

        /// <summary>
        /// ベストショットファイルの変換処理を持っているかを示す
        /// 変換処理を実装するサブクラスでは override が必要
        /// </summary>
        public virtual bool HasBestShotConverter { get { return false; } }

        /// <summary>
        /// ファイル毎の変換処理の完了を示すイベント
        /// </summary>
        public event EventHandler<ThConverterEventArgs> ConvertFinished;

        /// <summary>
        /// 全ての変換処理の完了を示すイベント
        /// </summary>
        public event EventHandler<ThConverterEventArgs> ConvertAllFinished;

        /// <summary>
        /// 例外発生を示すイベント
        /// </summary>
        public event EventHandler<ExceptionOccurredEventArgs> ExceptionOccurred;

        /// <summary>
        /// 本クラスのインスタンス生成を禁止する
        /// </summary>
        protected ThConverter() { }

        /// <summary>
        /// スコアファイルの変換処理
        /// スレッドのトップレベルにするために 1 引数化
        /// </summary>
        /// <param Name="obj">SettingsPerTitle インスタンス</param>
        public void Convert(Object obj)
        {
            try
            {
                if (obj is SettingsPerTitle)
                {
                    var args = (SettingsPerTitle)obj;
                    this.Convert(
                        args.ScoreFile, args.BestShotDirectory, args.TemplateFiles, args.OutputDirectory);
                }
            }
            catch (Exception e)
            {
                this.OnExceptionOccurred(new ExceptionOccurredEventArgs(e));
            }
        }

        /// <summary>
        /// スコアファイルの変換処理
        /// </summary>
        /// <param Name="score">スコアファイルのパス</param>
        /// <param Name="bestshot">ベストショットディレクトリのパス</param>
        /// <param Name="templates">テンプレートファイル群のパス</param>
        /// <param Name="output">変換後の出力ファイル群のパス</param>
        private void Convert(string score, string bestshot, string[] templates, string output)
        {
            using (var scr = new FileStream(score, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(scr))
            {
                scr.Seek(0, SeekOrigin.Begin);
                if (!this.ReadScoreFile(scr))
                    throw new NotSupportedException(Properties.Resources.msgErrScoreFileNotSupported);

                if (this.HasBestShotConverter)
                {
                    var dir = Path.Combine(output, Properties.Resources.strBestShotDirectory);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    var files = Directory.GetFiles(bestshot, Properties.Resources.ptnBestShot);
                    for (var index = 0; index < files.Length; index++)
                    {
                        var result = GetBestShotFilePath(files[index], dir);
                        using (var bsts = new FileStream(files[index], FileMode.Open, FileAccess.Read))
                        using (var rslt = new FileStream(result, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            this.ConvertBestShot(bsts, rslt);
                            this.OnConvertFinished(new ThConverterEventArgs(result, index + 1, files.Length));
                        }
                    }
                }

                for (var index = 0; index < templates.Length; index++)
                {
                    var result = GetOutputFilePath(templates[index], output);
                    using (var tmpl = new FileStream(templates[index], FileMode.Open, FileAccess.Read))
                    using (var rslt = new FileStream(result, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        this.Convert(tmpl, rslt);
                        this.OnConvertFinished(new ThConverterEventArgs(result, index + 1, templates.Length));
                    }
                }

                this.OnConvertAllFinished(new ThConverterEventArgs("", 0, 0));
            }
        }

        /// <summary>
        /// スコアファイルからの読み込み
        /// サブクラスでの override が必要
        /// </summary>
        /// <param Name="score">スコアファイル</param>
        protected virtual bool ReadScoreFile(Stream input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// スコアの変換処理（テンプレートファイル毎）
        /// サブクラスでの override が必要
        /// </summary>
        /// <param Name="input">テンプレートファイル</param>
        /// <param Name="output">出力ファイル</param>
        protected virtual void Convert(Stream input, Stream output)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ベストショットファイルの変換処理
        /// 変換処理を実装するサブクラスでは override が必要
        /// </summary>
        /// <param Name="input">変換前のベストショットファイル</param>
        /// <param Name="output">変換後のベストショットファイル</param>
        protected virtual void ConvertBestShot(Stream input, Stream output)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ファイル毎の変換処理の完了時の処理
        /// </summary>
        /// <param Name="e">ThConverter クラスから発行するイベントのデータ</param>
        private void OnConvertFinished(ThConverterEventArgs e)
        {
            var handler = this.ConvertFinished;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// 全ての変換処理の完了時の処理
        /// </summary>
        /// <param Name="e">ThConverter クラスから発行するイベントのデータ</param>
        private void OnConvertAllFinished(ThConverterEventArgs e)
        {
            var handler = this.ConvertAllFinished;
            if (handler != null)
                handler(this, e);
        }

        private void OnExceptionOccurred(ExceptionOccurredEventArgs e)
        {
            var handler = this.ExceptionOccurred;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// 出力ファイルのパスを取得する
        /// </summary>
        /// <param Name="templateFile">テンプレートファイルのパス</param>
        /// <param Name="outputDirectory">出力先ディレクトリのパス</param>
        /// <returns>出力ファイルのパス</returns>
        private static string GetOutputFilePath(string templateFile, string outputDirectory)
        {
            var outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(templateFile));
            if (outputDirectory == Path.GetDirectoryName(templateFile))
                outputFile += "_";
            outputFile += Path.GetExtension(templateFile);

            return outputFile;
        }

        /// <summary>
        /// 変換後のベストショットファイルのパスを取得する
        /// </summary>
        /// <param Name="bestshotFile">変換前のベストショットファイルのパス</param>
        /// <param Name="outputDirectory">出力先ディレクトリのパス</param>
        /// <returns>変換後のベストショットファイルのパス</returns>
        private static string GetBestShotFilePath(string bestshotFile, string outputDirectory)
        {
            var outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(bestshotFile));
            if (outputDirectory == Path.GetDirectoryName(bestshotFile))
                outputFile += "_";
            outputFile += Properties.Resources.strBestShotExtension;

            return outputFile;
        }
    }
}
