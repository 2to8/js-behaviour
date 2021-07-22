using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Puerts
{
    public static class Gen
    {
        [MenuItem("Puerts/Generate Extensions", false, -102)]
        static void GenerateExtensions()
        {
            var mb = Puerts.Editor.Generator.extensionMethods;
            var output = "";
            var tt = new Dictionary<string, HashSet<string>>();
            mb.ForEach(tk => {
                if (!tk.Key.IsInterface && !tk.Key.IsAbstract && !tk.Key.IsGenericType) {
                    tt[tk.Key.GetFriendlyName()] = new HashSet<string>();
                    tk.Value.ForEach(v => {
                        //Debug.Log(tk.Key.GetFriendlyName());
                        if (!v.DeclaringType.GetFriendlyName().StartsWith("System.") && v.DeclaringType.IsPublic &&
                            tk.Key.IsPublic && tk.Key.IsClass) {
                            tt[tk.Key.GetFriendlyName()].Add(v.DeclaringType.GetFriendlyName());
                        }
                    });
                }
            });
            var names = new List<string>();
            tt.ForEach(tk => {
                var key = tk.Key;
                if (key == "float") key = "System.Single";
                if (key == "string") key = "System.String";
                if (key == "bool") key = "System.Boolean";
                if (!key.Contains("[") && !key.Contains("<") && !key.Contains("&") && !key.Contains("Unity.Entities") &&
                    key != "System.Reflection.ParameterInfo" && !key.StartsWith("UnityEditor.") &&
                    key != "UnityEngine.Rendering.CommandBuffer") {
                    names.Add(key.Split('.').FirstOrDefault());
                    tk.Value.Where(s => !s.StartsWith("UnityEditor.")).ForEach(value => {
                        output += $"    $extension({key}, {value})\n";
                        names.Add(value.Split('.').FirstOrDefault());
                    });
                }
            });
            var ns = names.Where(s => !string.IsNullOrEmpty(s)).Distinct();
            var head = $@"
import {{ {string.Join(", ", ns)} }} from 'csharp';
import {{ $extension }} from 'puerts';
export default function() {{

    console.log(""init extensions"");
";
            File.WriteAllText(Application.dataPath + "/../Packages/src/extensions.ts", $"{head}\n{output}\n}}");
        }

        [MenuItem("Puerts/[new] Generate index.d.ts", false, -105)]
        public static void GenerateDTS()
        {
            //var start = DateTime.Now;
            var src = Path.Combine(Configure.GetCodeOutputDirectory(), "Typing/csharp");
            var saveTo = string.Join("/", Application.dataPath, "../Packages/typings");
            //Directory.CreateDirectory(saveTo);
            Directory.CreateDirectory(saveTo+"/csharp");
            Puerts.Editor.Generator.GenerateDTS();
            //File.Copy($"{src}/index.d.ts", $"{saveTo}/index.d.ts", true);
            using (StreamWriter textWriter = new StreamWriter($"{saveTo}/csharp/index.d.ts", false, Encoding.UTF8)) {
                //string fileContext = typingRender(ToTypingGenInfo(tsTypes));
                textWriter.Write(File.ReadAllText($"{saveTo}/extra/index.d.ts") + "\n" +
                    File.ReadAllText($"{src}/index.d.ts"));
                textWriter.Flush();
            }
            File.Delete($"{src}/index.d.ts");

            GenerateExtensions();
            //ExamplesCfg.TestUsingAction();

            //Debug.Log("finished! use " + (DateTime.Now - start).TotalMilliseconds + " ms");
            AssetDatabase.Refresh();
        }
    }
}