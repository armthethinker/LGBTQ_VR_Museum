﻿using System;
using UnityEngine;
using UnityEditor;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.ImportSDK;
using Pixyz.Plugin4Unity.Core;

namespace Pixyz.Plugin4Unity.EditorWindows
{
    /// <summary>
    /// Toolbar Menu in Unity Editor.
    /// Main access point for the Pixyz Plugin for Unity.
    /// </summary>
    internal static class Menus {

        [MenuItem("Pixyz/Import Model", false, 7)]
        private static void ImportCADInScene() {
            string file = null;

            try {
                file = EditorExtensions.SelectFile(Formats.SupportedFormatsForFileBrowser);
            } catch (Exception exception) {
                Debug.LogError(exception);
            }

            if (string.IsNullOrEmpty(file))
                return;

            if (!Formats.IsFileSupported(file)) {
                Debug.LogError("Unsupported file format");
                return;
            }
#if (PIXYZ_LOD_TOOLS || PIXYZ_RULE_ENGINE)
            CoreImportWindow.Open(file);
#else
            Pixyz.ImportSDK.ImportWindow.Open(file);
#endif
        }

        [MenuItem("Pixyz/Import Model", true)]
        private static bool ImportCADInSceneValidate()
        {
            return Importer.RunningInstance == null;
        }

        [MenuItem("Pixyz/Reset Default Import Settings", false, 10)]
        private static void ResetImportSettingsAsset()
        {
            ImportSettings settings = AssetDatabase.LoadAssetAtPath<ImportSettings>(Preferences.PluginLocation + '/' + ImportWindow.DEFAULT_IMPORT_SETTINGS_NAME + ".asset");
            
            if (settings != null)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(settings));
            }

            settings = ScriptableObject.CreateInstance<ImportSettings>();
            settings.name = ImportWindow.DEFAULT_IMPORT_SETTINGS_NAME;
            EditorExtensions.SaveAsset(settings, ImportWindow.DEFAULT_IMPORT_SETTINGS_NAME, false, Preferences.PluginLocation);
        }

        [MenuItem("Pixyz/Create Import Settings", false, 9)]
        [MenuItem("Assets/Create/Pixyz/Import Settings", priority = 1)]
        private static void CreateImportSettingsAsset()
        {
            EditorExtensions.CreateAsset<Pixyz.ImportSDK.ImportSettings>("Import Settings");
        }

        [MenuItem("Pixyz/Open Plugin Documentation", false, 145)]
        private static void OpenDocumentation() {
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                EditorUtility.DisplayDialog("Information", $"The Pixyz Documentation webpage can't be reached. Please visit {Configuration.DocumentationURL} for the Pixyz Plugin for Unity documentation.", "Ok");
            } else {
                Application.OpenURL(Configuration.DocumentationURL);
            }
        }

        [MenuItem("Pixyz/License Manager", false, 146)]
        private static void OpenLicenseManager() {
            EditorExtensions.OpenWindow<LicensingWindow>();
        }

        [MenuItem("Pixyz/Check Compatibility", false, 147)]
        private static bool CheckCompatibility() {
            string message;
            var compatibility = Configuration.IsPluginCompatible(out message);
            if (EditorUtility.DisplayDialog("Pixyz Compatibility Check", message, "See Compatibility", "OK")) {
                Application.OpenURL("https://www.pixyz-software.com/documentations/html/2020.1/plugin4unity/Compatibility.html");
            }
            return compatibility == Compatibility.COMPATIBLE;
        }

        [MenuItem("Pixyz/Check For Update", false, 148)]
        private static void OpenUpdateWindow() {
            EditorExtensions.OpenWindow<UpdateWindow>();
        }

        [MenuItem("Pixyz/Support...", false, 200)]
        private static void OpenSupport()
        {
            bool success;
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                success = false;
            } else {
                try {
                    Application.OpenURL("https://pixyzsoftware.freshdesk.com/support/solutions/folders/43000555618");
                    success = true;
                } catch {
                    success = false;
                }
            }
            if (!success) {
                //EditorUtility.DisplayDialog("Information", $"The Pixyz Support webpage can't be reached. Please visit {Configuration.WebsiteURL} and go to the support category.", "Ok");
            }
        }

        [MenuItem("Pixyz/About Pixyz...", false, 201)]
        private static void OpenAboutWindow() {
            EditorExtensions.OpenWindow<AboutWindow>();
        }

        /*
#if UNITY_EDITOR_WIN
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [MenuItem("Pixyz/Uninstall", false, 301)]
        private static void Uninstall()
        {
            if (EditorUtility.DisplayDialog("Warning", $"You are about to remove Pixyz from this project, would you like to continue ?\n'{Preferences.PluginLocationAbsolute}'", "Yes", "No")) {
                // Get the DLL Handle
                IntPtr Handle = LoadLibrary(Plugin4Unity.NativeInterface.PixyzPlugin4Unity_dll);
                // Fully unloads the DLL
                while (FreeLibrary(Handle)) ;
                // Delete Pixyz
                Directory.Delete(Preferences.PluginLocationAbsolute, true);
            }
        }
#endif
        */
    }
}