using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ZenLauncher {
    class Program {
        // =============================================================
        // 【请修改这里】你的 GitHub 仓库地址 (格式: 用户名/仓库名)
        // =============================================================
        const string GITHUB_REPO = "CyLoiMe/Zen-Browser-Portable-Builder"; 

        static void Main(string[] args) {
            try {
                // 1. 定义路径
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string appDir = Path.Combine(baseDir, @"App\ZenBrowser");
                string exePath = Path.Combine(appDir, "zen.exe");
                string profileDir = Path.Combine(baseDir, @"Data\profile");
                string versionFile = Path.Combine(baseDir, @"App\AppInfo\version.txt");

                // 2. 启动前检查
                if (!File.Exists(exePath)) {
                    MessageBox.Show("Error: zen.exe not found in:\n" + appDir + "\n\nPlease ensure you installed the Portable version correctly.", "Zen Browser Portable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. 确保数据目录存在
                if (!Directory.Exists(profileDir)) Directory.CreateDirectory(profileDir);

                // 4. 启动浏览器 (便携模式)
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = exePath;
                // -profile: 指定数据存储在 Data\profile
                // -no-remote: 允许与本地安装的 Firefox/Zen 共存
                psi.Arguments = "-profile \"" + profileDir + "\" -no-remote " + string.Join(" ", args);
                psi.UseShellExecute = false;
                
                // 启动进程
                Process.Start(psi);

                // 5. 启动后：后台静默检查更新 (不卡界面)
                if (File.Exists(versionFile) && !string.IsNullOrEmpty(GITHUB_REPO) && !GITHUB_REPO.Contains("YOUR_USERNAME")) {
                    Thread updateThread = new Thread(() => CheckUpdate(versionFile));
                    updateThread.IsBackground = true;
                    updateThread.Start();
                }

            } catch (Exception ex) {
                MessageBox.Show("Launch Error: " + ex.Message, "Zen Browser Portable");
            }
        }

        // 后台检查更新逻辑
        static void CheckUpdate(string localVerPath) {
            try {
                // 读取本地版本
                string localVer = File.ReadAllText(localVerPath).Trim();
                
                // 设置网络安全协议 (GitHub 需要 TLS 1.2)
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "ZenPortableCheck");
                
                // 获取最新 Release 信息
                string url = "https://api.github.com/repos/" + GITHUB_REPO + "/releases/latest";
                string json = client.DownloadString(url);

                // 解析 JSON 中的 "tag_name"
                string search = "\"tag_name\":";
                int idx = json.IndexOf(search);
                if (idx != -1) {
                    int start = json.IndexOf("\"", idx + search.Length) + 1;
                    int end = json.IndexOf("\"", start);
                    string remoteVer = json.Substring(start, end - start);

                    // 如果版本不同，弹窗提示
                    if (remoteVer != localVer) {
                        DialogResult dr = MessageBox.Show(
                            "New version available!\n\nLocal: " + localVer + "\nLatest: " + remoteVer + "\n\nDo you want to go to the download page?",
                            "Zen Browser Portable Update",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                        if (dr == DialogResult.Yes) {
                            Process.Start("https://github.com/" + GITHUB_REPO + "/releases");
                        }
                    }
                }
            } catch {
                // 静默失败
            }
        }
    }
}
