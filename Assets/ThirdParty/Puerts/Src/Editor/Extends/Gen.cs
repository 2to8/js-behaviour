using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Puerts
{
    public static class Gen
    {
        [MenuItem("Puerts/[new] Generate index.d.ts", false, -105)]
        public static void GenerateDTS()
        {
            //var start = DateTime.Now;
            var src = Path.Combine(Configure.GetCodeOutputDirectory(), "Typing/csharp");
            var saveTo = string.Join("/", Application.dataPath, "../Packages/typings");
            //Directory.CreateDirectory(saveTo);
            Directory.CreateDirectory(saveTo);
            Puerts.Editor.Generator.GenerateDTS();
            //File.Copy($"{src}/index.d.ts", $"{saveTo}/index.d.ts", true);
            using (StreamWriter textWriter = new StreamWriter($"{saveTo}/csharp/index.d.ts", false, Encoding.UTF8)) {
                //string fileContext = typingRender(ToTypingGenInfo(tsTypes));
                textWriter.Write(File.ReadAllText($"{saveTo}/extra/index.d.ts") + "\n" +
                    File.ReadAllText($"{src}/index.d.ts"));
                textWriter.Flush();
            }

            //Debug.Log("finished! use " + (DateTime.Now - start).TotalMilliseconds + " ms");
            AssetDatabase.Refresh();
        }
    }
}