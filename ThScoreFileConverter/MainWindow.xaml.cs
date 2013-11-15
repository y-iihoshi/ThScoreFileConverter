using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace ThScoreFileConverter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 全設定
        /// </summary>
        private Settings settings = null;

        /// <summary>
        /// 変換処理を行うクラスインスタンス
        /// </summary>
        private ThConverter converter = null;

        /// <summary>
        /// 数値を桁区切り形式で出力する場合 true
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
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                this.settings = new Settings();
                try
                {
                    this.settings.Load(Properties.Resources.strSettingFile);
                }
                catch (InvalidDataException)
                {
                    var backup = Path.ChangeExtension(
                        Properties.Resources.strSettingFile,
                        Properties.Resources.strBackupFileExtension);
                    File.Delete(backup);
                    File.Move(Properties.Resources.strSettingFile, backup);
                    var message = string.Format(
                        Properties.Resources.msgFmtBrokenSettingFile,
                        Properties.Resources.strSettingFile, backup);
                    MessageBox.Show(
                        message, Properties.Resources.msgTitleWarning,
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                var lastTitleItem = Utils.Find<ComboBoxItem>(
                    this.cmbTitle.Items, new Predicate<ComboBoxItem>(
                        item => (item.Name == this.settings.LastTitle)));
                if (lastTitleItem != null)
                    lastTitleItem.IsSelected = true;
                else
                {
                    var firstEnabledItem = Utils.Find<ComboBoxItem>(
                        this.cmbTitle.Items, new Predicate<ComboBoxItem>(item => item.IsEnabled));
                    if (firstEnabledItem != null)
                        firstEnabledItem.IsSelected = true;
                }
                ((App)App.Current).UpdateResources(settings.FontFamilyName, settings.FontSize);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// メインウィンドウの終了前処理
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void wndMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.UpdateSettingsFromControls((ComboBoxItem)this.cmbTitle.SelectedItem);
                this.settings.FontFamilyName = App.Current.Resources["FontFamilyKey"].ToString();
                this.settings.FontSize = Convert.ToDouble(App.Current.Resources["FontSizeKey"]);
                this.settings.Save(Properties.Resources.strSettingFile);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        #region 作品名コンボボックス

        /// <summary>
        /// 作品の選択状況の変化
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cmbTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
                this.UpdateSettingsFromControls((ComboBoxItem)e.RemovedItems[0]);

            if (e.AddedItems.Count > 0)
            {
                var item = (ComboBoxItem)e.AddedItems[0];

                this.converter = ThConverterFactory.Create(item.Name);
                this.converter.OutputNumberGroupSeparator = this.settings.OutputNumberGroupSeparator.Value;
                this.converter.ConvertFinished += ThConverter_ConvertFinished;
                this.converter.ConvertAllFinished += ThConverter_ConvertAllFinished;
                this.converter.ExceptionOccurred += ThConverter_ExceptionOccurred;

                this.settings.LastTitle = item.Name;
                this.UpdateControlsFromSettings(item);
            }
        }

        #endregion

        #region スコアファイル

        /// <summary>
        /// スコアファイルの選択
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnScore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.OpenFileDialog();
                dialog.Filter = Properties.Resources.fltScoreFile;
                if (this.txtScore.Text.Length > 0)
                    dialog.InitialDirectory = Path.GetDirectoryName(this.txtScore.Text);

                var result = dialog.ShowDialog(new Win32Window(this));
                if (result == WinForms.DialogResult.OK)
                    this.txtScore.Text = dialog.FileName;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// スコアファイル欄へのドラッグ中
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtScore_Dragging(object sender, DragEventArgs e)
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
            }
        }

        /// <summary>
        /// スコアファイル欄へのドロップ
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtScore_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var scorePath = Array.Find<string>(
                    (string[])e.Data.GetData(DataFormats.FileDrop),
                    new Predicate<string>(path => File.Exists(path)));
                if (scorePath != null)
                    this.txtScore.Text = scorePath;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// スコアファイル欄内の変化
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateBtnConvertIsEnabled();
        }

        #endregion

        #region ベストショットディレクトリ

        /// <summary>
        /// ベストショットディレクトリの選択
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnBestShot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.FolderBrowserDialog();
                dialog.Description = Properties.Resources.msgSelectBestShotDirectory;
                if (Directory.Exists(this.txtBestShot.Text))
                    dialog.SelectedPath = this.txtBestShot.Text;

                var result = dialog.ShowDialog(new Win32Window(this));
                if (result == WinForms.DialogResult.OK)
                    this.txtBestShot.Text = dialog.SelectedPath;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// ベストショットディレクトリ欄へのドラッグ中
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtBestShot_Dragging(object sender, DragEventArgs e)
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
            }
        }

        /// <summary>
        /// ベストショットディレクトリ欄へのドロップ
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtBestShot_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var bestShotPath = Array.Find<string>(
                    (string[])e.Data.GetData(DataFormats.FileDrop),
                    new Predicate<string>(path => Directory.Exists(path)));
                if (bestShotPath != null)
                    this.txtBestShot.Text = bestShotPath;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// ベストショットディレクトリ欄内の変化
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtBestShot_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateBtnConvertIsEnabled();
        }

        #endregion

        #region テンプレートファイル

        /// <summary>
        /// テンプレートファイルの選択
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnTemplateAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.OpenFileDialog();
                dialog.Filter = Properties.Resources.fltTemplateFile;
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
            }
        }

        /// <summary>
        /// テンプレートファイルの選択解除
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnTemplateClear_Click(object sender, RoutedEventArgs e)
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
            }
        }

        /// <summary>
        /// テンプレートファイルの一括選択解除
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnTemplateClearAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.lstTemplate.Items.Clear();
                this.UpdateBtnConvertIsEnabled();
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// テンプレートファイル一覧へのドラッグ中
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void lstTemplate_Dragging(object sender, DragEventArgs e)
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
            }
        }

        /// <summary>
        /// テンプレートファイル一覧へのドロップ
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void lstTemplate_Drop(object sender, DragEventArgs e)
        {
            try
            {
                foreach (var filename in (string[])(e.Data.GetData(DataFormats.FileDrop, false)))
                    if (File.Exists(filename) && !this.lstTemplate.Items.Contains(filename))
                        this.lstTemplate.Items.Add(filename);
                this.UpdateBtnConvertIsEnabled();
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// テンプレートファイル一覧の選択状況の変化
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void lstTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnTemplateClear.IsEnabled = (this.lstTemplate.SelectedItems.Count > 0);
        }

        #endregion

        #region 出力先ディレクトリ

        /// <summary>
        /// 出力先ディレクトリの選択
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnOutput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new WinForms.FolderBrowserDialog();
                dialog.Description = Properties.Resources.msgSelectOutputDirectory;
                if (Directory.Exists(this.txtOutput.Text))
                    dialog.SelectedPath = this.txtOutput.Text;

                var result = dialog.ShowDialog(new Win32Window(this));
                if (result == WinForms.DialogResult.OK)
                    this.txtOutput.Text = dialog.SelectedPath;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// 出力先ディレクトリ欄へのドラッグ中
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtOutput_Dragging(object sender, DragEventArgs e)
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
            }
        }

        /// <summary>
        /// 出力先ディレクトリ欄へのドロップ
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtOutput_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var outputPath = Array.Find<string>(
                    (string[])e.Data.GetData(DataFormats.FileDrop),
                    new Predicate<string>(path => Directory.Exists(path)));
                if (outputPath != null)
                    this.txtOutput.Text = outputPath;
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// 出力先ディレクトリ欄内の変化
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateBtnConvertIsEnabled();
        }

        #endregion

        /// <summary>
        /// 変換開始
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ChangeCursor(Cursors.Wait);
                this.SetAllControlsEnabled(false);

                this.txtLog.Clear();
                this.AddLogLine(Properties.Resources.msgStartConversion);

                var selectedItem = (ComboBoxItem)this.cmbTitle.SelectedItem;
                this.UpdateSettingsFromControls(selectedItem);

                new Thread(new ParameterizedThreadStart(this.converter.Convert)).Start(
                    this.settings.Dictionary[selectedItem.Name]);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// ファイル毎の変換処理完了
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void ThConverter_ConvertFinished(object sender, ThConverterEventArgs e)
        {
            this.AddLogLine(e.Message);
        }

        /// <summary>
        /// 全ての変換処理完了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThConverter_ConvertAllFinished(object sender, ThConverterEventArgs e)
        {
            this.AddLogLine(Properties.Resources.msgEndConversion);
            this.SetAllControlsEnabled(true);
            this.ChangeCursor(null);
        }

        /// <summary>
        /// 変換処理での例外発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThConverter_ExceptionOccurred(object sender, ExceptionOccurredEventArgs e)
        {
            this.ShowExceptionMessage(e.Exception);
            this.SetAllControlsEnabled(true);
            this.ChangeCursor(null);
        }

        /// <summary>
        /// 設定ダイアログの表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            new SettingWindow(this).ShowDialog();
        }

        /// <summary>
        /// About ダイアログの表示
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow(this).ShowDialog();
        }

        #region Utility

        /// <summary>
        /// ログ出力用テキストボックスに 1 行出力する
        /// </summary>
        /// <param Name="log">出力するログ</param>
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
        /// マウスカーソルを変更する
        /// </summary>
        /// <param Name="cursor"></param>
        private void ChangeCursor(Cursor cursor)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher.CheckAccess())
                Mouse.OverrideCursor = cursor;
            else
                dispatcher.Invoke(new Action<Cursor>(this.ChangeCursor), cursor);
        }

        /// <summary>
        /// 全てのコントロールの IsEnabled プロパティを一括設定する
        /// </summary>
        /// <param Name="isEnabled"></param>
        private void SetAllControlsEnabled(bool isEnabled)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher.CheckAccess())
                this.IsEnabled = isEnabled;
            else
                dispatcher.Invoke(new Action<bool>(this.SetAllControlsEnabled), isEnabled);
        }

        /// <summary>
        /// 例外発生時のメッセージを表示する
        /// </summary>
        /// <param name="message"></param>
        private void ShowExceptionMessage(Exception e)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher.CheckAccess())
                MessageBox.Show(
#if DEBUG
                    e.ToString(),
#else
                    e.Message,
#endif
                    Properties.Resources.msgTitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                dispatcher.Invoke(new Action<Exception>(this.ShowExceptionMessage), e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
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
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void UpdateControlsFromSettings(ComboBoxItem item)
        {
            if (!this.settings.Dictionary.ContainsKey(item.Name))
                this.settings.Dictionary.Add(item.Name, new SettingsPerTitle());

            this.txtScore.Clear();
            this.lblSupportedVersion.Content = "";
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
                    Properties.Resources.strSupportedVersions + this.converter.SupportedVersions;
            if (this.txtBestShot.IsEnabled && Directory.Exists(entry.BestShotDirectory))
                this.txtBestShot.Text = entry.BestShotDirectory;
            foreach (var template in entry.TemplateFiles)
                if (File.Exists(template))
                    this.lstTemplate.Items.Add(template);
            if (Directory.Exists(entry.OutputDirectory))
                this.txtOutput.Text = entry.OutputDirectory;
            if (this.txtImageOutput.IsEnabled)
                this.txtImageOutput.Text = (entry.ImageOutputDirectory != "")
                    ? entry.ImageOutputDirectory : Properties.Resources.strBestShotDirectory;

            ((App)App.Current).UpdateResources(this.settings.FontFamilyName, this.settings.FontSize);
        }

        /// <summary>
        /// スコアファイル変換ボタンの有効・無効状態の更新
        /// </summary>
        private void UpdateBtnConvertIsEnabled()
        {
            btnConvert.IsEnabled = (
                (this.txtScore.Text.Length > 0) &&
                this.lstTemplate.HasItems &&
                (this.txtOutput.Text.Length > 0) &&
                (!this.txtBestShot.IsEnabled || (this.txtBestShot.Text.Length > 0)) &&
                (!this.txtImageOutput.IsEnabled || (this.txtImageOutput.Text.Length > 0)));
        }

        #endregion
    }
}
