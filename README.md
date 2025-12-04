# Zen Browser Portable (Automated Builder)

# Zen Browser 便携版 (自动化构建器)

[English](https://www.google.com/search?q=%23english) | [中文](https://www.google.com/search?q=%23chinese)

-----

\<a name="english"\>\</a\>


This project uses **GitHub Actions** to automatically build the **Portable Edition** of Zen Browser. It follows the **PortableApps.com Format** standard, allowing it to work as standalone software or integrate perfectly into the PortableApps platform.

### ✨ Key Features

  * **🚀 Fully Automated Build**: Automatically checks the official Zen Browser repository daily at **UTC 02:00 (10:00 Beijing Time)**. If a new version (containing a Windows installer) is released, it automatically downloads, extracts, packages, and publishes it to the **Releases** page of this repository.
  * **💾 Native Portable**: All user data (bookmarks, extensions, history) is stored in the `Data\profile` directory. No traces are left on the host machine after unplugging your USB drive.
  * **⚡ Smart Launcher**:
      * **Instant Start**: Launches the browser immediately without blocking startup for update checks.
      * **Background Check**: Silently checks GitHub for new versions in the background after launch. If a new version is found, a popup notification will appear.
  * **🛡️ Safety Policy**:
      * **Internal Updater Disabled**: The browser's built-in auto-update feature is forcibly disabled via the `policies.json` policy file.
      * **Reason**: To prevent the browser from self-updating, which could break the portable structure, cause version conflicts, or leave junk files on the host machine. Updates are managed entirely by the Launcher and this repository.

### 📥 How to Use

#### Method 1: Standalone Use (Recommended)

1.  Go to the **Releases** page of this repository.
2.  Download the latest `ZenBrowserPortable_x.x.x.zip` archive.
3.  Unzip it to any location (USB drive recommended).
4.  Double-click **`ZenBrowserPortable.exe`** to start.

#### Method 2: Integrate into PortableApps.com Platform

If you use the PortableApps menu:

1.  Download and unzip the archive.
2.  Drag the extracted **`ZenBrowserPortable`** folder into the **`PortableApps`** directory on your drive.
      * *Correct Path Example:* `X:\PortableApps\ZenBrowserPortable\ZenBrowserPortable.exe`
3.  Open the PortableApps menu, click **"Apps"** -\> **"Refresh App Icons"**.
4.  Zen Browser Portable will automatically appear in the list.

### 🔄 How to Update

When the launcher notifies you of a new version, please follow these steps to update (**This keeps all your data safe**):

1.  Download the latest `ZenBrowserPortable_x.x.x.zip`.
2.  Unzip the archive.
3.  **Overwrite** the old files with the new ones.
      * ✅ **Overwrite**: The `App` folder, `ZenBrowserPortable.exe`, and `appinfo.ini`.
      * 🛡️ **DO NOT DELETE**: The **`Data`** folder (This contains your bookmarks and passwords\!).
4.  Done\! Launch again to use the new version.

### 📦 Advanced: Manually Build .paf.exe (Optional)

> **Note**: The ZIP file provided in Releases is fully functional. Unless you need a self-extracting installer, there is **no need** to package it as a `.paf.exe`.

Since this project automatically generates all compliant metadata (icons, config, directory structure), you can easily package it locally:

1.  Download and unzip `ZenBrowserPortable_x.x.x.zip`.
2.  Download the official [PortableApps.com Installer](https://portableapps.com/apps/development/portableapps.com_installer) or use the **`InstallerTool.zip`** provided in the Releases.
3.  Run the packaging tool and select your unzipped `ZenBrowserPortable` folder.
4.  Wait a moment, and a `.paf.exe` installer will be generated.

### 🛠️ Self-Build (Fork)

If you want to control the build process yourself or modify the configuration, you can **Fork** this project:

1.  Click the **Fork** button at the top right.
2.  **Critical Modification**:
      * Open `Launcher.cs` in your forked repo.
      * Find the line `const string GITHUB_REPO`.
      * Change it to **YourUserName/YourRepoName** (e.g., `"YourName/Zen-Browser-Portable"`). Otherwise, the launcher will check the original repository for updates.
3.  **Enable Actions**:
      * Go to the **Actions** tab in your forked repository.
      * Click the green button to enable workflows.
4.  **Schedule**:
      * The script is set to check automatically every day at **UTC 02:00**.
      * You can also manually trigger a build by clicking "Run workflow" in the Actions tab.

### 📄 Disclaimer

This is an unofficial build. Zen Browser belongs to the Zen Team. This project only provides automated packaging scripts and does not modify the core browser binaries.

-----

\<a name="chinese"\>\</a\>


本项目通过 **GitHub Actions** 自动构建 **Zen Browser** 的便携版（Portable Edition）。它遵循 **PortableApps.com** 格式标准，既可以作为独立软件运行，也可以完美集成到 PortableApps 平台中。

### ✨ 主要特性

  * **🚀 全自动构建**：每天 **UTC 02:00 (北京时间 10:00)** 自动检查 Zen Browser 官方仓库。如果有新版本发布（且包含 Windows 安装包），会自动下载、解压、打包并发布到本仓库的 Releases 页面。
  * **💾 原生便携**：所有用户数据（书签、扩展、历史记录）均存储在 `Data\profile` 目录下，拔掉 U 盘不留痕迹。
  * **⚡ 智能启动器**：
      * **秒开体验**：启动时不进行阻塞式检查，直接打开浏览器。
      * **后台检查**：启动后在后台静默检查 GitHub 是否有新版本。如果有，会弹窗提醒。
  * **🛡️ 安全策略**：
      * **已禁用内部自动更新**：通过注入 `policies.json` 策略文件，强制禁用了浏览器自带的自动更新功能。
      * **原因**：防止浏览器自动升级导致便携结构破坏、版本冲突或在宿主机留下垃圾文件。更新完全由启动器和本仓库接管。

### 📥 使用方法

#### 方式一：独立使用 (推荐)

1.  前往本仓库的 **Releases** 页面。
2.  下载最新的 `ZenBrowserPortable_x.x.x.zip` 压缩包。
3.  解压到任意位置（推荐解压到 U 盘）。
4.  双击 **`ZenBrowserPortable.exe`** 即可启动。

#### 方式二：集成到 PortableApps.com Platform

如果你使用 PortableApps 菜单：

1.  下载并解压压缩包。
2.  将解压得到的 **`ZenBrowserPortable`** 文件夹，完整的拖入 PortableApps 的 **`PortableApps`** 目录下。
      * *正确路径示例：* `X:\PortableApps\ZenBrowserPortable\ZenBrowserPortable.exe`
3.  打开 PortableApps 菜单，点击 **"应用 (Apps)"** -\> **"刷新应用图标 (Refresh App Icons)"**。
4.  Zen Browser Portable 将会自动出现在列表中。

### 🔄 更新方法

当启动器弹窗提示有新版本，请按以下步骤更新（**这能保留你的所有数据**）：

1.  下载最新的 `ZenBrowserPortable_x.x.x.zip`。
2.  解压压缩包。
3.  将解压出来的文件 **覆盖** 到你旧的文件夹中。
      * ✅ **覆盖**：`App` 文件夹、`ZenBrowserPortable.exe`、`appinfo.ini`。
      * 🛡️ **千万不要删除**：**`Data`** 文件夹（这里面是你的书签和密码！）。
4.  完成！再次启动即为最新版。

### 📦 高级：手动打包 .paf.exe (可选)

> **提示**：Releases 提供的 ZIP 包解压后即可完美使用。除非你需要一个自解压的安装程序，否则**没有必要**打包成 .paf.exe。

由于本项目已经自动生成了所有符合规范的元数据（图标、配置、目录结构），你可以非常轻松地在本地打包：

1.  下载并解压 `ZenBrowserPortable_x.x.x.zip`。
2.  下载官方的 [PortableApps.com Installer](https://portableapps.com/apps/development/portableapps.com_installer) 或使用 Releases 提供的 **`InstallerTool.zip`**。
3.  运行打包工具，选择你解压出来的 `ZenBrowserPortable` 文件夹。
4.  稍等一段时间，即可生成 `.paf.exe` 安装包。

### 🛠️ 关于自行构建 (Fork)

如果你希望自己控制构建流程，或者修改配置，可以 **Fork** 本项目：

1.  点击右上角的 **Fork** 按钮。
2.  **关键修改**：
      * 你需要在 `Launcher.cs` 文件中，找到 `const string GITHUB_REPO` 这一行。
      * 将其修改为 **你自己的用户名/仓库名**（例如 `"YourName/Zen-Browser-Portable"`）。否则启动器会去检查这个原仓库的更新。
3.  **启用 Actions**：
      * 进入你 Fork 后的仓库，点击 **Actions** 标签页。
      * 点击绿色按钮启用 Workflows。
4.  **运行计划**：
      * 脚本默认设定为每天 **UTC 02:00** 自动检查。
      * 你也可以在 Actions 页面手动点击 "Run workflow" 立即触发构建。

### 📄 免责声明

本项目是非官方构建版本。Zen Browser 的所有权归 Zen Team 所有。本项目仅提供自动化打包脚本，不修改浏览器核心二进制文件。
