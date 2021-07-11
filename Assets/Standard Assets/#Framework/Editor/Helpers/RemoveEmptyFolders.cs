using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// Remove empty folders automatically.
    /// </summary>
    public class RemoveEmptyFolders : UnityEditor.AssetModificationProcessor
    {
        public const string kMenuText = "Assets/Remove Empty Folders";
        static readonly StringBuilder s_Log = new StringBuilder();
        static readonly List<DirectoryInfo> s_Results = new List<DirectoryInfo>();

        /// <summary>
        /// Raises the initialize on load method event.
        /// </summary>
        [InitializeOnLoadMethod]
        static void OnInitializeOnLoadMethod()
        {
            EditorApplication.delayCall += () => Valid();
        }

        [DidReloadScripts]
        static void CheckOnce()
        {
            OnWillSaveAssets(null);
            //EditorPrefs.GetBool(kMenuText, false)
        }

        [MenuItem("Assets/Clear Empty Folders")]
        static void Clear()
        {
            OnWillSaveAssets(null);
        }

        [MenuItem("Assets/== Create Empty Folder ==")]
        static void CreateFolder()
        {
            var folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            Core.CreateNewFolder(folderPath);

            // if (folderPath.Contains(".")) folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
            // var dir = Directory.CreateDirectory(folderPath + "/New-Folder");
            // AssetDatabase.Refresh();
        }

        /// <summary>
        /// Raises the will save assets event.
        /// </summary>
        static string[] OnWillSaveAssets(string[] paths)
        {
            // if (EditorApplication.isCompiling || EditorApplication.isUpdating) {
            //     return paths;
            // }
            // If menu is unchecked, do nothing.
            if (!EditorPrefs.GetBool(kMenuText, false) && paths != null) {
                return paths;
            }

            var assetsDir = Application.dataPath + Path.DirectorySeparatorChar;

            // Get empty directories in Assets directory
            s_Results.Clear();
            GetEmptyDirectories(new DirectoryInfo(assetsDir), s_Results);
            GetEmptyDirectories(
                new DirectoryInfo(
                    Path.GetDirectoryName(Application.dataPath) + Path.DirectorySeparatorChar + "Packages"), s_Results);

            // When empty directories has detected, remove the directory.
            if (0 < s_Results.Count) {
                s_Log.Length = 0;
                s_Log.AppendFormat("Remove {0} empty directories as following:\n", s_Results.Count);

                // string folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
                // if (folderPath.Contains(".")) folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
                // var dirInfo = new DirectoryInfo(folderPath);
                foreach (var d in s_Results) {
                    if (d.Name == "New Folder") {
                        Debug.Log(d.FullName);
                        continue;
                    }

                    s_Log.AppendFormat("- {0}\n", d.FullName.Replace(assetsDir, ""));
                    try {
                        File.Delete(d.FullName + ".meta");
                        FileUtil.DeleteFileOrDirectory(d.FullName);
                    }
                    catch (DirectoryNotFoundException e) { }
//                    if (Directory.Exists(d.FullName) || File.Exists(d.FullName)) {
//                    }

                    //FileUtil.DeleteFileOrDirectory(d.FullName+".meta");
                }

                // UNITY BUG: Debug.Log can not set about more than 15000 characters.
                s_Log.Length = Mathf.Min(s_Log.Length, 15000);
                Debug.Log(s_Log.ToString());
                s_Log.Length = 0;
                AssetDatabase.Refresh();
            }

            return paths;
        }

        /// <summary>
        /// Toggles the menu.
        /// </summary>
        [MenuItem(kMenuText)]
        static void OnClickMenu()
        {
            // Check/Uncheck menu.
            var isChecked = !Menu.GetChecked(kMenuText);
            Menu.SetChecked(kMenuText, isChecked);

            // Save to EditorPrefs.
            EditorPrefs.SetBool(kMenuText, isChecked);
            OnWillSaveAssets(null);
        }

        [MenuItem(kMenuText, true)]
        static bool Valid()
        {
            // Check/Uncheck menu from EditorPrefs.
            Menu.SetChecked(kMenuText, EditorPrefs.GetBool(kMenuText, false));
            return true;
        }

        /// <summary>
        /// Get empty directories.
        /// </summary>
        static bool GetEmptyDirectories(DirectoryInfo dir, List<DirectoryInfo> results)
        {
            var isEmpty = true;
            try {
                isEmpty =
                    dir.GetDirectories().Count(x => !GetEmptyDirectories(x, results)) == 0 // Are sub directories empty?
                    && dir.GetFiles("*.*").All(x => x.Extension == ".meta"); // No file exist?
            }
            catch { }

            // Store empty directory to results.
            if (isEmpty) {
                results.Add(dir);
            }

            return isEmpty;
        }
    }
}