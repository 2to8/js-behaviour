#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;

namespace MoreTags {

public static class TagPreset {

    static List<string> s_PresetString = new List<string>();
    static Dictionary<string, Color> s_ColorTable = new Dictionary<string, Color>();
    static ReorderableList m_ReorderableList;
    public static readonly string kPrefsPrefix = "MoreTags.";

    static TagPreset()
    {
        LoadPreset();
        m_ReorderableList = new ReorderableList(s_PresetString, typeof(string));
        m_ReorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 2;
        m_ReorderableList.drawHeaderCallback += DrawHeaderCallback;
        m_ReorderableList.drawElementCallback += DrawElementCallback;
        m_ReorderableList.onChangedCallback += (list) => SavePreset();
        m_ReorderableList.onAddCallback += (list) => {
            var tag = GetUniquePresetString("Tag");
            s_PresetString.Add(tag);
            s_ColorTable[tag] = Color.white;
        };
        m_ReorderableList.onRemoveCallback += (list) => {
            s_ColorTable.Remove(s_PresetString[list.index]);
            s_PresetString.RemoveAt(list.index);
        };
    }

    public static void OnGUI()
    {
        m_ReorderableList.DoLayoutList();
    }

    public static string[] GetPresets() => s_PresetString.ToArray();

    public static Color GetPresetColor(string preset) =>
        s_ColorTable.ContainsKey(preset) ? s_ColorTable[preset] : Color.white;

    static void DrawHeaderCallback(Rect rect)
    {
        GUI.Label(rect, "Preset", EditorStyles.boldLabel);
        var r = new Rect(rect);
        r.x = rect.xMax - 252;
        r.width = 80;
        if (GUI.Button(r, "To Manager", EditorStyles.miniButton)) {
            foreach (var tag in s_PresetString.Except(TagSystem.Tags())) {
                TagSystem.AddTag(tag);
                TagSystem.SetTagColor(tag, GetPresetColor(tag));
            }
        }
        r.x = r.xMax + 4;
        r.width = 80;
        if (GUI.Button(r, "To Preset", EditorStyles.miniButton)) {
            foreach (var tag in TagSystem.Tags().Except(s_PresetString)) {
                s_PresetString.Add(tag);
                s_ColorTable[tag] = TagSystem.GetTagColor(tag);
            }
            SavePreset();
        }
        r.x = r.xMax + 4;
        r.width = 80;
        if (GUI.Button(r, "Auto Class", EditorStyles.miniButton)) {
            AutoClass();
        }
    }

    static void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        GUI.changed = false;
        var r = new Rect(rect);
        r.y += 1;
        r.height = EditorGUIUtility.singleLineHeight;
        r.xMax -= 50;
        s_PresetString[index] = GUI.TextField(r, s_PresetString[index]);
        r.xMin = r.xMax + 4;
        r.xMax = rect.xMax;
        var col = EditorGUI.ColorField(r, GetPresetColor(s_PresetString[index]));
        if (GUI.changed) {
            s_ColorTable[s_PresetString[index]] = col;
            SavePreset();

            //Repaint();
        }
    }

    static void LoadPreset()
    {
        s_PresetString.Clear();
        s_ColorTable.Clear();
        var count = EditorPrefs.GetInt(kPrefsPrefix + "Count", 0);
        for (var i = 0; i < count; i++) {
            var key = kPrefsPrefix + i;
            var def = GetUniquePresetString("Tag");
            var s = EditorPrefs.GetString(key, def);
            var r = EditorPrefs.GetFloat(key + ".r", 1);
            var g = EditorPrefs.GetFloat(key + ".g", 1);
            var b = EditorPrefs.GetFloat(key + ".b", 1);
            var a = EditorPrefs.GetFloat(key + ".a", 1);
            s_PresetString.Add(s);
            s_ColorTable[s] = new Color(r, g, b, a);
        }
    }

    static void SavePreset()
    {
        var count = EditorPrefs.GetInt(kPrefsPrefix + "Count", 0);
        for (var i = s_PresetString.Count; i < count; i++) {
            var key = kPrefsPrefix + i;
            EditorPrefs.DeleteKey(key);
            EditorPrefs.DeleteKey(key + ".r");
            EditorPrefs.DeleteKey(key + ".g");
            EditorPrefs.DeleteKey(key + ".b");
            EditorPrefs.DeleteKey(key + ".a");
        }
        EditorPrefs.SetInt(kPrefsPrefix + "Count", s_PresetString.Count);
        for (var i = 0; i < s_PresetString.Count; i++) {
            var key = kPrefsPrefix + i;
            EditorPrefs.SetString(key, s_PresetString[i]);
            var col = GetPresetColor(s_PresetString[i]);
            EditorPrefs.SetFloat(key + ".r", col.r);
            EditorPrefs.SetFloat(key + ".g", col.g);
            EditorPrefs.SetFloat(key + ".b", col.b);
            EditorPrefs.SetFloat(key + ".a", col.a);
        }
    }

    public static int GetTagOrder(string tag)
    {
        var order = Array.IndexOf(GetPresets(), tag);
        if (order == -1) {
            order = GetPresets().Length;
        }
        return order;
    }

    public static string GetUniquePresetString(string tag) =>
        ObjectNames.GetUniqueName(s_PresetString.ToArray(), tag);

    static string GetFieldName(string name)
    {
        if (string.IsNullOrEmpty(name)) {
            return string.Empty;
        }
        name = name.Trim();
        var sb = new StringBuilder();
        if (!char.IsLetter(name.FirstOrDefault())) {
            sb.Append("_");
        }
        foreach (var ch in name) {
            if (char.IsLetterOrDigit(ch)) {
                sb.Append(ch);
            }
        }
        name = @"@" + sb.ToString();
        return name;
    }

    static void AutoClassGroup(string cat, ref StringBuilder code)
    {
        var tc = new TagGroup(cat);
        var catname = cat.Replace(".", string.Empty);
        code.AppendFormat("    public class {0}Group : TagGroup", catname).AppendLine();
        code.AppendLine("    {");
        code.AppendFormat("        public {0}Group(string name) : base(name) {{ }}", catname)
            .AppendLine();
        foreach (var tag in tc.children.OrderBy(tag => GetTagOrder(tag))) {
            var tagname = tag.Substring(cat.Length + 1).Replace(".", string.Empty);
            catname = tag.Replace(".", string.Empty);
            if (new TagGroup(tag).all.Any()) {
                code.AppendFormat("        public {2}Group {1} = new {2}Group(\"{0}\");", tag,
                        tagname, catname)
                    .AppendLine();
            } else {
                code.AppendFormat("        public TagName {1} = new TagName(\"{0}\");", tag,
                        tagname)
                    .AppendLine();
            }
        }
        code.AppendLine("    }").AppendLine();
        foreach (var tag in tc.children.Where(tag => new TagGroup(tag).all.Any())
            .OrderBy(tag => GetTagOrder(tag))) {
            AutoClassGroup(tag, ref code);
        }
    }

    static void AutoClass()
    {
        var go = TagHelper.CreateGameObject();
        var ms = MonoScript.FromMonoBehaviour(go.AddComponent<Tags>());
        var path = AssetDatabase.GetAssetPath(ms);
        path = path.Replace("Tags.cs", "TagList.cs");
        TagHelper.DestroyGameObject(go);
        var tags = TagSystem.Tags().OrderBy(tag => GetTagOrder(tag));
        var code = new StringBuilder();
        code.AppendLine("namespace MoreTags");
        code.AppendLine("{");
        code.AppendLine("    public static class Tag");
        code.AppendLine("    {");
        code.AppendLine("        public static AllTags all = new AllTags();");
        var allname = tags.Where(tag => tag.Split('.').Count() < 2);
        var allgroup = tags.Except(allname)
            .Select(tag => tag.Split('.').FirstOrDefault() ?? string.Empty)
            .Distinct();
        allname = allname.Except(allgroup);
        foreach (var tag in allgroup) {
            code.AppendFormat("        public static {0}Group {0} = new {0}Group(\"{0}\");", tag)
                .AppendLine();
        }
        foreach (var tag in allname) {
            code.AppendFormat("        public static TagName {0} = new TagName(\"{0}\");", tag)
                .AppendLine();
        }
        code.AppendLine("    }").AppendLine();
        foreach (var tag in allgroup) {
            AutoClassGroup(tag, ref code);
        }
        code.AppendLine("    public class AllTags : TagNames");
        code.AppendLine("    {");
        code.AppendLine("        public AllTags() : base(TagSystem.AllTags()) { }");
        var allchildren = tags.Except(allname).SelectMany(tag => tag.Split('.').Skip(1)).Distinct();
        foreach (var tag in allchildren) {
            code.AppendFormat("        public TagChildren {0} = new TagChildren(\"{0}\");", tag)
                .AppendLine();
        }
        code.AppendLine("    }");
        code.AppendLine("}");
        File.WriteAllText(path, code.ToString());
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

}

}
#endif