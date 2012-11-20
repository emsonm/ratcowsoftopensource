using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using SWF = System.Windows.Forms;

namespace Ingenium.WF2XAML.Views
{
  public partial class MainView : Window, IComponentConnector
  {
    public MainView()
    {
      this.InitializeComponent();
    }

    private void About_Click( object sender, RoutedEventArgs e )
    {
      AboutView aboutView = new AboutView();
      aboutView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      aboutView.ShowDialog();
    }

    private void btnSelSource_Click( object sender, RoutedEventArgs e )
    {
      using ( SWF.OpenFileDialog openFileDialog = new SWF.OpenFileDialog() )
      {
        if ( openFileDialog.ShowDialog() == SWF.DialogResult.OK )
        {
          this.txtSelSource.Text = openFileDialog.FileName;
          BindingExpression bindingExpression = BindingOperations.GetBindingExpression( this.txtSelSource, TextBox.TextProperty );
          bindingExpression.UpdateSource();
        }
      }
    }
  }
}