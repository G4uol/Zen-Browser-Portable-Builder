using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ZenLauncher {
    class Program {
        // =============================================================
        // 仓库地址
        // =============================================================
        const string GITHUB_REPO = "CyLoiMe/Zen-Browser-Portable-Builder"; 
        
        const string LOG_FILE = "launcher_log.txt";

        static void Main(string[] args) {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string appDir = Path.Combine(baseDir, @"App\ZenBrowser");
            string exePath = Path.Combine(appDir, "zen.exe");
            string profileDir = Path.Combine(baseDir, @"Data\profile");
            
            string versionFile = Path.Combine(baseDir, @"App\AppInfo\version.txt");
            string iniFile = Path.Combine(baseDir, @"App\AppInfo\appinfo.ini");

            // 每次启动清理旧日志，保持清爽，方便查看本次运行结果
            try { if (File.Exists(LOG_FILE)) File.Delete(LOG_FILE); } catch {}

            try {
                if (!File.Exists(exePath)) {
                    string msg = "Error: zen.exe not found in: " + appDir;
                    LogError(msg);
                    MessageBox.Show(msg, "Zen Browser Portable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Directory.Exists(profileDir)) Directory.CreateDirectory(profileDir);

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = exePath;
                psi.Arguments = "-profile \"" + profileDir + "\" -no-remote " + string.Join(" ", args);
                psi.UseShellExecute = false;
                Process.Start(psi);

                if (!string.IsNullOrEmpty(GITHUB_REPO)) {
                    // 改名为 CheckUpdateVerbose (话痨模式)
                    Thread updateThread = new Thread(() => CheckUpdateVerbose(versionFile, iniFile));
                    updateThread.IsBackground = true;
                    updateThread.Start();
                }

            } catch (Exception ex) {
                LogError("Critical Launch Error: " + ex.ToString());
            }
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

        // === 话痨模式更新检查 ===
        static void CheckUpdateVerbose(string txtPath, string iniPath) {
            try {
                // 1. 记录本地版本
                string localVer = GetLocalVersion(txtPath, iniPath);
                LogError(string.Format("Step 1: Local Version is [{0}]", localVer));

                // 2. 记录正在连接的 URL
                string url = "https://api.github.com/repos/" + GITHUB_REPO + "/releases/latest";
                LogError(string.Format("Step 2: Checking URL -> {0}", url));

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "ZenPortableCheck");
                
                string json = client.DownloadString(url);

                // 3. 解析并记录远程版本
                string search = "\"tag_name\":";
                int idx = json.IndexOf(search);
                if (idx != -1) {
                    int start = json.IndexOf("\"", idx + search.Length) + 1;
                    int end = json.IndexOf("\"", start);
                    string remoteVer = json.Substring(start, end - start);

                    LogError(string.Format("Step 3: Remote Version found -> [{0}]", remoteVer));

                    // 4. 强制记录对比结果
                    // 只要字符串不完全相等（忽略大小写），就是 YES
                    bool isDifferent = !remoteVer.Trim().Equals(localVer.Trim(), StringComparison.OrdinalIgnoreCase);
                    LogError(string.Format("Step 4: Comparison Result -> Different? {0}", isDifferent ? "YES" : "NO"));

                    if (isDifferent) {
                        LogError("Step 5: Triggering Update Popup...");
                        string uiMsg = string.Format("New version available!\n\nLocal: {0}\nLatest: {1}\n\nDo you want to go to the download page?", localVer, remoteVer);
                        DialogResult dr = MessageBox.Show(uiMsg, "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (dr == DialogResult.Yes) {
                            Process.Start("https://github.com/" + GITHUB_REPO + "/releases");
                        }
                    } else {
                        LogError("Step 5: Versions match. Going to sleep.");
                    }
                } else {
                    LogError("Error: 'tag_name' not found in JSON response.");
                }
            } catch (Exception ex) {
                LogError("Fatal Error: " + ex.Message);
            }
        }
    }
}
