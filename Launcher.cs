using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/');
            string zenExe = Path.Combine(baseDir, "App", "ZenBrowser", "zen.exe");

            if (!File.Exists(zenExe))
            {
                MessageBox.Show(
                    "zen.exe not found!\n\nExpected: " + zenExe + "\n\nBase dir: " + baseDir,
                    "Launch Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(1);
            }

            // Optional debug (uncomment to see path on launch)
            // MessageBox.Show("Launching: " + zenExe);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = zenExe,
                WorkingDirectory = Path.GetDirectoryName(zenExe),
                UseShellExecute = true
            };

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
