using Ingenium.WF2XAML.ViewModels;
using Ingenium.WF2XAML.Views;
using System;
using System.Diagnostics;
using System.Windows;

namespace Ingenium.WF2XAML
{
	public class App : Application
	{
		public App()
		{
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			base.Startup += new StartupEventHandler(this.OnStartup);
		}

		[DebuggerNonUserCode]
		[STAThread]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
		}

		private void OnStartup(object sender, StartupEventArgs e)
		{
			MainView mainView = new MainView();
			mainView.DataContext = new MainViewModel();
			mainView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			mainView.Show();
		}
	}
}