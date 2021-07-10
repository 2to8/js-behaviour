using UnityEditor;
using UnityEngine;

// namespace Editor {

public class GrpcEditorHelper {

  [MenuItem("Tools/Grpc/GenerateGrpcFile",false,100)]
  private static void GenerateGrpcFile()
  {
    var batName = Application.dataPath + "/Editor/generate_protos.bat"; //bat路径
    var pStartInfo = new System.Diagnostics.ProcessStartInfo(batName)                   //设置进程
    {
      CreateNoWindow = false,
      UseShellExecute = true,
      RedirectStandardError = false,
      RedirectStandardInput = false,
    };
    System.Diagnostics.Process.Start(pStartInfo); //开始进程
  }

// }

}