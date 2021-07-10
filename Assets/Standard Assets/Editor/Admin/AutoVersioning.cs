// namespace Admin
// {
// #if UNITY_EDITOR
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
//  
// [InitializeOnLoad]
// public class AutoVersioning : UnityEditor.Build.IPreprocessBuild
// {
//     private const string versionLabel = "Automated";
//     static AutoVersioning()
//     {
//         if (PlayerSettings.bundleVersion != versionLabel)
//             PlayerSettings.bundleVersion = versionLabel;
//     }
//  
//     public int callbackOrder { get { return 0; } }
//     public void OnPreprocessBuild(BuildTarget target, string path)
//     {
//         string newVersion = GetGitVersion();
//         PlayerSettings.bundleVersion = newVersion;
//         AssetDatabase.SaveAssets();
//         Debug.Log("Build version set to " + newVersion);
//     }
//  
//     [UnityEditor.Callbacks.PostProcessBuild]
//     public static void OnPostprocessBuild(BuildTarget target, string buildPath)
//     {
//         //rename the file here
//     }
//  
//     public static string GetGitVersion()
//     {
//         try
//         {
//             // Get the short commit hash of the current branch.
//             string cmdArguments = "git describe 2>&1";
//             var procStartInfo = new System.Diagnostics.ProcessStartInfo("wsl.exe", cmdArguments);
//  
//             // The following commands are needed to redirect the standard output.
//             // This means that it will be redirected to the Process.StandardOutput StreamReader.
//             procStartInfo.RedirectStandardOutput = true;
//             procStartInfo.UseShellExecute = false;
//  
//             // Do not create the black window.
//             procStartInfo.CreateNoWindow = true;
//  
//             // Now we create a process, assign its ProcessStartInfo and start it
//             System.Diagnostics.Process proc = new System.Diagnostics.Process();
//             proc.StartInfo = procStartInfo;
//             proc.Start();
//  
//             // Get the output into a string
//             return proc.StandardOutput.ReadToEnd();
//         }
//         catch
//         {
//             Debug.LogError("Unable to get git hash.");
//             return "unable to get version";
//         }
//     }
// }
// #endif
// }