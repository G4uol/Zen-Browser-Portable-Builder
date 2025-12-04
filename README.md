# Zen Browser Portable (Automated Builder)

本项目通过 GitHub Actions 自动构建 **Zen Browser** 的便携版（Portable Edition）。它遵循 PortableApps.com 格式标准，既可以作为独立软件运行，也可以完美集成到 PortableApps 平台中。

## ✨ 主要特性

  * **全自动构建**：每天 **UTC 02:00 (北京时间 10:00)** 自动检查 Zen Browser 官方仓库。如果有新版本发布（且包含 Windows 安装包），会自动下载、解压、打包并发布到本仓库的 [Releases](https://github.com/CyLoiMe/Zen-Browser-Portable-Builder/releases) 页面。
  * **原生便携**：所有用户数据（书签、扩展、历史记录）均存储在 `Data\profile` 目录下，拔掉 U 盘不留痕迹。
  * **智能启动器**：
      * **秒开体验**：启动时不进行阻塞式检查，直接打开浏览器。
      * **后台检查**：启动后在后台静默检查 GitHub 是否有新版本。如果有，会弹窗提醒。
  * **安全策略**：
      * 🚫 **已禁用内部自动更新**：通过注入 `policies.json` 策略文件，强制禁用了浏览器自带的自动更新功能。
      * **原因**：防止浏览器自动升级导致便携结构破坏、版本冲突或在宿主机留下垃圾文件。更新完全由启动器和本仓库接管。

## 📥 使用方法

### 方式一：独立使用

1.  前往本仓库的 [**Releases**](https://github.com/CyLoiMe/Zen-Browser-Portable-Builder/releases) 页面。
2.  下载最新的 `ZenBrowserPortable_x.x.x.zip` 压缩包。
3.  解压到任意位置（推荐解压到 U 盘）。
4.  双击 **`ZenBrowserPortable.exe`** 即可启动。

### 方式二：集成到 PortableApps.com Platform

如果你使用 PortableApps 菜单：

1.  下载并解压压缩包。
2.  将解压得到的 **`ZenBrowserPortable`** 文件夹，完整的拖入 PortableApps 的 **`PortableApps`** 目录下。
      * *正确路径示例：* `X:\PortableApps\ZenBrowserPortable\ZenBrowserPortable.exe`
3.  打开 PortableApps 菜单，点击 **"应用 (Apps)"** -\> **"刷新应用图标 (Refresh App Icons)"**。
4.  Zen Browser Portable 将会自动出现在列表中。

-----

## 🔄 更新方法

当启动器弹窗提示有新版本，请按以下步骤更新（**这能保留你的所有数据**）：

1.  下载最新的 `ZenBrowserPortable_x.x.x.zip`。
2.  解压压缩包。
3.  将解压出来的文件 **覆盖** 到你旧的文件夹中。
      * ✅ **需要覆盖**：`App` 文件夹、`ZenBrowserPortable.exe`、`appinfo.ini`。
      * 🛡️ **不要删除**：**`Data`** 文件夹（这里面是你的书签和密码！）。
4.  完成！再次启动即为最新版。

-----

## 🛠️ 关于自行构建 (Fork)

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

-----

## 📄 免责声明

本项目是非官方构建版本。Zen Browser 的所有权归 Zen Team 所有。本项目仅提供自动化打包脚本，不修改浏览器核心二进制文件。
