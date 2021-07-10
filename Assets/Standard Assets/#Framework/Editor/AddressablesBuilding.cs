// namespace DefaultNamespace {
//
//  //using Ubiant.Common.Tools;
//     using UnityEditor;
//     using UnityEditor.AddressableAssets;
//     using UnityEditor.AddressableAssets.Settings;
//     using UnityEditor.AddressableAssets.Settings.GroupSchemas;
//     using UnityEngine;
//     using System.IO;
//     using System;
//     using System.Globalization;
//    // using Ubiant.Common.Utils;
//     using System.Collections.Generic;
//     using System.Text;
//     using UnityEngine.AddressableAssets;
//     //using Ubiant.Designer.PlanCreator;
//
//     public class AddressablesBuilding
//     {
//         private const string ADDRESSABLES_OBJECTS_ASSET_PATH = "Assets/Addressables/Objects";
//         private const string ADDRESSABLES_BUILDINGS_ASSET_PATH = "Assets/Addressables/Buildings";
//
//         public static AddressablesBuilding s_model;
//
//         public string PlatformsToBuild = "global";
//         public string VersionTag = "0.0.0";
//         public string TargetVersion = "dev";
//         public string[] BuildingList = new string[]{ "all" };
//         public AddressableAssetSettings AddressableToBuild;
//
//         /// <summary>
//         /// Method used by Gitlab builder to have an external entry point to Addressables build pipeline.true (Don't use it !)
//         /// </summary>
//         public static void BatchBuild()
//         {
//             InstallAddressablesTools();
//
//             s_model = new AddressablesBuilding();
//             // s_model.VersionTag = GlobalTools.GetCommandLineArg("BUILD_NUMBER", "0.0.0");
//             // s_model.PlatformsToBuild = GlobalTools.GetCommandLineArg("PLATFORMS", "all");
//             // s_model.TargetVersion = GlobalTools.GetCommandLineArg("ADDRESSABLE_SERVER", "all");
//             // string buildingConcatList = GlobalTools.GetCommandLineArg("BUILDING_LIST", "all");
//             //s_model.BuildingList = buildingConcatList.Split('-');
//             s_model.BuildAddressables();
//         }
//
//         [MenuItem("Ubiant/Tools/Addressables/Install tools")]
//         public static void InstallAddressablesTools()
//         {
//             System.Diagnostics.Process process = new System.Diagnostics.Process();
//             string extension = Application.platform == RuntimePlatform.WindowsEditor ? ".bat" : ".sh";
//             //string scriptPath = IOUtility.CombineMultiplePaths(Application.dataPath, "..", $"install_aws_utils{extension}");
//             string exeToUse = Application.platform == RuntimePlatform.WindowsEditor ? "cmd.exe" : "/bin/bash";
//            // Debug.Log( $"scriptPath : {scriptPath}" );
//            // process = Builder.Utils.StartProcess(exeToUse, $"\"{scriptPath}\"", "", false, "", true);
//
//             StringBuilder outputOk = new StringBuilder();
//             StringBuilder outputError = new StringBuilder();
//
//             while (!process.HasExited)
//             {
//                 outputOk.Append(process.StandardOutput.ReadToEnd());
//                 outputError.Append(process.StandardError.ReadToEnd());
//             }
//
//             Debug.Log($"Standard output : {outputOk.ToString()}");
//             Debug.LogWarning($"Standard error : {outputError.ToString()}");
//         }
//
//         [MenuItem("Ubiant/Tools/Addressables/Build")]
//         public static void BuildAddressablesMenu()
//         {
//             s_model = new AddressablesBuilding();
//             s_model.PlatformsToBuild = EditorUserBuildSettings.activeBuildTarget.ToString().ToLowerInvariant();
//             s_model.BuildAddressables();
//         }
//
//         #region Build Automation Part
//
//         /// <summary>
//         /// This allow you to build Addressables assets based on the parameters in entry.
//         /// </summary>
//         public void BuildAddressables()
//         {
//             int exitCode = 0;
//             s_model.AddressableToBuild = AddressableAssetSettingsDefaultObject.GetSettings(false);
//
//             //Just in case all word is used, we set it to a true value then we split it to use it later
//             if( s_model.TargetVersion == "all" )
//                 TargetVersion = "dev_master";
//             string[] targetVersionArray = TargetVersion.Split('_');
//
//             if( s_model.PlatformsToBuild == "all" )
//                 PlatformsToBuild = "android_ios_standalone_webgl";
//
//             // Update Collection With All Object Edition Data
//             //BuildingElementsCollection.instance.UpdateData(true);
//
//             Dictionary<BuildTarget, BuildTargetGroup> dicBuildAndGroup = GenerateBuildTargetAndGroup(s_model.PlatformsToBuild);
//             foreach( var buildAndGroup in dicBuildAndGroup )
//             {
//                 EditorUserBuildSettings.SwitchActiveBuildTarget(buildAndGroup.Value, buildAndGroup.Key);
//
//                 //We will now iterate through all target version and through all platform possibility to generate each catalog and assets per platform and server
//                 foreach( string target in targetVersionArray )
//                 {
//                     string profileID = s_model.AddressableToBuild.profileSettings.AddProfile($"AutoGeneratedProfile_{EditorUserBuildSettings.activeBuildTarget}_{target}",
//                                                                                     s_model.AddressableToBuild.profileSettings.GetProfileId("Default"));
//                     s_model.AddressableToBuild.activeProfileId = profileID;
//
//                     //This part is a workaround to avoid addressables issue until unity fixed it. Currently not fixed in 1.1.7 Addressables package.
//                     string remoteBuildPath = $"ServerData/[BuildTarget]/{target}";
//                     remoteBuildPath = remoteBuildPath.Replace("[BuildTarget]", EditorUserBuildSettings.activeBuildTarget.ToString());
//                    // string remoteLoadPath = $"{UbiantConstants.instance.AWS_ADDRESSABLES_URL}/[BuildTarget]/{target}";
//                    // remoteLoadPath = remoteLoadPath.Replace("[BuildTarget]", EditorUserBuildSettings.activeBuildTarget.ToString());
//
//                     //Set the value into the last profile created
//                     s_model.AddressableToBuild.profileSettings.SetValue(profileID,"RemoteBuildPath",remoteBuildPath);
//                     //s_model.AddressableToBuild.profileSettings.SetValue(profileID,"RemoteLoadPath",remoteLoadPath);
//
//                     //We read the remoteBuildPath to generate the complete path to the folder where all bundles / catalog are generated to use it in others operations
//                     //remoteBuildPath = remoteBuildPath.Replace("[BuildTarget]", EditorUserBuildSettings.activeBuildTarget.ToString());
//                     //string addressablesPath = IOUtility.ReplacePathSeparator( IOUtility.CombinePaths(Directory.GetCurrentDirectory(), remoteBuildPath));
//
//                     //CleanAndVersionAddressablesFolder(addressablesPath,target);
//                     //exitCode = BuildAndUploadAddressables(addressablesPath,target);
//                 }
//             }
//
//             if (Application.isBatchMode)
//                 EditorApplication.Exit(exitCode);
//         }
//
//         /// <summary>
//         /// Retrieve the BuildTarget and BuildTargetGroup based on string in entry
//         /// </summary>
//         /// <param name="platformsToBuild">string with this syntax : platform1_platformX</param>
//         /// <returns>A dictionnary with BuildTarget and BuildTargetGroup</returns>
//         private Dictionary<BuildTarget, BuildTargetGroup> GenerateBuildTargetAndGroup(string platformsToBuild)
//         {
//             Dictionary<BuildTarget, BuildTargetGroup> buildTargetAndGroup = new Dictionary<BuildTarget, BuildTargetGroup>();
//             string[] platforms = platformsToBuild.Split('_');
//             for(int platformIndex = 0 ; platformIndex < platforms.Length ; ++platformIndex)
//             {
//                 if( Enum.TryParse(platforms[platformIndex], true, out BuildTarget buildtarget) )
//                     buildTargetAndGroup.Add( buildtarget, BuildPipeline.GetBuildTargetGroup(buildtarget));
//                 else if( platforms[platformIndex].Equals("standalone") ) //Special case if the user creates a tag with standalone without specific platform
//                 {
//                     buildTargetAndGroup.Add( BuildTarget.StandaloneWindows64, BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneWindows64));
//                     buildTargetAndGroup.Add( BuildTarget.StandaloneOSX, BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneOSX));
//                     buildTargetAndGroup.Add( BuildTarget.StandaloneLinux64, BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneLinux64));
//                 }
//             }
//
//             return buildTargetAndGroup;
//         }
//
//         /// <summary>
//         /// This will clean the Addressable generation folder and will generate the folder if it doesn't exist to generate the versioning file in it.
//         /// </summary>
//         /// <param name="addressablesPath">The addressables generation path defines by an Addressable profile</param>
//         private void CleanAndVersionAddressablesFolder(string addressablesPath, string target)
//         {
//             if(!Directory.Exists(addressablesPath))
//                 Directory.CreateDirectory(addressablesPath);
//
//             //Clean up the addressable folder where assets are generated
//             DirectoryInfo di = new DirectoryInfo(addressablesPath);
//             foreach (FileInfo file in di.GetFiles())
//                 file.Delete();
//
//             foreach (DirectoryInfo dir in di.GetDirectories())
//                 dir.Delete(true);
//
//             //Create a versioning file which is basically just an empty file with necessary information in name
//             //File.Create(IOUtility.CombinePaths(addressablesPath, $"{VersionTag}_{target}_{DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss", CultureInfo.InvariantCulture)}.versioning")).Close();
//         }
//
//         /// <summary>
//         /// Build the addressables files and upload them with a script on the S3 assets server.
//         /// It will display the Standard and the Error output log just in case.
//         /// </summary>
//         /// <param name="addressablesPath">The addressables generation path defines by an Addressable profile</param>
//         private int BuildAndUploadAddressables(string addressablesPath, string target)
//         {
//             Debug.Log($"===================================> {EditorUserBuildSettings.activeBuildTarget} <=====================================");
//             Debug.Log($"===================================> {s_model.AddressableToBuild.profileSettings.GetValueByName(s_model.AddressableToBuild.activeProfileId,"RemoteBuildPath")} <=====================================");
//             AddressableAssetSettings.BuildPlayerContent();
//
//             //Iterate all catalog generated to rename them to catalog.extension instead of catalog_timestap.extension
//             foreach (string filepath in Directory.GetFiles(addressablesPath, "*catalog*"))
//             {
//                 //string simpleCatalogPath = IOUtility.CombinePaths(addressablesPath, $"catalog{Path.GetExtension(filepath)}");
//                 //File.Move(filepath, simpleCatalogPath);
//             }
//
//             string buildTarget =  EditorUserBuildSettings.activeBuildTarget.ToString();
//             string completeS3Path = $"{buildTarget}/{target}";
//
//             string exeToUse = Application.platform == RuntimePlatform.WindowsEditor ? "cmd.exe" : "/bin/bash";
//             string extension = Application.platform == RuntimePlatform.WindowsEditor ? ".bat" : ".sh";
//             //string scriptPath = IOUtility.CombineMultiplePaths(Application.dataPath, "..", $"aws_asset_copy{extension}");
//
//             System.Diagnostics.Process process = new System.Diagnostics.Process();
//             //Start the shell or batch script from C# code to upload all generated bundle on the Amazon S3 Server
//            // process = Builder.Utils.StartProcess(exeToUse, $"\"{scriptPath}\" \"{addressablesPath}\" ubiant-unity-assets {completeS3Path}", "", false, "", true);
//
//             StringBuilder outputOk = new StringBuilder();
//             StringBuilder outputError = new StringBuilder();
//
//             while (!process.HasExited)
//             {
//                 outputOk.Append(process.StandardOutput.ReadToEnd());
//                 outputError.Append(process.StandardError.ReadToEnd());
//             }
//
//             Debug.Log($"Standard output : {outputOk.ToString()}");
//             Debug.LogWarning($"Standard error : {outputError.ToString()}");
//
//             if( process.ExitCode != 0 )
//                 Debug.LogWarning($"[ADDRESSABLES]Something bad happens during assets generation on {EditorUserBuildSettings.activeBuildTarget}.");
//
//             return process.ExitCode;
//         }
//
//         #endregion
//
//         #region PreBuild part
//
//         /// <summary>
//         /// Method to clean up the addressableAssetSettings in entry before performing any operation.
//         /// </summary>
//         /// <param name="addressableAssetsSettings">AddressableAssetSettings to clean up</param>
//         public static void CleanAddressableAssetSettings(AddressableAssetSettings addressableAssetsSettings)
//         {
//             //First we clean the entire Addressable Asset Settings by removing groups
//             for (int i = addressableAssetsSettings.groups.Count - 1; i >= 0; --i)
//                 addressableAssetsSettings.RemoveGroup(addressableAssetsSettings.groups[i]);
//         }
//
//         [MenuItem("Ubiant/Tools/Addressables/Create Buildings Group Testing")]
//         public static void GenerateBuildingsAddressableGroups()
//         {
//             if (s_model.BuildingList.Length > 0)
//             {
//                 AddressableAssetSettings addressableAssetsSettings = AddressableAssetSettingsDefaultObject.GetSettings(false);
//
//                 CleanAddressableAssetSettings(addressableAssetsSettings);
//
//                 string[] allBuildingGroupToCreate = AssetDatabase.GetSubFolders(ADDRESSABLES_BUILDINGS_ASSET_PATH);
//                 List<string> buildingListToGenerate = new List<string>();
//
//                 if (s_model.BuildingList[0] != "all")
//                 {
//                     for (int idxBuildingFolder = 0; idxBuildingFolder < allBuildingGroupToCreate.Length; ++idxBuildingFolder)
//                     {
//                         for( int idxBuildingToGenerate = 0; idxBuildingToGenerate < s_model.BuildingList.Length; ++idxBuildingToGenerate )
//                         {
//                             if(allBuildingGroupToCreate[idxBuildingFolder].Contains(s_model.BuildingList[idxBuildingToGenerate]))
//                             {
//                                 buildingListToGenerate.Add(allBuildingGroupToCreate[idxBuildingFolder]);
//                             }
//                         }
//                     }
//                 }
//                 else
//                 {
//                     buildingListToGenerate.AddRange(allBuildingGroupToCreate);
//                 }
//
//                 foreach (var groupsContainerFolder in buildingListToGenerate)
//                 {
//                     GenerateGroupSchemeSet(BundledAssetGroupSchema.BundlePackingMode.PackTogether);
//                 }
//             }
//         }
//
//         [MenuItem("Ubiant/Tools/Addressables/Create Objects Group Testing")]
//         public static void GenerateObjectsAddressableGroups()
//         {
//             AddressableAssetSettings addressableAssetsSettings = AddressableAssetSettingsDefaultObject.GetSettings(false);
//
//             CleanAddressableAssetSettings(addressableAssetsSettings);
//
//             //TODO
//             //Choose the right profile here
//             //We need to use the autogenerated one so we will build it before this step and build one
//
//             //TODO
//             //Add the default group before performing the rest. BuildingCollectionElement need to be in the default group cause for now they are ignored.
//
//             FindAssetsAndCreateGroupRecursievly(addressableAssetsSettings, ADDRESSABLES_OBJECTS_ASSET_PATH);
//         }
//
//         /// <summary>
//         /// Search each asset into a particular folder path then iterate in all folders to retrieve files.
//         /// Each folder that is detected will generate a label into the AddressableAssetSettings and will be concatenated for each file under those folders.
//         /// This will add each asset in a group defined by the folders right above the assets for the current step.
//         /// This will add each folder as a label in the AddressableAssetSettings in entry.
//         /// </summary>
//         /// <param name="addressableAssetsSettings">AddressableAssetSettings to setup</param>
//         /// <param name="folderPath">folder path to scan</param>
//         /// <param name="labels">previous labels to use for the current process</param>
//         public static void FindAssetsAndCreateGroupRecursievly(AddressableAssetSettings addressableAssetsSettings, string folderPath, List<string> labels = null)
//         {
//             string[] subFoldersPath = AssetDatabase.GetSubFolders(folderPath);
//             List<string> instantLabels;
//
//             foreach (string subFolderPath in subFoldersPath)
//             {
//                 if(labels == null)
//                     labels = new List<string>();
//
//                 instantLabels = new List<string>(labels);
//
//                 string[] pathPieces = subFolderPath.Split('/');
//                 string currentFolderName = pathPieces[pathPieces.Length-1];
//                 if (!labels.Contains(currentFolderName))
//                 {
//                     addressableAssetsSettings.AddLabel(currentFolderName);
//                     labels.Add(currentFolderName);
//                 }
//
//                 string[] assetsInCurrentFolder = AssetDatabase.FindAssets("*", new string[] {subFolderPath });
//
//                 AddressableAssetGroupSchemaSet groupSet = null;
//                 AddressableAssetGroup newGroup = null;
//
//                 int folderCount = 0;
//
//                 foreach (var assetGuid in assetsInCurrentFolder)
//                 {
//                     string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
//                     string groupName = GetLastFolderFromPath(assetPath);
//                     int currentPathDepth = assetPath.Split('/').Length - 1;
//
//                     if (!AssetDatabase.IsValidFolder(assetPath) && pathPieces.Length == currentPathDepth)
//                     {
//                         if(groupSet == null)
//                             groupSet = GenerateGroupSchemeSet(BundledAssetGroupSchema.BundlePackingMode.PackSeparately);
//
//                         if (newGroup == null)
//                             newGroup = addressableAssetsSettings.CreateGroup(groupName, false, false, false, groupSet.Schemas);
//
//                         string[] sAssetsCutPath = assetPath.Split('/');
//                         sAssetsCutPath = $"{sAssetsCutPath[sAssetsCutPath.Length - 2]}/{sAssetsCutPath[sAssetsCutPath.Length - 1]}".Split('.');
//
//                         AddressableAssetEntry assetEntry = addressableAssetsSettings.CreateOrMoveEntry(assetGuid, newGroup);
//                         assetEntry.SetAddress(sAssetsCutPath[0]);
//                         foreach( var label in labels )
//                             assetEntry.SetLabel(label, true);
//                     }
//                     else
//                     {
//                         folderCount++;
//                     }
//                 }
//
//                 if(folderCount > 0)
//                     FindAssetsAndCreateGroupRecursievly(addressableAssetsSettings, subFolderPath, labels);
//
//                 labels = instantLabels;
//             }
//         }
//
//         /// <summary>
//         /// Method to get the folder name just above a file.
//         /// CAUTION : This will not return the right thing if you specified a folder.
//         /// </summary>
//         /// <param name="filePath"></param>
//         /// <returns></returns>
//         public static string GetLastFolderFromPath(string filePath)
//         {
//             string[] pathSplit = filePath.Split('/');
//             return pathSplit[pathSplit.Length - 2];
//         }
//
//         /// <summary>
//         /// This will generate a generic groupSchema for the objects.
//         /// </summary>
//         /// <param name="packingMode">Let you choose between the 3 possible packing mode from Addressables bundle.</param>
//         /// <returns></returns>
//         public static AddressableAssetGroupSchemaSet GenerateGroupSchemeSet(BundledAssetGroupSchema.BundlePackingMode packingMode)
//         {
//             AddressableAssetGroupSchemaSet groupSet = new AddressableAssetGroupSchemaSet();
//
//             //We generate a BundleAssetGroupSchema to specify each group parameters
//             BundledAssetGroupSchema bundleGroupSchema = ScriptableObject.CreateInstance<BundledAssetGroupSchema>();
//             bundleGroupSchema.Compression = BundledAssetGroupSchema.BundleCompressionMode.LZ4;
//             bundleGroupSchema.IncludeInBuild = false;
//             //missing bundleAssetprovider
//             bundleGroupSchema.ForceUniqueProvider = false;
//             bundleGroupSchema.UseAssetBundleCache = true;
//             bundleGroupSchema.UseAssetBundleCrc = true;
//             bundleGroupSchema.Timeout = 0;
//             bundleGroupSchema.ChunkedTransfer = false;
//             bundleGroupSchema.RedirectLimit = -1;
//             bundleGroupSchema.RetryCount = 0;
//             //TODO Find a way to setup build path
//             //TODO Find a way to setup load path
//             bundleGroupSchema.BundleMode = packingMode;
//
//             ContentUpdateGroupSchema contentGroupschema = ScriptableObject.CreateInstance<ContentUpdateGroupSchema>();
//             contentGroupschema.StaticContent = false;
//
//             groupSet.AddSchema(bundleGroupSchema, null);
//             groupSet.AddSchema(contentGroupschema, null);
//
//             return groupSet;
//         }
//
//         #endregion
//     }
//
// }

