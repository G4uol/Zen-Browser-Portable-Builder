using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ZenLauncher {
    class Program {
        const string GITHUB_REPO = "CyLoiMe/Zen-Browser-Portable-Builder"; 
        const string LOG_FILE = "launcher_log.txt";
        // 设定日志最大为 1MB
        const long MAX_LOG_SIZE = 1024 * 1024; 

        static void Main(string[] args) {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string appDir = Path.Combine(baseDir, @"App\ZenBrowser");
            string exePath = Path.Combine(appDir, "zen.exe");
            string profileDir = Path.Combine(baseDir, @"Data\profile");
            
            string versionFile = Path.Combine(baseDir, @"App\AppInfo\version.txt");
            string iniFile = Path.Combine(baseDir, @"App\AppInfo\appinfo.ini");

            // 启动时进行日志“大扫除”
            CleanLogFile();

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
                    Thread updateThread = new Thread(() => CheckUpdateSilent(versionFile, iniFile));
                    updateThread.IsBackground = true;
                    updateThread.Start();
                }

            } catch (Exception ex) {
                LogError("Critical Launch Error: " + ex.ToString());
                MessageBox.Show("Launch Error: " + ex.Message, "Zen Browser Portable");
            }
        }

        // 清理日志逻辑
        static void CleanLogFile() {
            try {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE);
                if (File.Exists(logPath)) {
                    FileInfo fi = new FileInfo(logPath);
                    // 如果文件超过 1MB，直接删除旧的，保持清爽。
                    if (fi.Length > MAX_LOG_SIZE) {
                        File.Delete(logPath);
                        // 创建一个新文件并记录清理操作
                        LogError("Log file was too large and has been reset.");
                    }
                }
            } catch { /* 清理失败就算了，不影响使用 */ }
        }

        static void LogError(string message) {
            try {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE);
                string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                // 保留最近的记录
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
            } catch (Exception ex) {
                LogError("Failed to read local version: " + ex.Message);
            }
            return null;
        }

        static void CheckUpdateSilent(string txtPath, string iniPath) {
            try {
                string localVer = GetLocalVersion(txtPath, iniPath);
                if (string.IsNullOrEmpty(localVer)) {
                    LogError("Skipping update check: Local version not found.");
                    return;
                }

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "ZenPortableCheck");
                
                string url = "https://api.github.com/repos/" + GITHUB_REPO + "/releases/latest";
                string json = client.DownloadString(url);

                string search = "\"tag_name\":";
                int idx = json.IndexOf(search);
                if (idx != -1) {
                    int start = json.IndexOf("\"", idx + search.Length) + 1;
                    int end = json.IndexOf("\"", start);
                    string remoteVer = json.Substring(start, end - start);

                    if (!remoteVer.Trim().Equals(localVer.Trim(), StringComparison.OrdinalIgnoreCase)) {
                        string msg = string.Format("New version available! Local: {0}, Latest: {1}", localVer, remoteVer);
                        LogError(msg); 

                        string uiMsg = string.Format("New version available!\n\nLocal: {0}\nLatest: {1}\n\nDo you want to go to the download page?", localVer, remoteVer);
                        DialogResult dr = MessageBox.Show(uiMsg, "Zen Browser Portable Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (dr == DialogResult.Yes) {
                            Process.Start("https://github.com/" + GITHUB_REPO + "/releases");
                        }
                    } 
                } else {
                    LogError("JSON Parse Error: tag_name not found.");
                }
            } catch (WebException webEx) {
                string responseText = "";
                if (webEx.Response != null) {
                    using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream())) {
                        responseText = reader.ReadToEnd();
                    }
                }
                LogError(string.Format("Network Error: {0}. Details: {1}", webEx.Message, responseText));
            } catch (Exception ex) {
                LogError("General Update Error: " + ex.ToString());
            }
        }
    }
}
