﻿using System.IO;
using UnityEditor;
using UnityEngine;

public static class OpenTsProject
{

    [MenuItem("Assets/Open TS Project")]
    public static void OpenTsProj()
    {
        var tsProjPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../TsProj"));
        CmdExecutor.RunShell("code", tsProjPath);
    }
}