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
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:GraphicalCheckbox/>
    ///
    /// </summary>
    public class GraphicalCheckBox : Control
    {
        static GraphicalCheckBox()
        {
            CheckClickCommand = new RoutedCommand("CheckClickedCommand", typeof(GraphicalCheckBox));

            //  register the command bindings - if the buttons get clicked, call these methods.
            CommandManager.RegisterClassCommandBinding(typeof(GraphicalCheckBox), new CommandBinding(CheckClickCommand, OnCheckClickCommand));
            
            //  lastly bind some inputs:  i.e. if the user presses up/down arrow 
            //  keys, call the appropriate commands.
            CommandManager.RegisterClassInputBinding(typeof(GraphicalCheckBox), new InputBinding(CheckClickCommand, new KeyGesture(Key.Space)));
            CommandManager.RegisterClassInputBinding(typeof(GraphicalCheckBox), new InputBinding(CheckClickCommand, new KeyGesture(Key.Return)));
            CommandManager.RegisterClassInputBinding(typeof(GraphicalCheckBox), new InputBinding(CheckClickCommand, new KeyGesture(Key.Enter)));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphicalCheckBox), new FrameworkPropertyMetadata(typeof(GraphicalCheckBox)));
        }

        public GraphicalCheckBox()
            : base()
        {
            CurrentImage = GetImage(UncheckedImage);
        }

        #region CurrentImage ptoperty

        [Category("GraphicalCheckBox")]
        public Image CurrentImage
        {
            get { return (Image)GetValue(CurrentImageProperty); }
            set { SetValue(CurrentImageProperty, value); }
        }

        private static readonly DependencyProperty CurrentImageProperty =
            DependencyProperty.Register("CurrentImage", typeof(Image), typeof(GraphicalCheckBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCurrentImageChanged
                ));

        private static void OnCurrentImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as GraphicalCheckBox;
            if (control != null)
            {
                var newValue = (Image)args.NewValue;
                var oldValue = (Image)args.OldValue;

                RoutedPropertyChangedEventArgs<Image> e =
                    new RoutedPropertyChangedEventArgs<Image>(oldValue, newValue, CurrentImageChangedEvent);

                control.OnCurrentImageChanged(e);
            }
        }

        virtual protected void OnCurrentImageChanged(RoutedPropertyChangedEventArgs<Image> e)
        {
            RaiseEvent(e);
        }

        private static readonly RoutedEvent CurrentImageChangedEvent =
          EventManager.RegisterRoutedEvent("CurrentImageChanged", RoutingStrategy.Bubble,
          typeof(RoutedPropertyChangedEventHandler<Image>), typeof(GraphicalCheckBox));

        public event RoutedPropertyChangedEventHandler<Image> CurrentImageChanged
        {
            add { AddHandler(CurrentImageChangedEvent, value); }
            remove { RemoveHandler(CurrentImageChangedEvent, value); }
        }

        
        #endregion


        #region IsChecked ptoperty

        [Category("GraphicalCheckBox")]
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(GraphicalCheckBox),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIsCheckedChanged
                ));

        private static void OnIsCheckedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as GraphicalCheckBox;
            if (control != null)
            {
                var newValue = (bool)args.NewValue;
                var oldValue = (bool)args.OldValue;

                RoutedPropertyChangedEventArgs<bool> e =
                    new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue, IsCheckedChangedEvent);

                control.OnIsCheckedChanged(e);
            }
        }

        virtual protected void OnIsCheckedChanged(RoutedPropertyChangedEventArgs<bool> e)
        {
            RaiseEvent(e);
        }

        private static readonly RoutedEvent IsCheckedChangedEvent =
          EventManager.RegisterRoutedEvent("IsCheckedChanged", RoutingStrategy.Bubble,
          typeof(RoutedPropertyChangedEventHandler<bool>), typeof(GraphicalCheckBox));

        public event RoutedPropertyChangedEventHandler<bool> IsCheckedChanged
        {
            add { AddHandler(IsCheckedChangedEvent, value); }
            remove { RemoveHandler(IsCheckedChangedEvent, value); }
        }

        //The click event

        public static RoutedCommand CheckClickCommand { get; set; }

        protected static void OnCheckClickCommand(Object sender, ExecutedRoutedEventArgs e)
        {
            var control = sender as GraphicalCheckBox;

            if (control != null && e.OriginalSource != null && e.OriginalSource is Button)
            {
                control.OnCheckClick(e.OriginalSource as Button);
            }
        }

        private static RoutedEvent CheckClickEvent =
            EventManager.RegisterRoutedEvent("CheckClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GraphicalCheckBox));

        public event RoutedEventHandler CheckClick
        {
            add { AddHandler(CheckClickEvent, value); }
            remove { RemoveHandler(CheckClickEvent, value); }
        }

        protected virtual void OnCheckClick(Button source)
        {
            //this is "clicked"
            IsChecked = !IsChecked;

            if (IsChecked)
            {
                if (source != null && CheckedImage != null)
                {
                    if (source.Content != null && source.Content is Image)
                    {
                        source.Content = null; 
                    }

                    source.Content = GetImage(CheckedImage); 
                }
            }
            else
            {
                if (source != null && UncheckedImage != null)
                {
                    if (source.Content != null && source.Content is Image)
                    {
                        source.Content = null;
                    }

                    source.Content = GetImage(UncheckedImage);
                }
            }

            RoutedEventArgs args = new RoutedEventArgs(CheckClickEvent, this);

            RaiseEvent(args);

            //this would be "beforeclicked"
            //if (!args.Handled)
            //{
            //    //set the check
            //    IsChecked = !IsChecked;
            //}
        }

        private Image GetImage(BitmapImage bitmapImage)
        {
            try
            {
                var result = new Image();
                result.BeginInit();
                result.Source = bitmapImage;
                result.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                result.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                result.Width = bitmapImage.Width;
                result.Height = bitmapImage.Height;
                result.EndInit();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Text property

        [Category("GraphicalCheckBox")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(GraphicalCheckBox),
            new FrameworkPropertyMetadata("CheckBox",
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
            var control = obj as GraphicalCheckBox;
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
            typeof(RoutedPropertyChangedEventHandler<string>), typeof(GraphicalCheckBox));

        public event RoutedPropertyChangedEventHandler<string> TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        private static object CoerceText(DependencyObject obj, object value)
        {
            var newValue = (string)value;
            var control = obj as GraphicalCheckBox;

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

        #region CheckedImage property

        [Category("GraphicalCheckBox")]
        public BitmapImage CheckedImage
        {
            get { return (BitmapImage)GetValue(CheckedImageProperty); }
            set { SetValue(CheckedImageProperty, value); }
        }

        private static readonly DependencyProperty CheckedImageProperty =
            DependencyProperty.Register("CheckedImage", typeof(BitmapImage), typeof(GraphicalCheckBox),
            new FrameworkPropertyMetadata(DefaultCheckedImage(),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCheckedImageChanged,
                CoerceCheckedImage
                ));

        static BitmapImage _defaultCheckedImage = null;

        private static BitmapImage DefaultCheckedImage()
        {
            if (_defaultCheckedImage == null)
            {
                //var uri = new Uri("pack://application:,,,/RatCow.WPF.Controls;component:/Empty_mousedown.bmp", UriKind.RelativeOrAbsolute);
                try
                {
                    _defaultCheckedImage = new BitmapImage();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }

            return _defaultCheckedImage;
        }

        private static void OnCheckedImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as GraphicalCheckBox;
            if (control != null)
            {
                var newValue = (BitmapImage)args.NewValue;
                var oldValue = (BitmapImage)args.OldValue;

                RoutedPropertyChangedEventArgs<BitmapImage> e =
                    new RoutedPropertyChangedEventArgs<BitmapImage>(oldValue, newValue, CheckedImageChangedEvent);

                control.OnCheckedImageChanged(e);
            }
        }

        virtual protected void OnCheckedImageChanged(RoutedPropertyChangedEventArgs<BitmapImage> e)
        {
            RaiseEvent(e);
        }

        private static readonly RoutedEvent CheckedImageChangedEvent =
            EventManager.RegisterRoutedEvent("CheckedImageChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<BitmapImage>), typeof(GraphicalCheckBox));

        public event RoutedPropertyChangedEventHandler<BitmapImage> CheckedImageChanged
        {
            add { AddHandler(CheckedImageChangedEvent, value); }
            remove { RemoveHandler(CheckedImageChangedEvent, value); }
        }

        private static object CoerceCheckedImage(DependencyObject obj, object value)
        {
            var newValue = (BitmapImage)value;
            var control = obj as GraphicalCheckBox;

            // don't need to do this at the moment

            if (control != null && newValue == null)
            {
                //  ensure that the value stays within the bounds of the minimum and
                //  maximum values that we define.
                newValue = DefaultCheckedImage();
            }

            return newValue;
        }

        #endregion



        #region UncheckedImage property

        [Category("GraphicalCheckBox")]
        public BitmapImage UncheckedImage
        {
            get { return (BitmapImage)GetValue(UncheckedImageProperty); }
            set { SetValue(UncheckedImageProperty, value); }
        }

        private static readonly DependencyProperty UncheckedImageProperty =
            DependencyProperty.Register("UncheckedImage", typeof(BitmapImage), typeof(GraphicalCheckBox),
            new FrameworkPropertyMetadata(DefaultUncheckedImage(),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnUncheckedImageChanged,
                CoerceUncheckedImage
                ));

        static BitmapImage _defaultUncheckedImage = null;

        private static BitmapImage DefaultUncheckedImage()
        {
            if (_defaultUncheckedImage == null)
            {
                //var uri = new Uri("pack://application:,,,/RatCow.WPF.Controls;component:/Empty_mousedown.bmp", UriKind.RelativeOrAbsolute);
                try
                {
                    _defaultUncheckedImage = new BitmapImage();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }

            return _defaultUncheckedImage;
        }

        private static void OnUncheckedImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as GraphicalCheckBox;
            if (control != null)
            {
                var newValue = (BitmapImage)args.NewValue;
                var oldValue = (BitmapImage)args.OldValue;

                RoutedPropertyChangedEventArgs<BitmapImage> e =
                    new RoutedPropertyChangedEventArgs<BitmapImage>(oldValue, newValue, UncheckedImageChangedEvent);

                control.OnUncheckedImageChanged(e);
            }
        }

        virtual protected void OnUncheckedImageChanged(RoutedPropertyChangedEventArgs<BitmapImage> e)
        {
            RaiseEvent(e);
        }

        private static readonly RoutedEvent UncheckedImageChangedEvent =
            EventManager.RegisterRoutedEvent("UncheckedImageChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<BitmapImage>), typeof(GraphicalCheckBox));

        public event RoutedPropertyChangedEventHandler<BitmapImage> UncheckedImageChanged
        {
            add { AddHandler(UncheckedImageChangedEvent, value); }
            remove { RemoveHandler(UncheckedImageChangedEvent, value); }
        }

        private static object CoerceUncheckedImage(DependencyObject obj, object value)
        {
            var newValue = (BitmapImage)value;
            var control = obj as GraphicalCheckBox;

            // don't need to do this at the moment

            if (control != null && newValue == null)
            {
                //  ensure that the value stays within the bounds of the minimum and
                //  maximum values that we define.
                newValue = DefaultUncheckedImage();
            }

            return newValue;
        }

        #endregion

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (e.OriginalSource != null && e.OriginalSource is Button)
            {
                //ImageChange();
            }
        }
    }
}
