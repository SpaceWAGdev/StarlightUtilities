using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SupportUtilities
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static bool DEBUG_ENABLE = true;

		public MainWindow()
		{
			Settings.LoadSettingsFromFile();
			InitializeComponent();
			Java.Installer.InstallCheck();
		}

		public void ShowDebugMessage(string message)
		{
			if (DEBUG != null)
			{
				DEBUG.Text = message;
			}
			if (DEBUG_ENABLE)
			{
				Logging.Log(message);
			}
		}

		private void OSInfoPanelInit(object sender, EventArgs e)
		{
			ShowDebugMessage("OS Version Panel Initialized");
			OSInfoPanel.Content = Environment.OSVersion.ToString();
		}

		private void LaunchSysInfo(object sender, RoutedEventArgs e)
		{
			Process.Start("winver");
			ShowDebugMessage("winver");
		}

		private static void SetButtonColour(Button button, SolidColorBrush color)
		{
			button.Background = color;
		}

		private void DiscordButtonClick(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText("Stardust#4085");
			DiscordCreatorButton.Background = new SolidColorBrush(Colors.GreenYellow);
			ShowDebugMessage("Copied Username to Clipboard");

			Timer.DelayAction(300, new Action(() => { SetButtonColour(DiscordCreatorButton, new SolidColorBrush(Colors.LightGray)); }));
		}

		private void TaskmanagerButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("taskmgr.exe");
			Process.Start("taskmgr.exe");
		}

		private void ControlpanelButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("control.exe");
			Process.Start("control.exe");
		}

		private void AppdataButtonClick(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				UseShellExecute = true,
				Verb = "open"
			}); ;

			ShowDebugMessage("Opened %appdata% in File Explorer");
			//ShowDebugMessage("powershell.exe -Command&{cd $HOME/Appdata/Roaming; explorer .}");
			//Process.Start("powershell.exe", "-WindowStyle Hidden -Command & {cd $HOME/AppData/Roaming; explorer .}");
		}

		private void AdvancedsystemsettingsButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("explorer shell:::{BB06C0E4-D293-4f75-8A90-CB05B6477EEE}");
			Process.Start("explorer", "shell:::{BB06C0E4-D293-4f75-8A90-CB05B6477EEE}");
		}

		private void EnableDebugMode(object sender, RoutedEventArgs e)
		{
			Settings.SetSetting("App.EnableLogging", "1");
			DEBUG_ENABLE = true;
			ShowDebugMessage("Logging ENABLED");
		}
		private void DisableDebugMode(object sender, RoutedEventArgs e)
		{
			Settings.SetSetting("App.EnableLogging", "0");
			ShowDebugMessage("Logging DISABLED");
			DEBUG_ENABLE = false;
		}

		private void ShowLog(object sender, MouseButtonEventArgs e)
		{
			//Clipboard.SetText(DEBUG.Text);
			//string previous = DEBUG.Text;
			//ShowDebugMessage("Copied Last Command to Clipboard");
			//Timer.DelayAction(400, new Action(() => { ShowDebugMessage(previous); }));
			Logging.ViewLog();
			ShowDebugMessage("Opened Log in Notepad");
		}

		private void RegeditButtonClick(object sender, RoutedEventArgs e)
		{
			string messageBoxText = "The Registry Editor is only intended for experienced users, it is VERY easy to break your entire system. Do you wish to continue?";
			string caption = "Registry Editor";
			MessageBoxButton button = MessageBoxButton.YesNo;
			MessageBoxImage icon = MessageBoxImage.Warning;
			MessageBoxResult result;

			result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
			if (result == MessageBoxResult.Yes)
			{
				Process.Start("regedit.exe");
				ShowDebugMessage("regedit.exe with user confirmation");
			}
			else
			{
				ShowDebugMessage("regedit cancelled due to user concerns");
			}
		}

		private void ServiceButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("mmc.exe services.msc");
			Process.Start("mmc.exe", "services.msc");
		}

		private void PerfmonButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("perfmon.exe");
			Process.Start("perfmon.exe");
		}

		private void ResmonButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("resmon.exe");
			Process.Start("resmon.exe");
		}

		private void PowershellButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("powershell.exe in $HOME w/ custom logo");
			Process.Start("powershell.exe", "-NoExit -NoLogo -Command & {echo \"STARLIGHT-POWERSHELL-1.0.0\"; cd $HOME}");
		}

		private void GenericInfoPanelInit(object sender, EventArgs e)
		{
			GenericInfoPanel.Text = Environment.UserName + "@" + Environment.MachineName;
			ShowDebugMessage("Initialized Generic Info Panel: " + GenericInfoPanel.Text);
		}

		private void PyInfoButtonInit(object sender, EventArgs e)
		{
			//bool pythonExists = Python.Utilities.IsPythonInstalled();
			bool pythonExists = true;
			PyInfoButton.IsEnabled = true;
			if (pythonExists)
			{
				PyInfoButton.Background = new SolidColorBrush(Colors.Green);
				ShowDebugMessage("Python correctly installed in " + Python.Environment.PYTHON_PATH);
			}
			else
			{
				PyInfoButton.Background = new SolidColorBrush(Colors.Red);
				ShowDebugMessage("ERROR with the shipped Python installation");
			}
		}

		private void NodeInfoButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("node.exe");
			Process.Start("C:\\Program Files\\nodejs\\node.exe");
		}
		private void NodeInfoButtonInit(object sender, EventArgs e)
		{
			bool nodeExists = Directory.Exists("C:\\Program Files\\nodejs\\");
			NodeJSInfoButton.IsEnabled = nodeExists;
			Settings.SetSetting("Node.IsInstalled", nodeExists.ToString());
			if (nodeExists)
			{
				ShowDebugMessage("Node.JS installed");
				NodeJSInfoButton.Background = new SolidColorBrush(Colors.Green);
			}
			else
			{
				ShowDebugMessage("Node.JS is missing");
				NodeJSInfoButton.Background = new SolidColorBrush(Colors.Red);
			}
		}

		private void PyInfoButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("python.exe");
			Process.Start(Python.Environment.PYTHON_PATH);
		}

		private void GitInfoButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("git-bash.exe");
			Process.Start("C:\\Program Files\\git\\git-bash.exe");
		}

		private void GitInfoButtonInit(object sender, EventArgs e)
		{
			//? update for shipped git install
			bool gitExists = Directory.Exists("C:\\Program Files\\git");
			GitInfoButton.IsEnabled = gitExists;
			if (gitExists)
			{
				ShowDebugMessage("Git is installed");
				GitInfoButton.Background = new SolidColorBrush(Colors.Green);
			}
			else
			{
				ShowDebugMessage("Git is missing");
				GitInfoButton.Background = new SolidColorBrush(Colors.Red);
			}
		}

		private void TempButtonClick(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = Path.GetTempPath(),
				UseShellExecute = true,
				Verb = "open"
			}); ;
			ShowDebugMessage("Opened %TMP% in File Explorer");
		}

		private void MSInfoButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("msinfo32.exe");
			Process.Start("msinfo32.exe");
		}

		private void EventviewerButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("eventvwr.exe");
			Process.Start("eventvwr.exe");
		}

		private void QuickassistButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("[WIN]+R > \"quickassist.exe\" > [ENTER]");
		}

		private void DevicemanagerButtonCLick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("mmc.exe devmgmt.msc");
			Process.Start("mmc.exe", "devmgmt.msc");
		}

		private void PythonScriptLoadButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("Opened Python file");
			PythonScriptNameTextBlock.Text = Python.Utilities.GetLoadedScript();
			Python.Environment.OpenScriptFile();
		}

		private void ScriptRunnerButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("Run Python script " + Python.Utilities.GetLoadedScript());
			Python.Environment.Run();
		}

		private void EnableThirdpartyScripts(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("Thirdparty scripts ENABLED");
			Python.Utilities.EnableThirdpartyScripts();
		}

		private void DisableThirdpartyScripts(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("Thirdparty scripts DISABLED");
			Python.Utilities.DisableThirdpartyScripts();
		}

		private void HWINFO64ButtonClick(object sender, RoutedEventArgs e)
		{
			ShowDebugMessage("whinfo64.exe");
			Process.Start(Path.Join(System.Environment.CurrentDirectory, "hwinfo64\\HWiNFO64.exe"));
		}

		private void JavaButtonClick(object sender, RoutedEventArgs e)
		{
			Java.Installer.LaunchJavaw(@"C:\Users\Liams\Downloads\optifine.jar");
		}

		private void JavaButtonInit(object sender, EventArgs e)
		{
			if (Directory.Exists(Java.Installer.JavaPath))
			{
				JavaInfoButton.IsEnabled = true;
			}
			else
			{
				JavaInfoButton.IsEnabled = true;
			}
		}

		private void ShowRawSettings(object sender, RoutedEventArgs e)
		{
			ProcessStartInfo info = new ProcessStartInfo();
			info.FileName = "notepad.exe";
			info.Arguments = Settings.SETTINGS_PATH;
			info.Verb = "open";
			Process.Start(info);
		}

		private void EnableLogButtonInit(object sender, EventArgs e)
		{
			LogCheckBox.IsChecked = Settings.TryGetBool("App.EnableLogging");
		}

		private void OnWindowClose(object sender, EventArgs e)
		{
			Settings.SaveSettings();
		}

		string OptifineVersion = "";

		private void Optifine1_8_9(object sender, RoutedEventArgs e)
		{
			OptifineVersion = "1.8.9";
		}

		private void Optifine_1_12_2(object sender, RoutedEventArgs e)
		{
			OptifineVersion = "1.12.2";
		}

		private void OptifineLatest(object sender, RoutedEventArgs e)
		{
			OptifineVersion = "LatestMinecraft";
		}

		private void OptfineInstallButtonClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(OptifineVersion)) { return; }
			Java.Installer.InstallOptifine(OptifineVersion);
		}

		private void VSTOpenZip(object sender, RoutedEventArgs e)
		{
			string? path = Windows.Dialogs.OpenFileBox(".zip", "Zip files(.zip)|*.zip");
			if(path == null) { return; }
			VSTOpenZipButton.Content = path;
		}

		private void VSTInstallPlugin(object sender, RoutedEventArgs e)
		{
			VSTPlugins.InstallPlugin(VSTOpenZipButton.Content.ToString());
		}

		private void PipTextboxKeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key != Key.Enter) { return; }
			string modules = PipTextBox.Text;
			PipTextBox.Text = "";
			Python.Utilities.PipInstall(modules);
		}
	}
}