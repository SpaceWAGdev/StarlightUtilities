using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SupportUtilities
{
	public static class VSTPlugins
	{
		public static string STEINBERG_PATH { get; private set; } = "";
		public static string PROGRAMFILES_PATH { get; private set; } = "";

		public static void InstallPlugin(string path, string mode = "programfiles")
		{
			Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "java"));
			Process sevenZip = new Process();
			sevenZip.StartInfo.FileName = Path.Join(Environment.CurrentDirectory, "7zip", "x64", "7za.exe");
			if(mode == "programfiles")
			{
				sevenZip.StartInfo.Arguments = "x " + path + " -o" + PROGRAMFILES_PATH;
			}
			else
			{
				sevenZip.StartInfo.Arguments = "x " + path + " -o" + PROGRAMFILES_PATH;
			}
			sevenZip.Start();
			sevenZip.WaitForExit();
		}
	}
}
