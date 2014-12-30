﻿//-----------------------------------------------------------------------
// <copyright file="SettingWindow.xaml.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Views
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;
    using SysDraw = System.Drawing;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window, IDisposable
    {
        /// <summary>
        /// The instance of the <see cref="System.Windows.Forms.FontDialog"/> class.
        /// </summary>
        private WinForms.FontDialog fontDialog = null;

        /// <summary>
        /// The flag that represents whether <see cref="Dispose(bool)"/> has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingWindow"/> class.
        /// </summary>
        /// <param name="owner">The instance of the owner window.</param>
        public SettingWindow(Window owner)
            : this()
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SettingWindow"/> class from being created.
        /// </summary>
        private SettingWindow()
        {
            this.InitializeComponent();

            this.fontDialog = new WinForms.FontDialog();
            this.fontDialog.ShowApply = true;
            this.fontDialog.Apply += this.FontDialog_Apply;
            this.fontDialog.FontMustExist = true;
            this.fontDialog.ShowEffects = false;

            this.disposed = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SettingWindow"/> class.
        /// </summary>
        ~SettingWindow()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Implements the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources of the current instance.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> if calls from the <see cref="Dispose()"/> method; <c>false</c> for the destructor.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    this.fontDialog.Dispose();
                this.disposed = true;
            }
        }

        #region Font settings

        /// <summary>
        /// Handles the <c>Click</c> routed events of the <see cref="btnFontChange"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnFontChange_Click(object sender, RoutedEventArgs e)
        {
            this.fontDialog.Font = new SysDraw.Font(
                App.Current.Resources["FontFamilyKey"].ToString(),
                Convert.ToSingle(App.Current.Resources["FontSizeKey"], CultureInfo.InvariantCulture));

            var oldFont = this.fontDialog.Font;
            var result = this.fontDialog.ShowDialog(new Win32Window(this));

            switch (result)
            {
                case WinForms.DialogResult.OK:
                    this.FontDialog_Apply(sender, e);
                    break;
                case WinForms.DialogResult.Cancel:
                    {
                        var app = App.Current as App;
                        if (app != null)
                            app.UpdateResources(oldFont);
                        break;
                    }
            }

            this.Focus();
        }

        /// <summary>
        /// Handles the <c>FontDialog.Apply</c> event of the <see cref="fontDialog"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void FontDialog_Apply(object sender, EventArgs e)
        {
            var app = App.Current as App;
            if (app != null)
                app.UpdateResources(this.fontDialog.Font);
        }

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnFontReset"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnFontReset_Click(object sender, RoutedEventArgs e)
        {
            var app = App.Current as App;
            if (app != null)
                app.UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize);
        }

        #endregion

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnOK"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the <c>PreviewKeyDown</c> routed event of the current window.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
