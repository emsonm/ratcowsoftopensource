using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Reflection;

namespace RatCow.WPF.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RatCow.WPF.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RatCow.WPF.Controls;assembly=RatCow.WPF.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class HeaderControl : UserControl
    {
        static HeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderControl), new FrameworkPropertyMetadata(typeof(HeaderControl)));
        }

        public HeaderControl()
        {
        }

        /// <summary>
        /// This is the Control property that we expose to the user.
        /// </summary>


        #region NormalImage property

        [Category("HeaderControl")]
        public BitmapImage NormalImage
        {
            get { return (BitmapImage)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        private static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(BitmapImage), typeof(HeaderControl),
            new FrameworkPropertyMetadata(DefaultNormalImage(),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnNormalImageChanged,
                CoerceNormalImage
                ));

        static BitmapImage _defaultNormalImage = null;

        private static BitmapImage DefaultNormalImage()
        {
            if (_defaultNormalImage == null)
            {
                //var uri = new Uri("pack://application:,,,/RatCow.WPF.Controls;component:/Empty_mousedown.bmp", UriKind.RelativeOrAbsolute);
                try
                {
                    _defaultNormalImage = new BitmapImage();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }

            return _defaultNormalImage;
        }

        private static void OnNormalImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as HeaderControl;
            if (control != null)
            {
                var newValue = (BitmapImage)args.NewValue;
                var oldValue = (BitmapImage)args.OldValue;

                RoutedPropertyChangedEventArgs<BitmapImage> e =
                    new RoutedPropertyChangedEventArgs<BitmapImage>(oldValue, newValue, NormalImageChangedEvent);

                control.OnNormalImageChanged(e);
            }
        }

        virtual protected void OnNormalImageChanged(RoutedPropertyChangedEventArgs<BitmapImage> e)
        {
            RaiseEvent(e);
        }

        private static readonly RoutedEvent NormalImageChangedEvent =
            EventManager.RegisterRoutedEvent("NormalImageChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<BitmapImage>), typeof(HeaderControl));

        public event RoutedPropertyChangedEventHandler<BitmapImage> NormalImageChanged
        {
            add { AddHandler(NormalImageChangedEvent, value); }
            remove { RemoveHandler(NormalImageChangedEvent, value); }
        }

        private static object CoerceNormalImage(DependencyObject obj, object value)
        {
            var newValue = (BitmapImage)value;
            var control = obj as HeaderControl;

            // don't need to do this at the moment

            if (control != null && newValue == null)
            {
                //  ensure that the value stays within the bounds of the minimum and
                //  maximum values that we define.
                newValue = DefaultNormalImage();
            }

            return newValue;
        }

        #endregion

        #region OverImage property

        [Category("HeaderControl")]
        public BitmapImage OverImage
        {
            get { return (BitmapImage)GetValue(OverImageProperty); }
            set { SetValue(OverImageProperty, value); }
        }

        private static readonly DependencyProperty OverImageProperty =
            DependencyProperty.Register("OverImage", typeof(BitmapImage), typeof(HeaderControl),
            new FrameworkPropertyMetadata(DefaultOverImage(),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnOverImageChanged,
                CoerceOverImage
                ));

        static BitmapImage _defaultOverImage = null;

        private static BitmapImage DefaultOverImage()
        {
            //var uri = new Uri("/RatCow.WPF.Controls;component:/Empty_mousedown.bmp", UriKind.RelativeOrAbsolute);
            try
            {
                _defaultOverImage = new BitmapImage();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return _defaultOverImage;
        }

        private static void OnOverImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as HeaderControl;
            if (control != null)
            {
                var newValue = (BitmapImage)args.NewValue;
                var oldValue = (BitmapImage)args.OldValue;

                RoutedPropertyChangedEventArgs<BitmapImage> e =
                    new RoutedPropertyChangedEventArgs<BitmapImage>(oldValue, newValue, OverImageChangedEvent);

                control.OnOverImageChanged(e);
            }
        }

        virtual protected void OnOverImageChanged(RoutedPropertyChangedEventArgs<BitmapImage> e)
        {
            RaiseEvent(e);
        }

        private static readonly RoutedEvent OverImageChangedEvent =
            EventManager.RegisterRoutedEvent("OverImageChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<BitmapImage>), typeof(HeaderControl));

        public event RoutedPropertyChangedEventHandler<BitmapImage> OverImageChanged
        {
            add { AddHandler(OverImageChangedEvent, value); }
            remove { RemoveHandler(OverImageChangedEvent, value); }
        }

        private static object CoerceOverImage(DependencyObject obj, object value)
        {
            var newValue = (BitmapImage)value;
            var control = obj as HeaderControl;

            // don't need to do this at the moment

            if (control != null && newValue == null)
            {
                //  ensure that the value stays within the bounds of the minimum and
                //  maximum values that we define.
                newValue = DefaultOverImage();
            }

            return newValue;
        }

        #endregion


        #region Text property

        [Category("HeaderControl")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HeaderControl),
            new FrameworkPropertyMetadata("Header",
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnTextChanged,
                CoerceText
                ));

        /// <summary>
        /// If the value changes, update the text box that displays the Value 
        /// property to the consumer.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void OnTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as HeaderControl;
            if (control != null)
            {
                var newValue = (string)args.NewValue;
                var oldValue = (string)args.OldValue;

                RoutedPropertyChangedEventArgs<string> e =
                    new RoutedPropertyChangedEventArgs<string>(oldValue, newValue, TextChangedEvent);

                control.OnTextChanged(e);
            }
        }

        /// <summary>
        /// Raise the ValueChanged event.  Derived classes can use this.
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnTextChanged(RoutedPropertyChangedEventArgs<string> e)
        {
            RaiseEvent(e);
        }

        private static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<string>), typeof(HeaderControl));

        public event RoutedPropertyChangedEventHandler<string> TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        private static object CoerceText(DependencyObject obj, object value)
        {
            var newValue = (string)value;
            var control = obj as HeaderControl;

            // don't need to do this at the moment

            //if (control != null)
            //{
            //    //  ensure that the value stays within the bounds of the minimum and
            //    //  maximum values that we define.
            //    newValue = LimitValueByBounds(newValue, control);
            //}

            return newValue;
        }


        #endregion

        public static RoutedEvent ImageClickEvent =
        EventManager.RegisterRoutedEvent("ImageClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HeaderControl));

        public event RoutedEventHandler ImageClick
        {
            add { AddHandler(ImageClickEvent, value); }
            remove { RemoveHandler(ImageClickEvent, value); }
        }

        protected virtual void OnImageClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(ImageClickEvent, this);

            RaiseEvent(args);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (e.OriginalSource is Image)
            {
                OnImageClick();
            }
        }
    }

    //public static class UriExtensions
    //{
    //    public static BitmapImage GetBitmapImage(
    //    this Uri imageAbsolutePath,
    //    BitmapCacheOption bitmapCacheOption = BitmapCacheOption.OnLoad)
    //    {
    //        BitmapImage image = new BitmapImage();

    //        try
    //        {
    //            image.BeginInit();
    //            image.CacheOption = bitmapCacheOption;
    //            image.UriSource = imageAbsolutePath;
    //            image.EndInit();
    //        }
    //        catch (Exception ex)
    //        {
    //            System.Diagnostics.Debug.WriteLine(ex.Message);
    //            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
    //        }

    //        return image;
    //    }
    //}
}
