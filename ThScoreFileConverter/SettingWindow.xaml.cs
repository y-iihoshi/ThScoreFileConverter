using System;
using System.Windows;
using System.Windows.Input;
using SysDraw = System.Drawing;
using WinForms = System.Windows.Forms;

namespace ThScoreFileConverter
{
    /// <summary>
    /// ConfigWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        /// <summary>
        /// フォント設定ダイアログのインスタンス
        /// </summary>
        private WinForms.FontDialog fontDialog = null;

        private SettingWindow()
        {
            InitializeComponent();

            this.fontDialog = new WinForms.FontDialog();
            this.fontDialog.ShowApply = true;
            this.fontDialog.Apply += fontDialog_Apply;
            this.fontDialog.FontMustExist = true;
            this.fontDialog.ShowEffects = false;

            this.chkOutputNumberGroupSeparator.Click += chkOutputNumberGroupSeparator_Click;
        }

        public SettingWindow(Window owner)
            : this()
        {
            this.Owner = owner;
            this.chkOutputNumberGroupSeparator.IsChecked = ((MainWindow)owner).OutputNumberGroupSeparator;
        }

        #region フォント設定

        /// <summary>
        /// フォントの変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontChange_Click(object sender, RoutedEventArgs e)
        {
            this.fontDialog.Font = new SysDraw.Font(
                App.Current.Resources["FontFamilyKey"].ToString(),
                Convert.ToSingle(App.Current.Resources["FontSizeKey"]));

            var oldFont = this.fontDialog.Font;
            var result = this.fontDialog.ShowDialog(new Win32Window(this));

            switch (result)
            {
                case WinForms.DialogResult.OK:
                    fontDialog_Apply(sender, e);
                    break;
                case WinForms.DialogResult.Cancel:
                    ((App)App.Current).UpdateResources(oldFont);
                    break;
            }

            this.Focus();
        }

        /// <summary>
        /// フォント変更ダイアログによるフォント設定の一時適用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontDialog_Apply(object sender, EventArgs e)
        {
            ((App)App.Current).UpdateResources(this.fontDialog.Font);
        }

        /// <summary>
        /// フォント設定のリセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontReset_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize);
        }

        #endregion

        #region 出力書式設定

        /// <summary>
        /// 「数値を桁区切り形式で出力する」チェックボックスのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chkOutputNumberGroupSeparator_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)this.Owner).OutputNumberGroupSeparator =
                this.chkOutputNumberGroupSeparator.IsChecked.Value;
        }

        #endregion

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
