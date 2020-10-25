//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Prism.Ioc;
using Prism.Unity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.ViewModels;
using ThScoreFileConverter.Views;
using ThScoreFileConverter.Wrappers;
using Unity;
using WPFLocalizeExtension.Engine;
using Prop = ThScoreFileConverter.Properties;

namespace ThScoreFileConverter
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// Gets the font family used for the UI of this application.
        /// </summary>
        public FontFamily FontFamily
        {
            get => (FontFamily)this.Resources["FontFamilyKey"];
            private set => this.Resources["FontFamilyKey"] = value;
        }

        /// <summary>
        /// Gets the font size used for the UI of this application.
        /// </summary>
        public double FontSize
        {
            get => (double)this.Resources["FontSizeKey"];
            private set => this.Resources["FontSizeKey"] = value;
        }

        /// <summary>
        /// Updates the resources of this application.
        /// </summary>
        /// <param name="fontFamily">The font family used for the UI of this application.</param>
        /// <param name="fontSize">The font size used for the UI of this application.</param>
        public void UpdateResources(FontFamily fontFamily, double? fontSize)
        {
            if (fontFamily is { })
                this.FontFamily = fontFamily;
            if (fontSize.HasValue)
                this.FontSize = fontSize.Value;
        }

        /// <summary>
        /// Updates the resources of this application.
        /// </summary>
        /// <param name="fontFamilyName">The font family name used for the UI of this application.</param>
        /// <param name="fontSize">The font size used for the UI of this application.</param>
        public void UpdateResources(string fontFamilyName, double? fontSize)
        {
            this.UpdateResources(new FontFamily(fontFamilyName), fontSize);
        }

        /// <inheritdoc/>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
#if DEBUG
            if (containerRegistry.GetContainer() is UnityContainer container)
                container.EnableDebugDiagnostic();
#endif

            _ = containerRegistry.RegisterInstance<ISettings>(Settings.Instance);
            _ = containerRegistry.Register<IDispatcherAdapter, DispatcherAdapter>();
            containerRegistry.RegisterDialog<AboutWindow>(nameof(AboutWindowViewModel));
            containerRegistry.RegisterDialog<SettingWindow>(nameof(SettingWindowViewModel));
        }

        /// <inheritdoc/>
        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Settings.Instance.Load(Prop.Resources.SettingFileName);
            }
            catch (InvalidDataException)
            {
                var backup = Path.ChangeExtension(Prop.Resources.SettingFileName, Prop.Resources.BackupFileExtension);
                File.Delete(backup);
                File.Move(Prop.Resources.SettingFileName, backup);
                var message = Utils.Format(
                    Utils.GetLocalizedValues<string>(nameof(Prop.Resources.MessageSettingFileIsCorrupted)),
                    Prop.Resources.SettingFileName,
                    backup);
                _ = MessageBox.Show(
                    message,
                    Utils.GetLocalizedValues<string>(nameof(Prop.Resources.MessageTitleWarning)),
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            this.UpdateResources(Settings.Instance.FontFamilyName, Settings.Instance.FontSize);

            var provider = LocalizationProvider.Instance;
            LocalizeDictionary.Instance.SetCurrentValue(LocalizeDictionary.DefaultProviderProperty, provider);
            LocalizeDictionary.Instance.SetCurrentValue(LocalizeDictionary.IncludeInvariantCultureProperty, false);
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            if (provider.AvailableCultures.Any(culture => culture.Name == Settings.Instance.Language))
                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(Settings.Instance.Language!);
            else if (provider.AvailableCultures.Any(culture => culture.Equals(CultureInfo.CurrentCulture)))
                LocalizeDictionary.Instance.Culture = CultureInfo.CurrentCulture;
            else
                LocalizeDictionary.Instance.Culture = provider.AvailableCultures.First();

            base.OnStartup(e);
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Settings.Instance.Language = LocalizeDictionary.Instance.Culture.Name;
            Settings.Instance.FontFamilyName = this.FontFamily.ToString();
            Settings.Instance.FontSize = this.FontSize;

            Settings.Instance.Save(Prop.Resources.SettingFileName);
        }
    }
}
