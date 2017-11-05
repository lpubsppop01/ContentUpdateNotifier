using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace lpubsppop01.ContentUpdateNotifier
{
    /// <summary>
    /// Utility class that provides method to install shortcut for application.
    /// <para>Required: References to IWshRuntimeLibrary and WindowsAPICodePack (README.md for details)</para>
    /// </summary>
    static class StartMenuShortcutInstaller
    {
        public static bool? Install(string appID, string appName)
        {
            var shortcutPath = CreateShortcutPath(appName);
            try
            {
                bool modified = false;
                modified |= CreateShortcut(shortcutPath);
                modified |= SetAppIDToShortcut(shortcutPath, appID);
                return modified ? true : (bool?)null;
            }
            catch { return false; }
        }

        static string CreateShortcutPath(string appName)
        {
            string sanitizedAppName = appName;
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                sanitizedAppName = sanitizedAppName.Replace(c, ' ');
            }
            string filename = sanitizedAppName + ".lnk";
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", filename);
        }

        static bool CreateShortcut(string shortcutPath)
        {
            if (File.Exists(shortcutPath)) return false;
            string targetPath = Assembly.GetEntryAssembly().Location;
            var shell = new IWshRuntimeLibrary.WshShell();
            var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            try
            {
                shortcut.TargetPath = targetPath;
                shortcut.IconLocation = targetPath + ",0";
                shortcut.Save();
            }
            finally
            {
                Marshal.FinalReleaseComObject(shortcut);
                Marshal.FinalReleaseComObject(shell);
            }
            return true;
        }

        static bool SetAppIDToShortcut(string shortcutPath, string appID)
        {
            var shortcut = ShellObject.FromParsingName(shortcutPath);
            if (shortcut == null) return false;
            var appIDProp = shortcut.Properties.GetProperty(SystemProperties.System.AppUserModel.ID);
            if (appIDProp != null && appIDProp.ValueAsObject is string && (string)appIDProp.ValueAsObject == appID) return false;
            using (var writer = shortcut.Properties.GetPropertyWriter())
            {
                writer.WriteProperty(SystemProperties.System.AppUserModel.ID, appID);
            }
            return true;
        }
    }
}
