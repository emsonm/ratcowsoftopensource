using System;
using System.Windows;
using System.Windows.Input;

namespace Ingenium.WF2XAML.Commands
{
	public class CommandReference : Freezable, ICommand
	{
		public readonly static DependencyProperty CommandProperty;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(CommandReference.CommandProperty);
			}
			set
			{
				base.SetValue(CommandReference.CommandProperty, value);
			}
		}

		static CommandReference()
		{
			CommandReference.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandReference), new PropertyMetadata(new PropertyChangedCallback(CommandReference.OnCommandChanged)));
		}

		public CommandReference()
		{
		}

		public bool CanExecute(object parameter)
		{
			if (this.Command == null)
			{
				return false;
			}
			else
			{
				return this.Command.CanExecute(parameter);
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			throw new NotImplementedException();
		}

		public void Execute(object parameter)
		{
			this.Command.Execute(parameter);
		}

		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CommandReference commandReference = d as CommandReference;
			ICommand oldValue = e.OldValue as ICommand;
			ICommand newValue = e.NewValue as ICommand;
			if (oldValue != null)
			{
				oldValue.CanExecuteChanged -= commandReference.CanExecuteChanged;
			}
			if (newValue != null)
			{
				newValue.CanExecuteChanged += commandReference.CanExecuteChanged;
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}