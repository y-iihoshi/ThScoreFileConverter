//-----------------------------------------------------------------------
// <copyright file="ThConverter.cs" company="None">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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

    /// <summary>
    /// Represents the event data issued by the <see cref="ThConverter"/> class.
    /// </summary>
    internal class ThConverterEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThConverterEventArgs"/> class.
        /// </summary>
        /// <param name="path">The path of the last output file.</param>
        /// <param name="current">The number of the files that have been output.</param>
        /// <param name="total">The total number of the files.</param>
        public ThConverterEventArgs(string path, int current, int total)
        {
            this.Path = path;
            this.Current = current;
            this.Total = total;
        }

        /// <summary>
        /// Gets the path of the last output file.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the number of the files that have been output.
        /// </summary>
        public int Current { get; private set; }

        /// <summary>
        /// Gets the total number of the files.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Gets a message string that represents the current instance.
        /// </summary>
        public string Message
        {
            get
            {
                return Utils.Format(
                    "({0}/{1}) {2} ", this.Current, this.Total, System.IO.Path.GetFileName(this.Path));
            }
        }
    }

    /// <summary>
    /// Represents the event data that indicates occurring of an exception.
    /// </summary>
    internal class ExceptionOccurredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionOccurredEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The exception data.</param>
        public ExceptionOccurredEventArgs(Exception ex)
        {
            this.Exception = ex;
        }

        /// <summary>
        /// Gets the exception data.
        /// </summary>
        public Exception Exception { get; private set; }
    }

    /// <summary>
    /// Represents the base class for classes that executes conversion of a score file.
    /// </summary>
    internal class ThConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThConverter"/> class.
        /// </summary>
        protected ThConverter()
        {
        }

        /// <summary>
        /// Represents the event that the conversion process per file has finished.
        /// </summary>
        public event EventHandler<ThConverterEventArgs> ConvertFinished;

        /// <summary>
        /// Represents the event that all conversion process has finished.
        /// </summary>
        public event EventHandler<ThConverterEventArgs> ConvertAllFinished;

        /// <summary>
        /// Represents the event that an exception has occurred.
        /// </summary>
        public event EventHandler<ExceptionOccurredEventArgs> ExceptionOccurred;

        /// <summary>
        /// Gets the string indicating the supported version of the score file to convert.
        /// </summary>
        /// <remarks>It is required to override this method by a subclass.</remarks>
        public virtual string SupportedVersions
        {
            get { return null; }
        }

        /// <summary>
        /// Gets a value indicating whether the current instance has the conversion method for best-shot
        /// files or not.
        /// </summary>
        /// <remarks>
        /// It is required to override this method by the subclass that implements the conversion process
        /// of best-shot files.
        /// </remarks>
        public virtual bool HasBestShotConverter
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a numeric value is output with thousand separator
        /// characters.
        /// </summary>
        public bool OutputNumberGroupSeparator { get; set; }

        /// <summary>
        /// Converts a score file.
        /// </summary>
        /// <param name="threadArg">An instance of the <see cref="SettingsPerTitle"/> class.</param>
        public void Convert(object threadArg)
        {
            try
            {
#if DEBUG
                using (var profiler = new Profiler("Convert"))
                    this.Convert(threadArg as SettingsPerTitle);
#else
                this.Convert(threadArg as SettingsPerTitle);
#endif
            }
            catch (Exception e)
            {
                this.OnExceptionOccurred(new ExceptionOccurredEventArgs(e));
                throw;
            }
        }

        /// <summary>
        /// Reads from the input stream that treats a score file.
        /// </summary>
        /// <remarks>Needs to be overridden by a subclass.</remarks>
        /// <param name="input">The input stream that treats a score file.</param>
        /// <returns><c>true</c> if read successfully; otherwise, <c>false</c>.</returns>
        protected virtual bool ReadScoreFile(Stream input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the stream indicating score data.
        /// </summary>
        /// <remarks>Needs to be overridden by a subclass.</remarks>
        /// <param name="input">The input stream that treats a template file.</param>
        /// <param name="output">The stream for outputting the converted data.</param>
        /// <param name="hideUntriedCards"><c>true</c> if it hides untried spell cards.</param>
        protected virtual void Convert(Stream input, Stream output, bool hideUntriedCards)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the stream indicating best shot data.
        /// </summary>
        /// <remarks>Needs to be overridden by the subclass that implements the conversion.</remarks>
        /// <param name="input">The input stream that treats the best shot data to convert.</param>
        /// <param name="output">The stream for outputting converted best shot data.</param>
        protected virtual void ConvertBestShot(Stream input, Stream output)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filters the file paths to extract the paths of best shot files.
        /// </summary>
        /// <remarks>
        /// Needs to be overridden by the subclass that implements the conversion of best shot data.
        /// </remarks>
        /// <param name="files">The array of file paths to filter.</param>
        /// <returns>An array of the paths of best shot files.</returns>
        protected virtual string[] FilterBestShotFiles(string[] files)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a string that represents the specified numeric value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="number"/>.</typeparam>
        /// <param name="number">A numeric value.</param>
        /// <returns>A string that represents <paramref name="number"/>.</returns>
        protected string ToNumberString<T>(T number) where T : struct
        {
            return Utils.ToNumberString(number, this.OutputNumberGroupSeparator);
        }

        /// <summary>
        /// Gets a path string of the output file.
        /// </summary>
        /// <param name="templateFile">A path string of the template file.</param>
        /// <param name="outputDirectory">A path string of the output directory.</param>
        /// <returns>A path string of the output file.</returns>
        private static string GetOutputFilePath(string templateFile, string outputDirectory)
        {
            var outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(templateFile));
            if (outputDirectory == Path.GetDirectoryName(templateFile))
                outputFile += "_";
            outputFile += Path.GetExtension(templateFile);

            return outputFile;
        }

        /// <summary>
        /// Gets a path string of the converted best shot file.
        /// </summary>
        /// <param name="bestshotFile">A path string of the best shot file before conversion.</param>
        /// <param name="outputDirectory">A path string of the output directory.</param>
        /// <returns>A path string of the converted best shot file.</returns>
        private static string GetBestShotFilePath(string bestshotFile, string outputDirectory)
        {
            var outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(bestshotFile));
            if (outputDirectory == Path.GetDirectoryName(bestshotFile))
                outputFile += "_";
            outputFile += Properties.Resources.strBestShotExtension;

            return outputFile;
        }

        /// <summary>
        /// Converts a score file.
        /// </summary>
        /// <param name="settings">The settings per work.</param>
        private void Convert(SettingsPerTitle settings)
        {
            using (var scr = new FileStream(settings.ScoreFile, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(scr))
            {
                scr.Seek(0, SeekOrigin.Begin);
                if (!this.ReadScoreFile(scr))
                    throw new NotSupportedException(Properties.Resources.msgErrScoreFileNotSupported);

                if (this.HasBestShotConverter)
                {
                    var dir = Path.Combine(settings.OutputDirectory, settings.ImageOutputDirectory);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    var files = this.FilterBestShotFiles(
                        Directory.GetFiles(settings.BestShotDirectory, Properties.Resources.ptnBestShot));
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

                var numFiles = settings.TemplateFiles.Count();
                for (var index = 0; index < numFiles; index++)
                {
                    var template = settings.TemplateFiles.ElementAt(index);
                    var result = GetOutputFilePath(template, settings.OutputDirectory);
                    using (var tmpl = new FileStream(template, FileMode.Open, FileAccess.Read))
                    using (var rslt = new FileStream(result, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        this.Convert(tmpl, rslt, settings.HideUntriedCards);
                        this.OnConvertFinished(new ThConverterEventArgs(result, index + 1, numFiles));
                    }
                }

                this.OnConvertAllFinished(new ThConverterEventArgs(string.Empty, 0, 0));
            }
        }

        /// <summary>
        /// Raises the event indicating the conversion process per file has finished.
        /// </summary>
        /// <param name="e">The event data issued by the <see cref="ThConverter"/> class.</param>
        private void OnConvertFinished(ThConverterEventArgs e)
        {
            var handler = this.ConvertFinished;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the event indicating the all conversion process has finished.
        /// </summary>
        /// <param name="e">The event data issued by the <see cref="ThConverter"/> class.</param>
        private void OnConvertAllFinished(ThConverterEventArgs e)
        {
            var handler = this.ConvertAllFinished;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the event indicating an exception has occurred.
        /// </summary>
        /// <param name="e">The event data that indicates occurring of an exception.</param>
        private void OnExceptionOccurred(ExceptionOccurredEventArgs e)
        {
            var handler = this.ExceptionOccurred;
            if (handler != null)
                handler(this, e);
        }
    }
}
