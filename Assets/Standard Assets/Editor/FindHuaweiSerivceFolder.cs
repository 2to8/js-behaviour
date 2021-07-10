using GameEngine.Extensions;
using GameEngine.Kernel;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

//namespace Editor {

public class FindHuaweiSerivceFolder {

  [MenuItem("Tests/FindHuawei")]
  static void findHuawei()
  {
    Debug.Log(FindRootPath("HuaweiService"));
    // DirectoryInfo info = new DirectoryInfo(Application.dataPath);
    // var dirs = info.EnumerateDirectories();
    // Debug.Log(dirs.FirstOrDefault(t => t.Name == "HuaweiService")?.FullName);
  }

  static string FindRootPath(string dirname)
  {
    // DirectoryInfo info = new DirectoryInfo(Application.dataPath);
    // var dirs = info.EnumerateDirectories();
    var dirs =
      Directory.EnumerateDirectories(Application.dataPath, "*.*", SearchOption.AllDirectories);
    return dirs.FirstOrDefault(t =>Path.GetFileName(t)  == dirname)?.dataPathRoot();
  }

//}

}