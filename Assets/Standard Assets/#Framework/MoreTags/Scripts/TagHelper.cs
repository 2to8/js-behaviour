using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreTags {

public static class TagHelper {

    public static TagPattern StringToPattern(string str)
    {
        var index = 0;
        var pattern = TagSystem.pattern;
        str = Regex.Replace(str, @"\s+", string.Empty);
        while (true) {
            TagPattern r;
            var s = str.Substring(index);
            var neg = s.StartsWith("-");
            if (neg) {
                index++;
                s = str.Substring(index);
            }
            if (s.StartsWith("*") && !s.StartsWith("*.")) {
                index++;
                r = new TagNames(TagSystem.AllTags()).either;
            } else if (s.StartsWith("(")) {
                int idx = 0, count = 0;
                while (true) {
                    idx = s.IndexOfAny("()".ToArray(), idx + 1);
                    if (idx == -1) {
                        break;
                    }
                    if (s[idx] == ')' && count == 0) {
                        break;
                    }
                    count += s[idx] == '(' ? 1 : -1;
                }
                if (idx == -1) {
                    break;
                }
                index += idx + 1;
                s = s.Substring(1, idx - 1);
                r = StringToPattern(s);
            } else if (s.ToLower().StartsWith("both(") || s.ToLower().StartsWith("either(")) {
                var baseidx = s.IndexOf('(') + 1;
                var idx = s.IndexOf(')', baseidx);
                var tn = StringToTagNames(s.Substring(baseidx, idx - baseidx));
                index += idx + 1;
                r = s.ToLower().StartsWith("both(") ? tn.both : tn.either;
            } else {
                var len = s.IndexOfAny("&|-".ToArray());
                len = len == -1 ? s.Length : len;
                s = s.Substring(0, len);
                index += len;
                if (!s.StartsWith("*.") && !s.EndsWith(".*") && !s.EndsWith(".?")) {
                    r = (TagName)s;
                } else {
                    r = ConvertToTagNames(s).either;
                }
            }
            pattern = pattern.Combine(neg ? -r : r);
            if (index >= str.Length) {
                break;
            }
            if (!"&|-".Contains(str[index])) {
                break;
            }
            switch (str[index]) {
                case '&' :
                    pattern = pattern.And();
                    break;
                case '|' :
                    pattern = pattern.Or();
                    break;
                case '-' :
                    pattern = pattern.Exclude();
                    break;
            }
            index++;
        }
        return pattern;
    }

    public static TagNames StringToTagNames(string str)
    {
        var tn = new TagNames();
        var op = '+';
        int baseidx = 0, idx = 0;
        while (idx != -1) {
            idx = str.IndexOfAny("+-".ToArray(), baseidx);
            var name = idx == -1 ? str.Substring(baseidx) : str.Substring(baseidx, idx - baseidx);
            if (op == '+') {
                tn.Add(ConvertToTagNames(name));
            } else {
                tn.Remove(ConvertToTagNames(name));
            }
            if (idx != -1) {
                op = str[idx];
                idx++;
                baseidx = idx;
            }
        }
        return tn;
    }

    static TagNames ConvertToTagNames(string str)
    {
        if (string.IsNullOrEmpty(str)) {
            return new TagNames();
        }
        if (!str.StartsWith("*.")) {
            if (str.EndsWith(".*")) {
                return ((TagGroup)str.TrimEnd('.', '*')).all;
            } else if (str.EndsWith(".?")) {
                return ((TagGroup)str.TrimEnd('.', '?')).children;
            } else {
                return (TagName)str;
            }
        } else {
            str = str.Substring(2);
            if (str.EndsWith(".*")) {
                return ((TagChildren)str.TrimEnd('.', '*')).all;
            } else if (str.EndsWith(".?")) {
                return ((TagChildren)str.TrimEnd('.', '?')).children;
            } else {
                return (TagChildren)str;
            }
        }
    }

    public static GameObject CreateGameObject()
    {
        var go = new GameObject();

        //go.hideFlags = HideFlags.HideAndDontSave;
        SceneManager.MoveGameObjectToScene(go, SceneManager.GetActiveScene());
        return go;
    }

    public static GameObject[] CreateGameObject(int count) =>
        Enumerable.Repeat(CreateGameObject(), count).ToArray();

    public static void DestroyGameObject(GameObject go)
    {
        Object.DestroyImmediate(go);
    }

    public static void DestroyGameObject(params GameObject[] list)
    {
        foreach (var go in list) {
            Object.DestroyImmediate(go);
        }
    }

    public static TagPattern Test() =>

        //TagGroup b = "Team";
        //var a = (Tag.Team.all - Tag.Team.Player);
        //var aa = (-a.either).GameObjects();
        //var tt = (Tag.all - Tag.all.Player).either;
        //var s = Tag.Team.Player & ((TagName)"Weapon.Sword" | "Magic.Fire");
        //var t = Tag.Team.Player & (Tag.Weapon.Sword | Tag.all.Fire.either);
        //var r = Tag.Team.Player | (Tag.Weapon.Sword & Tag.all.either);
        //var rr = Tag.Team.Player | (Tag.Weapon.Sword & Tag.Magic.Fire);
        TagSystem.pattern;

}

}