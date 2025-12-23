using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ZenLauncher {
    class Program {
        // =============================================================
        // 你的仓库地址
        // =============================================================
        const string GITHUB_REPO = "CyLoiMe/Zen-Browser-Portable-Builder"; 
        
        const string LOG_FILE = "launcher_log.txt";
        const long MAX_LOG_SIZE = 1024 * 1024; 

        static void Main(string[] args) {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string appDir = Path.Combine(baseDir, @"App\ZenBrowser");
            string exePath = Path.Combine(appDir, "zen.exe");
            string profileDir = Path.Combine(baseDir, @"Data\profile");
            string versionFile = Path.Combine(baseDir, @"App\AppInfo\version.txt");
            string iniFile = Path.Combine(baseDir, @"App\AppInfo\appinfo.ini");

            // 1. 先清理旧日志
            CleanLogFile();

            try {
                if (!File.Exists(exePath)) {
                    string msg = "Error: zen.exe not found in: " + appDir;
                    LogError(msg);
                    MessageBox.Show(msg, "Zen Browser Portable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Directory.Exists(profileDir)) Directory.CreateDirectory(profileDir);

                // 2. 启动浏览器 (用户立刻看到界面)
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = exePath;
                psi.Arguments = "-profile \"" + profileDir + "\" -no-remote " + string.Join(" ", args);
                psi.UseShellExecute = false;
                Process.Start(psi);

                // 直接在当前线程检查更新
                // 只有检查完、写完日志，启动器才会退出
                if (!string.IsNullOrEmpty(GITHUB_REPO)) {
                    CheckUpdateVerbose(versionFile, iniFile);
                }

            } catch (Exception ex) {
                LogError("Critical Launch Error: " + ex.ToString());
            }
        }

        static void CleanLogFile() {
            try {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE);
                if (File.Exists(logPath)) {
                    FileInfo fi = new FileInfo(logPath);
                    if (fi.Length > MAX_LOG_SIZE) {
                        File.Delete(logPath);
                        LogError("Log file reset.");
                    }
                }
            } catch { }
        }

        static void LogError(string message) {
            try {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE);
                string timeStamp = DateTime.Now.ToString("HH:mm:ss");
                string content = string.Format("[{0}] {1}\r\n", timeStamp, message);
                File.AppendAllText(logPath, content);
            } catch { }
        }

        static string GetLocalVersion(string txtPath, string iniPath) {
            try {
                if (File.Exists(txtPath)) return File.ReadAllText(txtPath).Trim();
                if (File.Exists(iniPath)) {
                    string[] lines = File.ReadAllLines(iniPath);
                    foreach (string line in lines) {
                        if (line.StartsWith("DisplayVersion=", StringComparison.OrdinalIgnoreCase)) {
                            string[] parts = line.Split('=');
                            if (parts.Length > 1) return parts[1].Trim();
                        }
                    }
                }
            } catch (Exception ex) { LogError("Read Local Version Failed: " + ex.Message); }
            return "UNKNOWN";
        }

        static void CheckUpdateVerbose(string txtPath, string iniPath) {
            try {
                // 强制写日志：开始检查
                LogError("=== Update Check Started ===");
                
                string localVer = GetLocalVersion(txtPath, iniPath);
                LogError(string.Format("Local Version: [{0}]", localVer));

                string url = "https://api.github.com/repos/" + GITHUB_REPO + "/releases/latest";
                LogError(string.Format("Target URL: {0}", url));

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "ZenPortableCheck");
                
                string json = client.DownloadString(url);

                string search = "\"tag_name\":";
                int idx = json.IndexOf(search);
                if (idx != -1) {
                    int start = json.IndexOf("\"", idx + search.Length) + 1;
                    int end = json.IndexOf("\"", start);
                    string remoteVer = json.Substring(start, end - start);

                    LogError(string.Format("Remote Version: [{0}]", remoteVer));

                    bool isDifferent = !remoteVer.Trim().Equals(localVer.Trim(), StringComparison.OrdinalIgnoreCase);
                    LogError(string.Format("Result: Need Update? {0}", isDifferent ? "YES" : "NO"));

                    if (isDifferent) {
                        string uiMsg = string.Format("New version available!\n\nLocal: {0}\nLatest: {1}\n\nDo you want to go to the download page?", localVer, remoteVer);
                        DialogResult dr = MessageBox.Show(uiMsg, "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (dr == DialogResult.Yes) {
                            Process.Start("https://github.com/" + GITHUB_REPO + "/releases");
                        }
                    }
                } else {
                    LogError("Error: 'tag_name' missing in JSON.");
                }
            } catch (Exception ex) {
                LogError("Update Check Error: " + ex.Message);
            } finally {
                LogError("=== Update Check Finished ===");
            }
        }
    }
}
