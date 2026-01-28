# Twilight Zen Browser Portable (Automated Builder)



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
2.  Download the latest `ZenBrowserPortable_Twilight_Twilight_build_x.x.x.zip` archive.
3.  Unzip it to any location (USB drive recommended).
4.  Double-click **`ZenBrowserPortable_Twilight_Twilight_build`** to start.
5.  You can overwrite everything, it does not overwrite your profile data, so all your pinned tabs & workspaces/extensions and intact.

#### Method 2: Integrate into PortableApps.com Platform

### 🔄 How to Update


### 📄 Disclaimer

This is an unofficial build. Zen Browser belongs to the Zen Team. This project only provides automated packaging scripts and does not modify the core browser binaries.

