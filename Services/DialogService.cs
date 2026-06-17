using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.Services
{
	public class DialogService : IDialogService
	{
		public void ShowError(string message, string title)
		{
			MessageBox.Show(message, title, MessageBoxButton.OK);
		}

		public void ShowInfo(string message, string title)
		{
			MessageBox.Show(message, title, MessageBoxButton.OK);
		}

		public void ShowWarning(string message, string title) 
		{
			MessageBox.Show(message, title, MessageBoxButton.OK);
		}

		public bool ShowYesOrNo(string message, string title)
		{
			MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo);
			return result == MessageBoxResult.Yes;
		}
	}
}
