//-----------------------------------------------------------------------
// <copyright file="ThConverter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
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
        public event EventHandler<ThConverterEventArgs>? ConvertFinished;

        /// <summary>
        /// Represents the event that all conversion process has finished.
        /// </summary>
        public event EventHandler<ThConverterEventArgs>? ConvertAllFinished;

        /// <summary>
        /// Represents the event that an exception has occurred.
        /// </summary>
        public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;

        /// <summary>
        /// Gets the string indicating the supported versions of the score file to convert.
        /// </summary>
        /// <remarks>It is required to override this property by a subclass.</remarks>
        public virtual string SupportedVersions { get; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether the current instance has the conversion method for best-shot
        /// files or not.
        /// </summary>
        /// <remarks>
        /// It is required to override this property by the subclass that implements the conversion process
        /// of best-shot files.
        /// </remarks>
        public virtual bool HasBestShotConverter { get; } = false;

        /// <summary>
        /// Gets a value indicating whether the current instance has the replace method for spell card
        /// information or not.
        /// </summary>
        /// <remarks>
        /// It is required to override this property by the subclass that does not have such replace method.
        /// </remarks>
        public virtual bool HasCardReplacer { get; } = true;

        /// <summary>
        /// Converts a score file.
        /// </summary>
        /// <param name="threadArg">An instance of the <see cref="SettingsPerTitle"/> class.</param>
        public void Convert(object? threadArg)
        {
            try
            {
                if (threadArg is null)
                    throw new ArgumentNullException(nameof(threadArg));

                if (threadArg is SettingsPerTitle settings)
                    this.Convert(settings);
                else
                    throw new ArgumentException(Resources.ArgumentExceptionWrongType, nameof(threadArg));
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
            const int DefaultBufferSize = 1024;

            using var reader = new StreamReader(
                input, Encoding.GetEncoding(Settings.Instance.InputCodePageId!.Value), true, DefaultBufferSize, true);
            using var writer = new StreamWriter(
                output, Encoding.GetEncoding(Settings.Instance.OutputCodePageId!.Value), DefaultBufferSize, true);

            var outputFilePath = (output is FileStream outputFile) ? outputFile.Name : string.Empty;
            var replacers = this.CreateReplacers(hideUntriedCards, outputFilePath);

            var allLines = reader.ReadToEnd();

            foreach (var replacer in replacers)
                allLines = replacer.Replace(allLines);

            writer.Write(allLines);
            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
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
        /// Creates the instances which implement IStringReplaceable interface.
        /// </summary>
        /// <param name="hideUntriedCards"><c>true</c> if it hides untried spell cards.</param>
        /// <param name="outputFilePath">The file path for outputting the converted data.</param>
        /// <returns>The created instances which implement IStringReplaceable interface.</returns>
        protected virtual IEnumerable<IStringReplaceable> CreateReplacers(bool hideUntriedCards, string outputFilePath)
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
            outputFile += Resources.ConvertedBestShotFileExtension;

            return outputFile;
        }

        /// <summary>
        /// Converts a score file.
        /// </summary>
        /// <param name="settings">The settings per work.</param>
        private void Convert(SettingsPerTitle settings)
        {
            using var scr = new FileStream(settings.ScoreFile, FileMode.Open, FileAccess.Read);
            _ = scr.Seek(0, SeekOrigin.Begin);
            if (!this.ReadScoreFile(scr))
                throw new NotSupportedException(Resources.MessageFailedToReadScoreFile);

            if (this.HasBestShotConverter)
            {
                var dir = Path.Combine(settings.OutputDirectory, settings.ImageOutputDirectory);
                if (!Directory.Exists(dir))
                    _ = Directory.CreateDirectory(dir);
                var files = this.FilterBestShotFiles(
                    Directory.GetFiles(settings.BestShotDirectory, Resources.BestShotFilePattern));
                for (var index = 0; index < files.Length; index++)
                {
                    var result = GetBestShotFilePath(files[index], dir);
                    using var bsts = new FileStream(files[index], FileMode.Open, FileAccess.Read);
                    using var rslt = new FileStream(result, FileMode.OpenOrCreate, FileAccess.Write);
                    this.ConvertBestShot(bsts, rslt);
                    this.OnConvertFinished(new ThConverterEventArgs(result, index + 1, files.Length));
                }
            }

            var numFiles = settings.TemplateFiles.Count();
            for (var index = 0; index < numFiles; index++)
            {
                var template = settings.TemplateFiles.ElementAt(index);
                var result = GetOutputFilePath(template, settings.OutputDirectory);
                using var tmpl = new FileStream(template, FileMode.Open, FileAccess.Read);
                using var rslt = new FileStream(result, FileMode.OpenOrCreate, FileAccess.Write);
                this.Convert(tmpl, rslt, settings.HideUntriedCards);
                this.OnConvertFinished(new ThConverterEventArgs(result, index + 1, numFiles));
            }

            this.OnConvertAllFinished(new ThConverterEventArgs());
        }

        /// <summary>
        /// Raises the event indicating the conversion process per file has finished.
        /// </summary>
        /// <param name="e">The event data issued by the <see cref="ThConverter"/> class.</param>
        private void OnConvertFinished(ThConverterEventArgs e)
        {
            this.ConvertFinished?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the event indicating the all conversion process has finished.
        /// </summary>
        /// <param name="e">The event data issued by the <see cref="ThConverter"/> class.</param>
        private void OnConvertAllFinished(ThConverterEventArgs e)
        {
            this.ConvertAllFinished?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the event indicating an exception has occurred.
        /// </summary>
        /// <param name="e">The event data that indicates occurring of an exception.</param>
        private void OnExceptionOccurred(ExceptionOccurredEventArgs e)
        {
            this.ExceptionOccurred?.Invoke(this, e);
        }
    }
}
