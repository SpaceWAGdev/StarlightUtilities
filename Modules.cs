using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SupportUtilities
{
	public class Module
	{
		public Module(string _name, string _description, string _version, string[] _commands)
		{
			this.Name = _name;
			this.Description = _description;
			this.Version = _version;
			this.Commands = _commands.ToList();
		}

		public string Name;
		public string Description;
		public string Version;
		public string Arguments;
		public List<string> Commands;

		public string Info(bool hr = true)
		{
			if (hr)
			{
				return $"Plugin {Name.Split("."[Name.Length - 1])} Version {Version} \n Used for: {Description} \n";
			}
			else
			{
				return $"Name:{Name},Version: {Version},Description:{Description},Arguments:{Arguments},Commands:{Commands}";
			}
		}
		public string[] GetCommands()
		{
			return Commands.ToArray();
		}
	}

	public static class Modules
	{
		public static string MODULE_PATH { get; } = Path.Join(Environment.CurrentDirectory, "modules");
		public static List<Module> InstalledModules { get; private set; } = new List<Module>();

		public static void AppendModule(string FullName)
		{
			if (!File.Exists(Path.Join(MODULE_PATH, FullName, $"{FullName}.module"))) { return; }

			string[] lines = File.ReadAllLines(Path.Combine(MODULE_PATH, FullName, $"{FullName}.module"));

			string name = "";
			string version = "";
			string description = "";
			string arguments = "";
			List<string> commands = new List<string>();
			foreach (string line in lines)
			{
				if (line.StartsWith("#")) { continue; }
				else if (line.StartsWith("NAME:")) { name = line.Split(":")[1]; }
				else if (line.StartsWith("VERSION:")) { version = line.Split(":")[1]; }
				else if (line.StartsWith("DESCRIPTION:")) { description = line.Split(":")[1]; }
				else if (line.StartsWith("ARGUMENTS:")) { arguments = line.Split(":")[1]; }
				else if (line.StartsWith("CMD:"))
				{
					List<string> split = line.Split(":").ToList();
					split.RemoveAt(0);
					string finalCommand = "";
					foreach (string command_split in split)
					{
						finalCommand += command_split;
					}
					commands = commands.Append(finalCommand).ToList();
				}

				else
				{
					continue;
				}
			}
			if (name == null || description == null || arguments == null || version == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(arguments) || string.IsNullOrEmpty(version))
			{
				return;
			}
			Module module = new Module(_name: name, _description: description, _version: version, _commands: commands.ToArray());
			InstalledModules = InstalledModules.Append(module).ToList();
		}

		public static void LoadAllModules()
		{
			List<string> modules = Directory.GetFiles(MODULE_PATH, "*.module", SearchOption.AllDirectories).ToList();
			foreach (string module in modules)
			{
				AppendModule(Path.GetFileNameWithoutExtension(module));
			}
		}
		private static string BuildPowershellCommand(string[] commands)
		{
			string finalCommand = "-Command &{";
			foreach (string command in commands)
			{
				finalCommand += command;
				finalCommand += "; ";
			}
			finalCommand += "} ";
			return finalCommand;
		}

		public static void RunModule(Module module)
		{
			if (!module.Name.StartsWith("sys"))
			{
				if (Windows.Dialogs.ShowMessageBox($"Do you want to run the module {module.Name}?", "Modules", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
				{
					Process.Start("powershell.exe", BuildPowershellCommand(module.GetCommands()));
				}
			}
		}
		public static Module[] GetModules()
		{
			return InstalledModules.ToArray();
		}
	}
}