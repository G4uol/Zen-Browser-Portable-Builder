using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;  // For potential MessageBox debug

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/');
            string[] possibleExePaths = new string[]
            {
                Path.Combine(baseDir, "App", "ZenBrowser", "zen.exe"),
                Path.Combine(baseDir, "App", "ZenBrowser", "zen", "zen.exe"),  // If extra subfolder
                Path.Combine(baseDir, "zen.exe")  // Rare fallback
            };

            string zenExe = null;
            foreach (var path in possibleExePaths)
            {
                if (File.Exists(path))
                {
                    zenExe = path;
                    break;
                }
            }

            if (zenExe == null)
            {
                MessageBox.Show("zen.exe not found in expected locations!\n\nBase dir: " + baseDir, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            // Optional: Debug path (comment out after testing)
            // MessageBox.Show("Launching: " + zenExe);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = zenExe,
                WorkingDirectory = Path.GetDirectoryName(zenExe),
                UseShellExecute = true  // Better for browser apps with profile args
            };

            // Pass any command-line args from portable launcher
            if (args.Length > 0)
                psi.Arguments = string.Join(" ", args);

            Process.Start(psi);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Launch failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }
    }
}
