//-----------------------------------------------------------------------
// <copyright file="FolderBrowserDialogAction.cs" company="None">
//     (c) 2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Actions
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using ThScoreFileConverter.Models;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Encapsulates the handling of <see cref="System.Windows.Forms.FolderBrowserDialog"/>.
    /// </summary>
    public class FolderBrowserDialogAction : TriggerAction<UIElement>
    {
        #region Dependency properties

        /// <summary>
        /// Identifies the <see cref="OkCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OkCommandProperty = DependencyProperty.Register(
            "OkCommand", typeof(ICommand), typeof(FolderBrowserDialogAction), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CancelCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register(
                "CancelCommand",
                typeof(ICommand),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
                "Description",
                typeof(string),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="Owner"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
            "Owner", typeof(Window), typeof(FolderBrowserDialogAction), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RootFolder"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootFolderProperty =
            DependencyProperty.Register(
                "RootFolder",
                typeof(Environment.SpecialFolder),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(Environment.SpecialFolder.Desktop));

        /// <summary>
        /// Identifies the <see cref="SelectedPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register(
                "SelectedPath",
                typeof(string),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="ShowNewFolderButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowNewFolderButtonProperty =
            DependencyProperty.Register(
                "ShowNewFolderButton",
                typeof(bool),
                typeof(FolderBrowserDialogAction),
                new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Site"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SiteProperty = DependencyProperty.Register(
            "Site", typeof(ISite), typeof(FolderBrowserDialogAction), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Tag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TagProperty = DependencyProperty.Register(
            "Tag", typeof(object), typeof(FolderBrowserDialogAction), new UIPropertyMetadata(null));

        #endregion

        #region CLR properties

        /// <summary>
        /// Gets or sets the command invoked when the user clicks the <c>OK</c> button in the dialog box.
        /// </summary>
        public ICommand OkCommand
        {
            get { return this.GetValue(OkCommandProperty) as ICommand; }
            set { this.SetValue(OkCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command invoked when the user clicks the <c>Cancel</c> button in the dialog box.
        /// </summary>
        public ICommand CancelCommand
        {
            get { return this.GetValue(CancelCommandProperty) as ICommand; }
            set { this.SetValue(CancelCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        public string Description
        {
            get { return this.GetValue(DescriptionProperty) as string; }
            set { this.SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the window which owns the font dialog box.
        /// </summary>
        public Window Owner
        {
            get { return this.GetValue(OwnerProperty) as Window; }
            set { this.SetValue(OwnerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root folder where the browsing starts from.
        /// </summary>
        public Environment.SpecialFolder RootFolder
        {
            get { return (Environment.SpecialFolder)this.GetValue(RootFolderProperty); }
            set { this.SetValue(RootFolderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        public string SelectedPath
        {
            get { return this.GetValue(SelectedPathProperty) as string; }
            set { this.SetValue(SelectedPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <c>New Folder</c> button appears in the folder
        /// browsing dialog box.
        /// </summary>
        public bool ShowNewFolderButton
        {
            get { return (bool)this.GetValue(ShowNewFolderButtonProperty); }
            set { this.SetValue(ShowNewFolderButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> of the <see cref="Component"/>.
        /// </summary>
        public ISite Site
        {
            get { return this.GetValue(SiteProperty) as ISite; }
            set { this.SetValue(SiteProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object that contains data about the control.
        /// </summary>
        public object Tag
        {
            get { return this.GetValue(TagProperty); }
            set { this.SetValue(TagProperty, value); }
        }

        #endregion

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action; but not used.</param>
        protected override void Invoke(object parameter)
        {
            WinForms.FolderBrowserDialog dialog = null;

            try
            {
                dialog = new WinForms.FolderBrowserDialog();

                dialog.Description = this.Description;
                dialog.RootFolder = this.RootFolder;
                dialog.SelectedPath = this.SelectedPath;
                dialog.ShowNewFolderButton = this.ShowNewFolderButton;
                dialog.Site = this.Site;
                dialog.Tag = this.Tag;

                var dialogResult = dialog.ShowDialog(new Win32Window(this.Owner));

                switch (dialogResult)
                {
                    case WinForms.DialogResult.OK:
                        if (this.OkCommand != null)
                        {
                            var result = new FolderBrowserDialogActionResult(dialog.SelectedPath);
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
