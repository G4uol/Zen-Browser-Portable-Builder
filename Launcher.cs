using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ZenLauncher {
    class Program {
        // =============================================================
        // 你的仓库地址
        // =============================================================
        const string GITHUB_REPO = "CyLoiMe/Zen-Browser-Portable-Builder"; 

        static void Main(string[] args) {
            try {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string appDir = Path.Combine(baseDir, @"App\ZenBrowser");
                string exePath = Path.Combine(appDir, "zen.exe");
                string profileDir = Path.Combine(baseDir, @"Data\profile");
                
                // 定义版本文件路径
                string versionFile = Path.Combine(baseDir, @"App\AppInfo\version.txt");
                string iniFile = Path.Combine(baseDir, @"App\AppInfo\appinfo.ini");

                // 启动检查
                if (!File.Exists(exePath)) {
                    MessageBox.Show("Error: zen.exe not found!", "Zen Browser Portable");
                    return;
                }

                if (!Directory.Exists(profileDir)) Directory.CreateDirectory(profileDir);

                // 启动浏览器
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = exePath;
                psi.Arguments = "-profile \"" + profileDir + "\" -no-remote " + string.Join(" ", args);
                psi.UseShellExecute = false;
                Process.Start(psi);

                // === 调试模式：不在后台运行，直接在前台运行，卡住界面也要看结果 ===
                if (!string.IsNullOrEmpty(GITHUB_REPO)) {
                    // 直接运行检查，不使用 Thread，确保你能看到弹窗
                    CheckUpdateDebug(versionFile, iniFile);
                }

            } catch (Exception ex) {
                MessageBox.Show("Launch Error: " + ex.Message);
            }
        }

        static string GetLocalVersion(string txtPath, string iniPath) {
            if (File.Exists(txtPath)) return File.ReadAllText(txtPath).Trim();
            if (File.Exists(iniPath)) {
                string[] lines = File.ReadAllLines(iniPath);
                foreach (string line in lines) {
                    if (line.StartsWith("DisplayVersion=", StringComparison.OrdinalIgnoreCase)) {
                        return line.Split('=')[1].Trim();
                    }
                }
            }
            return "UNKNOWN";
        }

        static void CheckUpdateDebug(string txtPath, string iniPath) {
            try {
                // 1. 获取本地版本
                string localVer = GetLocalVersion(txtPath, iniPath);
                
                // 2. 设置网络 TLS 1.2
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "ZenPortableCheck/Debug");
                
                // 3. 请求 GitHub API
                string url = "https://api.github.com/repos/" + GITHUB_REPO + "/releases/latest";
                
                // 【调试信息 1】告诉用户正在连接
                // MessageBox.Show("Checking URL: " + url + "\nLocal Version: " + localVer, "Debug Step 1");

                string json = client.DownloadString(url);

                // 4. 解析 JSON
                string search = "\"tag_name\":";
                int idx = json.IndexOf(search);
                if (idx != -1) {
                    int start = json.IndexOf("\"", idx + search.Length) + 1;
                    int end = json.IndexOf("\"", start);
                    string remoteVer = json.Substring(start, end - start);

                    // 【调试信息 2】显示对比结果 (这是最重要的！)
                    string msg = $"=== Debug Report ===\n\n" +
                                 $"Local Version:  [{localVer}]\n" +
                                 $"Remote Version: [{remoteVer}]\n\n" +
                                 $"Are they different? {(localVer != remoteVer ? "YES" : "NO")}";
                    
                    MessageBox.Show(msg, "Update Check Result");

                    // 5. 如果不同，打开网页
                    if (localVer != remoteVer) {
                         DialogResult dr = MessageBox.Show("New version found! Open download page?", "Update", MessageBoxButtons.YesNo);
                         if (dr == DialogResult.Yes) {
                             Process.Start("https://github.com/" + GITHUB_REPO + "/releases");
                         }
                    }
                } else {
                    MessageBox.Show("Error: Could not find 'tag_name' in JSON response.", "Json Parse Error");
                }
            } catch (WebException webEx) {
                // 捕获网络错误
                MessageBox.Show("Network Error:\n" + webEx.Message, "Connection Failed");
            } catch (Exception ex) {
                // 捕获其他错误
                MessageBox.Show("General Error:\n" + ex.Message, "Crash");
            }
        }
    }
}
