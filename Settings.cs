using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SupportUtilities
{
	public static class Settings
	{
		public static string SETTINGS_PATH { get; } = Path.Join(Environment.CurrentDirectory, "settings", "settings.txt");
		public static Dictionary<string, string> settings = new Dictionary<string, string>();

		public static void LoadSettingsFromFile()
		{
			string[] lines = File.ReadAllLines(SETTINGS_PATH);
			foreach (string line in lines)
			{
				string editableLine = line.Trim().ToUpper();
				string[] pair = editableLine.Split(":");
				if (pair.Length != 2) { continue; }
				KeyValuePair<string, string> pairToPass = new KeyValuePair<string, string>(pair[0], pair[1]);
				if (settings.ContainsKey(pairToPass.Key))
				{
					settings[pairToPass.Key] = pairToPass.Value;
				}
				else
				{
					settings.Add(pairToPass.Key, pairToPass.Value);
				}
			}

			#region Old version - blocks SupportUtilities.Settings.SaveSettings() from accessing the file
			// Old version - blocks SupportUtilities.Settings.SaveSettings() from accessing the file

			//const Int32 BufferSize = 128;
			//using (var fileStream = File.OpenRead(fileName))
			//using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
			//{
			//	string line;
			//	while ((line = streamReader.ReadLine()) != null)
			//	{
			//		line = line.Trim();
			//		string[] pair = line.Split(":");
			//		if (pair.Length > 2) { continue; }
			//		KeyValuePair<string, string> pairToPass = new KeyValuePair<string, string>(pair[0], pair[1]);
			//		if (settings.ContainsKey(pairToPass.Key))
			//		{
			//			settings[pairToPass.Key] = pairToPass.Value;
			//		}
			//		else
			//		{
			//			settings.Add(pairToPass.Key, pairToPass.Value);
			//		}
			//	}
			//	streamReader.Close();
			//}
			#endregion
		}
		public static void SaveSettings()
		{
			List<string> lines = new List<string>();
			foreach (KeyValuePair<string, string> kvp in settings)
			{
				string toWrite = ($"{kvp.Key}:{kvp.Value}");
				toWrite = toWrite.ToUpper();
				lines.Add(toWrite);
			}
			File.WriteAllLines(SETTINGS_PATH, lines.ToArray());
		}

		public static string? TryGetSetting(string setting)
		{
			setting = setting.ToUpper();
			if (settings.ContainsKey(setting))
			{
				return settings[setting];
			}
			else
			{
				return null;
			}
		}
		public static void SetSetting(string setting, string value)
		{
			setting = setting.ToUpper();
			value = value.ToUpper();
			if (settings.ContainsKey(setting))
			{
				settings[setting] = value;
			}
			else
			{
				settings.Add(setting, value);
			}
		}
		public static bool? TryGetBool(string setting)
		{
			setting=setting.ToUpper();
			if (settings.ContainsKey(setting))
			{
				switch (settings[setting])
				{
					case "TRUE":
						return true;
					case "1":
						return true;
					case "YES":
						return true;
					case "Y":
						return true;
					case "ENABLED":
						return true;
					case "FALSE":
						return false;
					case "0":
						return false;
					case "NO":
						return false;
					case "N":
						return false;
					case "DISABLED":
						return false;
					default:
						return null;
				}
			}
			else
			{
				return null;
			}
		}
	}
}