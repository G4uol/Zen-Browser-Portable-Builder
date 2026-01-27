# Twilight Zen Browser Portable (Automated Builder)

<a name="english"></a>


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

