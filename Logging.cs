using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SupportUtilities
{
    static internal class Logging
    {
        public static string LOG_FILE { get; set; } = Path.Join(Environment.CurrentDirectory, "settings", "log.txt");

        public static void Log(string message)
        {
            File.AppendAllText(LOG_FILE, DateTime.UtcNow.ToString() + "\t" +  message + Environment.NewLine);
        }
        public static void ViewLog()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "notepad.exe",
                Arguments = LOG_FILE,
                Verb = "open"
            };
            Process.Start(startInfo);
        }
    }
}
