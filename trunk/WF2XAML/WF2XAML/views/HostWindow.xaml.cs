using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ingenium.WF2XAML.Views
{
  public partial class HostWindow : Window, IComponentConnector
  {
    public HostWindow()
    {
      this.InitializeComponent();
    }

    public void InjectControl( UserControl hostedControl )
    {
      this.LayoutRoot.Children.Add( hostedControl );
    }
  }
}