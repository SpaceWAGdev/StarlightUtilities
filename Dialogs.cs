using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SupportUtilities.Windows
{
	public static class Dialogs
	{
		public static MessageBoxResult ShowMessageBox(string content, string title, MessageBoxButton button, MessageBoxImage icon)
		{
			return MessageBox.Show(content, title, button, icon);
		}
		public static string? OpenFileBox(string defaultExt, string filter)
		{
			// Configure open file dialog box
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.DefaultExt = defaultExt; // Default file extension
			dialog.Filter = filter; // Filter files by extension

			// Show open file dialog box
			bool? result = dialog.ShowDialog();

			// Process open file dialog box results
			if (result == true)
			{
				// Open document
				return dialog.FileName;
			}
			else
			{
				return null;
			}
		}
	}
}
