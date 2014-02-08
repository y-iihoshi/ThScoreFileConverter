//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Prop = ThScoreFileConverter.Properties;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Represents the all settings for this application.
        /// </summary>
        private Settings settings = null;

        /// <summary>
        /// The instance that executes a conversion process.
        /// </summary>
        private ThConverter converter = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            try
            {
                this.settings = new Settings();
                try
                {
                    this.settings.Load(Prop.Resources.strSettingFile);
                }
                catch (InvalidDataException)
                {
                    var backup = Path.ChangeExtension(
                        Prop.Resources.strSettingFile, Prop.Resources.strBackupFileExtension);
                    File.Delete(backup);
                    File.Move(Prop.Resources.strSettingFile, backup);
                    var message = Utils.Format(
                        Prop.Resources.msgFmtBrokenSettingFile, Prop.Resources.strSettingFile, backup);
                    MessageBox.Show(
                        message, Prop.Resources.msgTitleWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                var lastTitleItem = this.cmbTitle.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Name == this.settings.LastTitle);
                if (lastTitleItem != null)
                    lastTitleItem.IsSelected = true;
                else
                {
                    var firstEnabledItem = this.cmbTitle.Items.Cast<ComboBoxItem>()
                        .FirstOrDefault(item => item.IsEnabled);
                    if (firstEnabledItem != null)
                        firstEnabledItem.IsSelected = true;
                }

                ((App)App.Current).UpdateResources(this.settings.FontFamilyName, this.settings.FontSize);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether thousand separator characters are contained in the
        /// string that represents a numeric value.
        /// </summary>
        public bool OutputNumberGroupSeparator
        {
            get
            {
                return ((this.settings != null) && this.settings.OutputNumberGroupSeparator.HasValue)
                    ? this.settings.OutputNumberGroupSeparator.Value : true;
            }

            set
            {
                if (this.settings != null)
                    this.settings.OutputNumberGroupSeparator = value;
                if (this.converter != null)
                    this.converter.OutputNumberGroupSeparator = value;
            }
        }

        /// <summary>
        /// Handles the closing event of this window.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void WndMain_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                this.UpdateSettingsFromControls((ComboBoxItem)this.cmbTitle.SelectedItem);
                this.settings.FontFamilyName = App.Current.Resources["FontFamilyKey"].ToString();
                this.settings.FontSize = Convert.ToDouble(App.Current.Resources["FontSizeKey"]);
                this.settings.Save(Prop.Resources.strSettingFile);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        #region Work combo box

        /// <summary>
        /// Handles the <c>SelectionChanged</c> routed event of the <see cref="cmbTitle"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void CmbTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
                this.UpdateSettingsFromControls((ComboBoxItem)e.RemovedItems[0]);

            if (e.AddedItems.Count > 0)
            {
                var item = (ComboBoxItem)e.AddedItems[0];

                this.converter = ThConverterFactory.Create(item.Name);
                this.converter.OutputNumberGroupSeparator = this.settings.OutputNumberGroupSeparator.Value;
                this.converter.ConvertFinished += this.ThConverter_ConvertFinished;
                this.converter.ConvertAllFinished += this.ThConverter_ConvertAllFinished;
                this.converter.ExceptionOccurred += this.ThConverter_ExceptionOccurred;

                this.settings.LastTitle = item.Name;
                this.UpdateControlsFromSettings(item);
            }
        }

        #endregion

        #region Score file

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnScore"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnScore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.OpenFileDialog();
                dialog.Filter = Prop.Resources.fltScoreFile;
                if (this.txtScore.Text.Length > 0)
                    dialog.InitialDirectory = Path.GetDirectoryName(this.txtScore.Text);

                var result = dialog.ShowDialog(new Win32Window(this));
                if (result == WinForms.DialogResult.OK)
                    this.txtScore.Text = dialog.FileName;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>PreviewDragEnter</c> and <c>PreviewDragOver</c> routed event of the
        /// <see cref="txtScore"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtScore_Dragging(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else
                    e.Effects = DragDropEffects.None;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>Drop</c> routed event of the <see cref="txtScore"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtScore_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var droppedPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                    var scorePath = droppedPaths.FirstOrDefault(path => File.Exists(path));
                    if (scorePath != null)
                        this.txtScore.Text = scorePath;
                }
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>TextChanged</c> routed event of the <see cref="txtScore"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateBtnConvertIsEnabled();
        }

        #endregion

        #region Best shot directory

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnBestShot"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnBestShot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.FolderBrowserDialog();
                dialog.Description = Prop.Resources.msgSelectBestShotDirectory;
                if (Directory.Exists(this.txtBestShot.Text))
                    dialog.SelectedPath = this.txtBestShot.Text;

                var result = dialog.ShowDialog(new Win32Window(this));
                if (result == WinForms.DialogResult.OK)
                    this.txtBestShot.Text = dialog.SelectedPath;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>PreviewDragEnter</c> and <c>PreviewDragOver</c> routed event of the
        /// <see cref="txtBestShot"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtBestShot_Dragging(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else
                    e.Effects = DragDropEffects.None;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>Drop</c> routed event of the <see cref="txtBestShot"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtBestShot_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var droppedPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                    var bestShotPath = droppedPaths.FirstOrDefault(path => Directory.Exists(path));
                    if (bestShotPath != null)
                        this.txtBestShot.Text = bestShotPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>TextChanged</c> routed event of the <see cref="txtBestShot"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtBestShot_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateBtnConvertIsEnabled();
        }

        #endregion

        #region Template files

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnTemplateAdd"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnTemplateAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.OpenFileDialog();
                dialog.Filter = Prop.Resources.fltTemplateFile;
                dialog.Multiselect = true;
                if (this.lstTemplate.Items.Count > 0)
                {
                    var templatePath = (string)this.lstTemplate.Items[this.lstTemplate.Items.Count - 1];
                    if (templatePath.Length > 0)
                        dialog.InitialDirectory = Path.GetDirectoryName(templatePath);
                }

                var result = dialog.ShowDialog(new Win32Window(this));
                if (result == WinForms.DialogResult.OK)
                {
                    foreach (var name in dialog.FileNames)
                        if (!this.lstTemplate.Items.Contains(name))
                            this.lstTemplate.Items.Add(name);
                    this.UpdateBtnConvertIsEnabled();
                }
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnTemplateClear"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnTemplateClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (var index = this.lstTemplate.SelectedItems.Count - 1; index >= 0; index--)
                    this.lstTemplate.Items.Remove(this.lstTemplate.SelectedItems[index]);
                this.UpdateBtnConvertIsEnabled();
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnTemplateClearAll"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnTemplateClearAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.lstTemplate.Items.Clear();
                this.UpdateBtnConvertIsEnabled();
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>PreviewDragEnter</c> and <c>PreviewDragOver</c> routed event of the
        /// <see cref="lstTemplate"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void LstTemplate_Dragging(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else
                    e.Effects = DragDropEffects.None;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>Drop</c> routed event of the <see cref="lstTemplate"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void LstTemplate_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var droppedPaths = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                    foreach (var path in droppedPaths)
                        if (File.Exists(path) && !this.lstTemplate.Items.Contains(path))
                            this.lstTemplate.Items.Add(path);
                    this.UpdateBtnConvertIsEnabled();
                }
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>SelectionChanged</c> routed event of the <see cref="lstTemplate"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void LstTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnTemplateClear.IsEnabled = this.lstTemplate.SelectedItems.Count > 0;
        }

        #endregion

        #region Output directory

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnOutput"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnOutput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.FolderBrowserDialog();
                dialog.Description = Prop.Resources.msgSelectOutputDirectory;
                if (Directory.Exists(this.txtOutput.Text))
                    dialog.SelectedPath = this.txtOutput.Text;

                var result = dialog.ShowDialog(new Win32Window(this));
                if (result == WinForms.DialogResult.OK)
                    this.txtOutput.Text = dialog.SelectedPath;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>PreviewDragEnter</c> and <c>PreviewDragOver</c> routed event of the
        /// <see cref="txtOutput"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtOutput_Dragging(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else
                    e.Effects = DragDropEffects.None;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>Drop</c> routed event of the <see cref="txtOutput"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtOutput_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var droppedPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                    var outputPath = droppedPaths.FirstOrDefault(path => Directory.Exists(path));
                    if (outputPath != null)
                        this.txtOutput.Text = outputPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the <c>TextChanged</c> routed event of the <see cref="txtOutput"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void TxtOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateBtnConvertIsEnabled();
        }

        #endregion

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnConvert"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ChangeCursor(Cursors.Wait);
                this.SetAllControlsEnabled(false);

                this.txtLog.Clear();
                this.AddLogLine(Prop.Resources.msgStartConversion);

                var selectedItem = (ComboBoxItem)this.cmbTitle.SelectedItem;
                this.UpdateSettingsFromControls(selectedItem);

                new Thread(new ParameterizedThreadStart(this.converter.Convert)).Start(
                    this.settings.Dictionary[selectedItem.Name]);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the event indicating the conversion process per file has finished.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ThConverter_ConvertFinished(object sender, ThConverterEventArgs e)
        {
            this.AddLogLine(e.Message);
        }

        /// <summary>
        /// Handles the event indicating the all conversion process has finished.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ThConverter_ConvertAllFinished(object sender, ThConverterEventArgs e)
        {
            this.AddLogLine(Prop.Resources.msgEndConversion);
            this.SetAllControlsEnabled(true);
            this.ChangeCursor(null);
        }

        /// <summary>
        /// Handles the event indicating an exception has occurred.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ThConverter_ExceptionOccurred(object sender, ExceptionOccurredEventArgs e)
        {
            this.ShowExceptionMessage(e.Exception);
            this.AddLogLine(Prop.Resources.msgErrUnhandledException);
            this.SetAllControlsEnabled(true);
            this.ChangeCursor(null);
        }

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnSetting"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            new SettingWindow(this).ShowDialog();
        }

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnAbout"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow(this).ShowDialog();
        }

        #region Utility

        /// <summary>
        /// Outputs one line to the text box for logging.
        /// </summary>
        /// <param name="log">The log text to output.</param>
        private void AddLogLine(string log)
        {
            var dispatcher = this.txtLog.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                this.txtLog.AppendText(log + "\n");
                this.txtLog.ScrollToEnd();
            }
            else
                dispatcher.Invoke(new Action<string>(this.AddLogLine), log);
        }

        /// <summary>
        /// Changes the mouse cursor.
        /// </summary>
        /// <param name="cursor">The <see cref="Cursor"/> instance to set.</param>
        private void ChangeCursor(Cursor cursor)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher.CheckAccess())
                Mouse.OverrideCursor = cursor;
            else
                dispatcher.Invoke(new Action<Cursor>(this.ChangeCursor), cursor);
        }

        /// <summary>
        /// Sets the <c>IsEnabled</c> properties of all controls.
        /// </summary>
        /// <param name="isEnabled">The value to set.</param>
        private void SetAllControlsEnabled(bool isEnabled)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher.CheckAccess())
                this.IsEnabled = isEnabled;
            else
                dispatcher.Invoke(new Action<bool>(this.SetAllControlsEnabled), isEnabled);
        }

        /// <summary>
        /// Shows a message that represents the occurred exception.
        /// </summary>
        /// <param name="e">The occurred exception.</param>
        private void ShowExceptionMessage(Exception e)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher.CheckAccess())
#if DEBUG
                MessageBox.Show(
                    e.ToString(), Prop.Resources.msgTitleError, MessageBoxButton.OK, MessageBoxImage.Error);
#else
                MessageBox.Show(
                    e.Message, Prop.Resources.msgTitleError, MessageBoxButton.OK, MessageBoxImage.Error);
#endif
            else
                dispatcher.Invoke(new Action<Exception>(this.ShowExceptionMessage), e);
        }

        /// <summary>
        /// Updates the settings of this application by statuses of the controls on this window.
        /// </summary>
        /// <param name="item">The currently selected item of the "Work" combo box.</param>
        private void UpdateSettingsFromControls(ComboBoxItem item)
        {
            if (!this.settings.Dictionary.ContainsKey(item.Name))
                this.settings.Dictionary.Add(item.Name, new SettingsPerTitle());

            var entry = this.settings.Dictionary[item.Name];
            entry.ScoreFile = this.txtScore.Text;
            entry.BestShotDirectory = this.txtBestShot.Text;
            entry.TemplateFiles = new string[this.lstTemplate.Items.Count];
            entry.OutputDirectory = this.txtOutput.Text;
            entry.ImageOutputDirectory = this.txtImageOutput.Text;
            this.lstTemplate.Items.CopyTo(entry.TemplateFiles, 0);
        }

        /// <summary>
        /// Updates the controls on this window by the settings of this application.
        /// </summary>
        /// <param name="item">The currently selected item of the "Work" combo box.</param>
        private void UpdateControlsFromSettings(ComboBoxItem item)
        {
            if (!this.settings.Dictionary.ContainsKey(item.Name))
                this.settings.Dictionary.Add(item.Name, new SettingsPerTitle());

            this.txtScore.Clear();
            this.lblSupportedVersion.Content = string.Empty;
            this.txtBestShot.Clear();
            this.lstTemplate.Items.Clear();
            this.txtOutput.Clear();
            this.txtImageOutput.Clear();
            this.txtLog.Clear();

            if (this.converter.HasBestShotConverter)
            {
                this.lblBestShot.IsEnabled = true;
                this.txtBestShot.IsEnabled = true;
                this.btnBestShot.IsEnabled = true;
                this.lblImageOutput.IsEnabled = true;
                this.txtImageOutput.IsEnabled = true;
            }
            else
            {
                this.lblBestShot.IsEnabled = false;
                this.txtBestShot.IsEnabled = false;
                this.btnBestShot.IsEnabled = false;
                this.lblImageOutput.IsEnabled = false;
                this.txtImageOutput.IsEnabled = false;
            }

            var entry = this.settings.Dictionary[item.Name];
            if (File.Exists(entry.ScoreFile))
                this.txtScore.Text = entry.ScoreFile;
            if (this.converter != null)
                this.lblSupportedVersion.Content =
                    Prop.Resources.strSupportedVersions + this.converter.SupportedVersions;
            if (this.txtBestShot.IsEnabled && Directory.Exists(entry.BestShotDirectory))
                this.txtBestShot.Text = entry.BestShotDirectory;
            foreach (var template in entry.TemplateFiles)
                if (File.Exists(template))
                    this.lstTemplate.Items.Add(template);
            if (Directory.Exists(entry.OutputDirectory))
                this.txtOutput.Text = entry.OutputDirectory;
            if (this.txtImageOutput.IsEnabled)
                this.txtImageOutput.Text = (entry.ImageOutputDirectory != string.Empty)
                    ? entry.ImageOutputDirectory : Prop.Resources.strBestShotDirectory;

            ((App)App.Current).UpdateResources(this.settings.FontFamilyName, this.settings.FontSize);
        }

        /// <summary>
        /// Updates the value of <see cref="btnConvert"/><c>.IsEnabled</c>.
        /// </summary>
        private void UpdateBtnConvertIsEnabled()
        {
            btnConvert.IsEnabled =
                (this.txtScore.Text.Length > 0) &&
                this.lstTemplate.HasItems &&
                (this.txtOutput.Text.Length > 0) &&
                (!this.txtBestShot.IsEnabled || (this.txtBestShot.Text.Length > 0)) &&
                (!this.txtImageOutput.IsEnabled || (this.txtImageOutput.Text.Length > 0));
        }

        #endregion
    }
}
