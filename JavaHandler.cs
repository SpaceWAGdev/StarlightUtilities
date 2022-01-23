using System;
using System.Diagnostics;
using System.IO;

namespace SupportUtilities.Java
{
	public static class Installer
	{
		public static bool JavaInstalled { get; private set; } = false;
		public static string JavaPath { get; private set; } = Path.Join(Environment.CurrentDirectory, "java");

		public static string jwPath = Path.Join(JavaPath, "java", "bin", "javaw.exe");
		public static void LaunchJavaw(string arguments)
		{
			if (!JavaInstalled)
			{
				Install();
			}
			else
			{
				Process.Start(jwPath, arguments);
			}
		}
		public static bool Install()
		{
			Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "java"));
			string JavaZip = Path.Join(Environment.CurrentDirectory, "content", "java.zip");
			Process sevenZip = new Process();
			sevenZip.StartInfo.FileName = Path.Join(Environment.CurrentDirectory, "7zip", "x64", "7za.exe");
			sevenZip.StartInfo.Arguments = "x " + JavaZip + " -o" + JavaPath;
			sevenZip.Start();
			sevenZip.WaitForExit();
			if (sevenZip.ExitCode != 0)
			{
				return false;
			}
			else
			{
				JavaInstalled = true;
				return true;
			}
		}
		public static void InstallCheck()
		{
			JavaInstalled = Directory.Exists(JavaPath);
		}
		public static void InstallOptifine(string version)
		{
			switch (version)
			{
				case "1.8.9":
					Process.Start("powershell.exe", $"-Command &{{cd {Path.Join(Environment.CurrentDirectory, "content", "optifine")};& \".\\1_8_9.jar\"}}");
					break;
				case "1.12.2":
					Process.Start("powershell.exe", $"-Command &{{cd {Path.Join(Environment.CurrentDirectory, "content", "optifine")};& \".\\1_12_2.jar\"}}");
					break;
				case "LatestMinecraft":
					Process.Start("powershell.exe", $"-Command &{{cd {Path.Join(Environment.CurrentDirectory, "content", "optifine")};& \".\\latest.jar\"}}");
					break;
				default:
					break;
			}
		}
	}
}