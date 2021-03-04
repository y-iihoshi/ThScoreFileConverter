//-----------------------------------------------------------------------
// <copyright file="SettingsPerTitle.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ThScoreFileConverter
{
    /// <summary>
    /// Represents settings per work.
    /// </summary>
    public class SettingsPerTitle : INotifyPropertyChanged
    {
        private string scoreFile;
        private string bestShotDirectory;
        private IEnumerable<string> templateFiles;
        private string outputDirectory;
        private string imageOutputDirectory;
        private bool hideUntriedCards;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPerTitle"/> class.
        /// </summary>
        public SettingsPerTitle()
        {
            this.scoreFile = string.Empty;
            this.bestShotDirectory = string.Empty;
            this.templateFiles = new List<string>();
            this.outputDirectory = string.Empty;
            this.imageOutputDirectory = string.Empty;
            this.hideUntriedCards = true;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the path of the score file.
        /// </summary>
        public string ScoreFile
        {
            get => this.scoreFile;
            set => this.SetProperty(ref this.scoreFile, value);
        }

        /// <summary>
        /// Gets or sets the path of the best shot directory.
        /// </summary>
        public string BestShotDirectory
        {
            get => this.bestShotDirectory;
            set => this.SetProperty(ref this.bestShotDirectory, value);
        }

        /// <summary>
        /// Gets or sets the array of the paths of template files.
        /// </summary>
        public IEnumerable<string> TemplateFiles
        {
            get => this.templateFiles;
            set => this.SetProperty(ref this.templateFiles, value);
        }

        /// <summary>
        /// Gets or sets the path of the output directory.
        /// </summary>
        public string OutputDirectory
        {
            get => this.outputDirectory;
            set => this.SetProperty(ref this.outputDirectory, value);
        }

        /// <summary>
        /// Gets or sets the path of the output directory of the image files.
        /// </summary>
        public string ImageOutputDirectory
        {
            get => this.imageOutputDirectory;
            set => this.SetProperty(ref this.imageOutputDirectory, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether it hides untried cards.
        /// </summary>
        public bool HideUntriedCards
        {
            get => this.hideUntriedCards;
            set => this.SetProperty(ref this.hideUntriedCards, value);
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
