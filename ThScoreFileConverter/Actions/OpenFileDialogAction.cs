//-----------------------------------------------------------------------
// <copyright file="OpenFileDialogAction.cs" company="None">
//     (c) 2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Actions
{
    using System;
    using System.Windows;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Encapsulates the handling of <see cref="System.Windows.Forms.OpenFileDialog"/>.
    /// </summary>
    public class OpenFileDialogAction : CommonDialogAction
    {
        #region Dependency properties

        /// <summary>
        /// Identifies the <see cref="AddExtension"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AddExtensionProperty = DependencyProperty.Register(
            "AddExtension", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="AutoUpgradeEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoUpgradeEnabledProperty = DependencyProperty.Register(
            "AutoUpgradeEnabled", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="CheckFileExists"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CheckFileExistsProperty = DependencyProperty.Register(
            "CheckFileExists", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="CheckPathExists"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CheckPathExistsProperty = DependencyProperty.Register(
            "CheckPathExists", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="DefaultExt"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultExtProperty = DependencyProperty.Register(
            "DefaultExt", typeof(string), typeof(OpenFileDialogAction), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="DereferenceLinks"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DereferenceLinksProperty = DependencyProperty.Register(
            "DereferenceLinks", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="FileName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
            "FileName", typeof(string), typeof(OpenFileDialogAction), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="Filter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            "Filter", typeof(string), typeof(OpenFileDialogAction), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="FilterIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterIndexProperty = DependencyProperty.Register(
            "FilterIndex", typeof(int), typeof(OpenFileDialogAction), new UIPropertyMetadata(1));

        /// <summary>
        /// Identifies the <see cref="InitialDirectory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InitialDirectoryProperty =
            DependencyProperty.Register(
                "InitialDirectory",
                typeof(string),
                typeof(OpenFileDialogAction),
                new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="Multiselect"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MultiselectProperty = DependencyProperty.Register(
            "Multiselect", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ReadOnlyChecked"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReadOnlyCheckedProperty = DependencyProperty.Register(
            "ReadOnlyChecked", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="RestoreDirectory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RestoreDirectoryProperty = DependencyProperty.Register(
            "RestoreDirectory", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ShowHelp"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowHelpProperty = DependencyProperty.Register(
            "ShowHelp", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ShowReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowReadOnlyProperty = DependencyProperty.Register(
            "ShowReadOnly", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="SupportMultiDottedExtensions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SupportMultiDottedExtensionsProperty =
            DependencyProperty.Register(
                "SupportMultiDottedExtensions",
                typeof(bool),
                typeof(OpenFileDialogAction),
                new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(OpenFileDialogAction), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="ValidateNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidateNamesProperty = DependencyProperty.Register(
            "ValidateNames", typeof(bool), typeof(OpenFileDialogAction), new UIPropertyMetadata(true));

        #endregion

        #region CLR properties

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box automatically adds an extension to a file
        /// name if the user omits the extension.
        /// </summary>
        public bool AddExtension
        {
            get { return (bool)this.GetValue(AddExtensionProperty); }
            set { this.SetValue(AddExtensionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WinForms.FileDialog"/> instance should
        /// automatically upgrade appearance and behavior when running on Windows Vista.
        /// </summary>
        public bool AutoUpgradeEnabled
        {
            get { return (bool)this.GetValue(AutoUpgradeEnabledProperty); }
            set { this.SetValue(AutoUpgradeEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a
        /// file name that does not exist.
        /// </summary>
        public bool CheckFileExists
        {
            get { return (bool)this.GetValue(CheckFileExistsProperty); }
            set { this.SetValue(CheckFileExistsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a
        /// path that does not exist.
        /// </summary>
        public bool CheckPathExists
        {
            get { return (bool)this.GetValue(CheckPathExistsProperty); }
            set { this.SetValue(CheckPathExistsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default file name extension.
        /// </summary>
        public string DefaultExt
        {
            get { return this.GetValue(DefaultExtProperty) as string; }
            set { this.SetValue(DefaultExtProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box returns the location of the file referenced
        /// by the shortcut or whether it returns the location of the shortcut (.lnk).
        /// </summary>
        public bool DereferenceLinks
        {
            get { return (bool)this.GetValue(DereferenceLinksProperty); }
            set { this.SetValue(DereferenceLinksProperty, value); }
        }

        /// <summary>
        /// Gets or sets a string containing the file name selected in the file dialog box.
        /// </summary>
        public string FileName
        {
            get { return this.GetValue(FileNameProperty) as string; }
            set { this.SetValue(FileNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear in the
        /// "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        public string Filter
        {
            get { return this.GetValue(FilterProperty) as string; }
            set { this.SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the filter currently selected in the file dialog box.
        /// </summary>
        public int FilterIndex
        {
            get { return (int)this.GetValue(FilterIndexProperty); }
            set { this.SetValue(FilterIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog box.
        /// </summary>
        public string InitialDirectory
        {
            get { return this.GetValue(InitialDirectoryProperty) as string; }
            set { this.SetValue(InitialDirectoryProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box allows multiple files to be selected.
        /// </summary>
        public bool Multiselect
        {
            get { return (bool)this.GetValue(MultiselectProperty); }
            set { this.SetValue(MultiselectProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the read-only check box is selected.
        /// </summary>
        public bool ReadOnlyChecked
        {
            get { return (bool)this.GetValue(ReadOnlyCheckedProperty); }
            set { this.SetValue(ReadOnlyCheckedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box restores the current directory before
        /// closing.
        /// </summary>
        public bool RestoreDirectory
        {
            get { return (bool)this.GetValue(RestoreDirectoryProperty); }
            set { this.SetValue(ReadOnlyCheckedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <c>Help</c> button is displayed in the file dialog
        /// box.
        /// </summary>
        public bool ShowHelp
        {
            get { return (bool)this.GetValue(ShowHelpProperty); }
            set { this.SetValue(ShowHelpProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box contains a read-only check box.
        /// </summary>
        public bool ShowReadOnly
        {
            get { return (bool)this.GetValue(ShowReadOnlyProperty); }
            set { this.SetValue(ShowReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box supports displaying and saving files that
        /// have multiple file name extensions.
        /// </summary>
        public bool SupportMultiDottedExtensions
        {
            get { return (bool)this.GetValue(SupportMultiDottedExtensionsProperty); }
            set { this.SetValue(SupportMultiDottedExtensionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the file dialog box title.
        /// </summary>
        public string Title
        {
            get { return this.GetValue(TitleProperty) as string; }
            set { this.SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box accepts only valid Win32 file names.
        /// </summary>
        public bool ValidateNames
        {
            get { return (bool)this.GetValue(ValidateNamesProperty); }
            set { this.SetValue(ValidateNamesProperty, value); }
        }

        #endregion

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action; but not used.</param>
        protected override void Invoke(object parameter)
        {
            WinForms.OpenFileDialog dialog = null;

            try
            {
                dialog = new WinForms.OpenFileDialog();

                dialog.AddExtension = this.AddExtension;
                dialog.AutoUpgradeEnabled = this.AutoUpgradeEnabled;
                dialog.CheckFileExists = this.CheckFileExists;
                dialog.CheckPathExists = this.CheckPathExists;
                dialog.DefaultExt = this.DefaultExt;
                dialog.DereferenceLinks = this.DereferenceLinks;
                dialog.FileName = this.FileName;
                dialog.Filter = this.Filter;
                dialog.FilterIndex = this.FilterIndex;
                dialog.InitialDirectory = this.InitialDirectory;
                dialog.Multiselect = this.Multiselect;
                dialog.ReadOnlyChecked = this.ReadOnlyChecked;
                dialog.RestoreDirectory = this.RestoreDirectory;
                dialog.ShowHelp = this.ShowHelp;
                dialog.ShowReadOnly = this.ShowReadOnly;
                dialog.Site = this.Site;
                dialog.SupportMultiDottedExtensions = this.SupportMultiDottedExtensions;
                dialog.Tag = this.Tag;
                dialog.Title = this.Title;
                dialog.ValidateNames = this.ValidateNames;

                var dialogResult = dialog.ShowDialog(new Win32Window(this.Owner));

                switch (dialogResult)
                {
                    case WinForms.DialogResult.OK:
                        if (this.OkCommand != null)
                        {
                            var result = new OpenFileDialogActionResult(dialog.FileName, dialog.FileNames);
                            if (this.OkCommand.CanExecute(result))
                                this.OkCommand.Execute(result);
                        }

                        break;

                    case WinForms.DialogResult.Cancel:
                        if (this.CancelCommand != null)
                        {
                            if (this.CancelCommand.CanExecute(null))
                                this.CancelCommand.Execute(null);
                        }

                        break;

                    default:
                        throw new NotImplementedException("Should not reach here.");
                }
            }
            finally
            {
                dialog.Dispose();
            }
        }
    }
}
