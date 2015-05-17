//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    /// The view model class for <see cref="ThScoreFileConverter.Views.MainWindow"/>.
    /// </summary>
    internal class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.Title = Assembly.GetExecutingAssembly().GetName().Name;
        }

        /// <summary>
        /// Gets a title string.
        /// </summary>
        public string Title { get; private set; }
    }
}
