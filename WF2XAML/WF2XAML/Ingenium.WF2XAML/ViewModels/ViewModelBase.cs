using System;
using System.ComponentModel;

namespace Ingenium.WF2XAML.ViewModels
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		protected ViewModelBase()
		{
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}