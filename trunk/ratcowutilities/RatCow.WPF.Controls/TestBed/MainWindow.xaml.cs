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
using System.Collections.ObjectModel;
using RatCow.WPF.Controls;

namespace TestBed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            chx.Items = data;
        }

        ObservableCollection<RatCow.WPF.Controls.CheckListItem> data = new ObservableCollection<RatCow.WPF.Controls.CheckListItem>() {
            new RatCow.WPF.Controls.CheckListItem() { Text = "Hello", IsChecked=true },
            new RatCow.WPF.Controls.CheckListItem() { Text = "There", IsChecked=false },
            new RatCow.WPF.Controls.CheckListItem() { Text = "This", IsChecked=true },
            new RatCow.WPF.Controls.CheckListItem() { Text = "Is", IsChecked=false },
            new RatCow.WPF.Controls.CheckListItem() { Text = "Test", IsChecked=true },
            new RatCow.WPF.Controls.CheckListItem() { Text = "Data", IsChecked=false },
        };

        class X
        {
            public bool Value { get; set; }
            public string Text { get; set; }
        }

        private void HeaderControl_ImageClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Yes it works");
        }

        private void GraphicalCheckBox_CheckClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((sender as GraphicalCheckBox).IsChecked ? "Checked": "Unchecked");
        }
    }
}
