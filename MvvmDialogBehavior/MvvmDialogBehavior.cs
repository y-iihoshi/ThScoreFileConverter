using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Controls;

namespace MvvmDialogBehavior
{
    public class DialogBehavior : Behavior<UIElement>
    {

        public Object Content
        {
            get { return (Object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                "Content",
                typeof(Object),
                typeof(DialogBehavior),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (depObj, e) =>
                    {
                        var self = depObj as DialogBehavior;
                        if (self._IsAttached)
                        {
                            self.ContentChanged();
                        }
                    }));

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                "ContentTemplate",
                typeof(DataTemplate),
                typeof(DialogBehavior),
                new UIPropertyMetadata(null, null));

        public Style Style
        {
            get { return (Style)GetValue(StyleProperty); }
            set { SetValue(StyleProperty, value); }
        }

        public static readonly DependencyProperty StyleProperty =
            DependencyProperty.Register(
                "Style",
                typeof(Style),
                typeof(DialogBehavior),
                new UIPropertyMetadata(null, null));

        public ResourceDictionary Resources
        {
            get { return (ResourceDictionary)GetValue(ResourcesProperty); }
            set { SetValue(ResourcesProperty, value); }
        }

        public static readonly DependencyProperty ResourcesProperty =
            DependencyProperty.Register(
                "Resources",
                typeof(ResourceDictionary),
                typeof(DialogBehavior),
                new UIPropertyMetadata(new ResourceDictionary(), (depObj, e) =>
                {
                    var self = depObj as DialogBehavior;
                    if (self._Dialog != null)
                    {
                        self._Dialog.Resources = (ResourceDictionary)e.NewValue;
                    }
                }));

        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }

        public static readonly DependencyProperty IsModalProperty =
            DependencyProperty.Register(
                "IsModal",
                typeof(bool),
                typeof(DialogBehavior),
                new UIPropertyMetadata(true, null));

        private Window _Dialog;
        private bool _IsAttached;

        protected override void OnAttached()
        {
            _IsAttached = true;

            base.OnAttached();

            if (Content != null)
            {
                ContentChanged();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        public void ContentChanged()
        {
            if (_Dialog != null)
            {
                CloseDialog();
            }

            if (Content != null)
            {
                OpenDialog();
            }
        }

        private void OpenDialog()
        {
            _Dialog = new Window();

            Binding styleBinding = new Binding("Style");
            styleBinding.Source = this;
            _Dialog.SetBinding(FrameworkElement.StyleProperty, styleBinding);

            Binding contentTemplateBinding = new Binding("ContentTemplate");
            contentTemplateBinding.Source = this;
            _Dialog.SetBinding(ContentControl.ContentTemplateProperty, contentTemplateBinding);

            if (Resources.Count > 0)
            {
                _Dialog.Resources = Resources;
            }

            _Dialog.Closed += Closed;

            _Dialog.Content = Content;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (IsModal)
                {
                    _Dialog.ShowDialog();
                }
                else
                {
                    _Dialog.Show();
                }
            }));
        }

        private void Closed(object sender, EventArgs e)
        {
            if (Content != null) // Window Close Button or Alt+F4 or etc.
            {
                Content = null;
            }
        }

        private void CloseDialog()
        {
            _Dialog.Close();
            _Dialog = null;
        }
    }

    public class WindowStyleBehavior : Behavior<FrameworkElement>
    {

        public Style Style
        {
            get { return (Style)GetValue(StyleProperty); }
            set { SetValue(StyleProperty, value); }
        }

        public static readonly DependencyProperty StyleProperty =
            DependencyProperty.Register(
                "Style",
                typeof(Style),
                typeof(WindowStyleBehavior),
                new UIPropertyMetadata(null, null));

        private Window _Dialog;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(Object sender, RoutedEventArgs e)
        {
            _Dialog = Window.GetWindow(this.AssociatedObject);

            Binding binding = new Binding("Style");
            binding.Source = this;
            _Dialog.SetBinding(Window.StyleProperty, binding);

            this.AssociatedObject.Loaded -= OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

    }
}
