using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace SupportUtilities.Python
{
    public static class Utilities
    {
		[Obsolete("IsPythonInstalled is deprecated, please use Environment.PYTHON_PATH instead.")]
		public static bool IsPythonInstalled()
        {
            return File.Exists(Path.Join(System.Environment.CurrentDirectory, "python39\\python.exe"));
        }
        public static string PythonVersion(bool humanReadable = false)
        {
            if (humanReadable)
            {
                return "Python VERSION";
            }
            else
            {
                return "Python VERSION-LONG";
            }
        }
        public static string? GetLoadedScript()
        {
            return Path.GetFileName(Environment.ACTIVE_SCRIPT);
        }
        public static void EnableThirdpartyScripts()
        {
            Environment.ENABLE_THIRDPARTY = true;
        }
        public static void DisableThirdpartyScripts()
        {
            Environment.ENABLE_THIRDPARTY = false;
        }
        public static bool IsScriptThirdparty(string script)
        {
            script = Path.GetFullPath(script);
            if (Path.GetFileName(script).StartsWith("sys."))
            {
                return false;
            }
            else
            {	
                return true;
            }
        }
		public static void PipInstall(string modules)
		{
			Process.Start("powershell.exe", "-NoExit -Command &{"+Environment.PYTHON_PATH+" -m pip install "+modules+"}");
		}
    }
    public static class Environment
    {
        public static string ACTIVE_SCRIPT { get; set; } = Path.Join(System.Environment.CurrentDirectory, ".scripts", "sys.test.py");
        public static string PYTHON_PATH { get; } = Path.Join(System.Environment.CurrentDirectory, "python39", "python.exe");
        public static bool ENABLE_THIRDPARTY { get; set; } = false;
        public static void OpenScriptFile()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".py", // Default file extension
                Filter = "Python Files(*.py, *.pyw)|*.py;*.pyw|All files (*.*)|*.*"
            };

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                ACTIVE_SCRIPT = Path.GetFullPath(dialog.FileName);
            }

        }
        public static void Run(string? script = null, string arguments = "")
        {
            if (script == null)
            {
                if (Utilities.IsScriptThirdparty(ACTIVE_SCRIPT) && !ENABLE_THIRDPARTY) { return; }
                if (Utilities.IsScriptThirdparty(ACTIVE_SCRIPT))
                {
                    string messageBoxText = "Do you want to run the script " + " \"" + ACTIVE_SCRIPT + "\"?";
                    string caption = "Python";
                    MessageBoxButton button = MessageBoxButton.YesNo;
                    MessageBoxImage icon = MessageBoxImage.Question;
                    MessageBoxResult result;

                    result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                    if (result == MessageBoxResult.Yes)
                    {
                        RunScript(ACTIVE_SCRIPT);
                    }
                }
                else
                {
                    RunScript(ACTIVE_SCRIPT);
                }
            }
            else
            {
                if (Utilities.IsScriptThirdparty(script) && !ENABLE_THIRDPARTY) { return; }
                if (Utilities.IsScriptThirdparty(script))
                {
                    string messageBoxText = "Do you want to run the script " + " \"" + script + "\"?";
                    string caption = "Python";
                    MessageBoxButton button = MessageBoxButton.YesNo;
                    MessageBoxImage icon = MessageBoxImage.Question;
                    MessageBoxResult result;

                    result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                    if (result == MessageBoxResult.Yes)
                    {
                        RunScript(script, arguments);
                    }
                }
                else
                {
                    RunScript(script, arguments);
                }
            }
        }

        public static void RunScript(string script, string arguments = "")
        {
            Process.Start(PYTHON_PATH, script + " " + arguments);
        }
        public static string Log(string message)
        {
            var p = new Process();
            p.StartInfo.FileName = PYTHON_PATH;
            p.StartInfo.Arguments = PYTHON_PATH + " \n" + message + "\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.Verb = "runas";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}