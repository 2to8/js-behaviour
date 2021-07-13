using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Utils.Scenes;
using Object = UnityEngine.Object;

namespace GameEngine.Extensions
{
    public static partial class Transforms
    {
        public static void SetActive(this Component component, bool enable = true) =>
            component.gameObject.SetActive(enable);

        public static Transform FindOrCreate(this Transform transform, string name) =>
            transform.Find(name) ?? new GameObject(name).Of(go => go.transform.SetParent(transform)).transform;

        public static GameObject FindOrCreate(this GameObject go, string name) =>
            go?.transform.FindOrCreate(name).gameObject;

        public static GameObject FindOrCreateRoot(this GameObject gameObject, string name) =>
            gameObject.scene.GetRootGameObjects().FirstOrDefault(go => go.name == name) ??
            new GameObject(name).Of(go => SceneManager.MoveGameObjectToScene(go, gameObject.scene));

        /**
         * 否则x=6 会被转成x=5
         */
        public static Vector3Int ToVector3Int(this Vector3 vector3) => Vector3Int.RoundToInt(vector3);

        public static float X(this Component component, bool withParent = false, float x = 0f) =>
            component.transform.localPosition.x + (withParent ? component.transform.parent.localPosition.x + x : 0);

        public static float Y(this Component component, bool withParent = false, float y = 0f) =>
            component.transform.localPosition.y + (withParent ? component.transform.parent.localPosition.y + y : 0);

        public static float Z(this Component component, bool withParent = false, float z = 0f) =>
            component.transform.localPosition.z + (withParent ? component.transform.parent.localPosition.z + z : 0);

        public static int X(this Component component, bool withParent = false, int x = 0)
        {
            Transform transform;
            return Vector3Int.RoundToInt(component.transform.localPosition).x +
                (withParent ? Vector3Int.RoundToInt(component.transform.parent.localPosition).x + x : 0);
        }

        public static int Y(this Component component, bool withParent = false, int y = 0) =>
            Vector3Int.RoundToInt(component.transform.localPosition).y +
            (withParent ? Vector3Int.RoundToInt(component.transform.parent.localPosition).y + y : 0);

        public static int Z(this Component component, bool withParent = false, int z = 0) =>
            Vector3Int.RoundToInt(component.transform.localPosition).z +
            (withParent ? Vector3Int.RoundToInt(component.transform.parent.localPosition).z + z : 0);

        public static Vector3 XAdd(this Component component, float x) =>
            component.transform.localPosition += new Vector3(x, 0, 0);

        public static Vector3 YAdd(this Component component, float y) =>
            component.transform.localPosition += new Vector3(0, y, 0);

        public static Vector3 ZAdd(this Component component, float z) =>
            component.transform.localPosition += new Vector3(0, 0, z);

        public static Vector2Int XY(this Component component, bool withParent = false, int x = 0, int y = 0)
        {
            return new Vector2Int(component.X(withParent, 0) + x, component.Y(withParent, 0) + y);
        }

        public static Vector2Int XZ(this Component component, bool withParent = false, int x = 0, int z = 0)
        {
            Vector3 localPosition;
            return new Vector2Int(component.X(withParent, 0) + x, component.Z(withParent, 0) + z);
        }

        // public static Vector2Int setIntXY(this Component component,int x = 0, int y = 0, bool withParent = false)
        // {
        //     return new Vector2Int(component.getIntX(withParent)+x, component.getIntY(withParent)+y);
        // }
        //
        // public static Vector2Int setIntXZ(this Component component, bool withParent = false)
        // {
        //     Vector3 localPosition;
        //
        //     return new Vector2Int(component.getIntX(withParent), component.getIntZ(withParent));
        // }

        public static Vector3Int getvVector3Int(this Component component, bool withParent = false, int x = 0, int y = 0,
            int z = 0)
        {
            Vector3 localPosition;
            return new Vector3Int(component.X(withParent, 0) + x, component.Y(withParent, 0) + y,
                component.Z(withParent, 0) + z);
        }

        public static Vector3 addVector3(this Component component, float x, float y, float z) =>
            component.transform.localPosition += new Vector3(x, y, z);

        public static Vector3 addVector3(this Component component, int x, int y, int z) =>
            component.transform.localPosition += new Vector3(x, y, z);

        public static Transform AlignTo(this Transform src, Transform dest, bool moveToLast = false)
        {
            new RectTransformData(dest).PushToTransform(src);
            if (moveToLast) {
                src.SetAsLastSibling();
            }

            return src;
        }

        public static RectTransform AlignTo(this RectTransform src, RectTransform dest, bool moveToLast = false)
        {
            new RectTransformData(dest).PushToTransform(src);
            if (moveToLast) {
                src.SetAsLastSibling();
            }

            return src;
        }

        public static RectTransform AlignTo(this RectTransform src, Transform dest, bool moveToLast = false) =>
            AlignTo(src, dest.GetComponent<RectTransform>(), moveToLast);

        public static RectTransform AlignTo(this Transform src, RectTransform dest, bool moveToLast = false) =>
            AlignTo(src.GetComponent<RectTransform>(), dest.GetComponent<RectTransform>(), moveToLast);

        public static void ClearChildTransforms(this Transform transform, Expression<Func<Transform, bool>> expr = null,
            bool directOnly = true)
        {
            foreach (var child in transform.Childs(expr, directOnly) /*.Where(expr?.Compile() ?? (t => true))*/) {
                if (child.gameObject != null) {
                    child.gameObject.DestroySelf();
                }

                // if (Application.isPlaying) {
                //     Object.Destroy(child.gameObject);
                // } else {
                //     Object.DestroyImmediate(child.gameObject);
                // }
            }
        }

        public static List<GameObject> ChildGameObjects(this Transform transform,
            Expression<Func<Transform, bool>> expr = null, bool directOnly = true)
        {
            return transform.Childs(expr, directOnly).Select(t => t.gameObject).ToList();
        }

        public static List<GameObject> ChildGameObjects(this Component component,
            Expression<Func<Transform, bool>> expr = null, bool directOnly = true) =>
            ChildGameObjects(component?.transform, expr, directOnly);

        public static List<GameObject> ChildGameObjects(this GameObject gameObject,
            Expression<Func<Transform, bool>> expr = null, bool directOnly = true) =>
            ChildGameObjects(gameObject?.transform, expr, directOnly);

        public static List<Transform> Childs(this GameObject gameObject, Expression<Func<Transform, bool>> expr = null,
            bool directOnly = true) =>
            Childs(gameObject?.transform, expr, directOnly);

        public static List<Transform> Childs(this Component gameObject, Expression<Func<Transform, bool>> expr = null,
            bool directOnly = true) =>
            Childs(gameObject?.transform, expr, directOnly);

        public static List<Transform> Childs(this Transform transform, Expression<Func<Transform, bool>> expr = null,
            bool directOnly = true)
        {
            var children = new List<Transform>();
            foreach (Transform child in transform) {
                if (!directOnly || child.parent == transform) {
                    children.Add(child);
                }
            }

            return children.Where(expr?.Compile() ?? (t => true)).OrderBy(t => t.GetSiblingIndex()).ToList();
        }

        // public static List<Transform> ToList(this Transform transform) => transform.Childs();
        //
        // public static void ForEach(this Transform transform, Action<Transform> action)
        // {
        //     transform.Childs().ForEach(action);
        // }

        public static string dataPathRoot(this string path)
        {
            path = path.Replace("\\", "/");
            if (!path.Contains(Application.dataPath)) {
                return Path.Combine(Path.GetDirectoryName(Application.dataPath) + "", path).Replace("\\", "/");
            }

            return path;
        }

        public static string M_PregReplace(this string str, string[] rule, string to) =>
            M_PregReplace(str, rule, new[] {to});

        public static string M_PregReplace(this string str, string rule, string to) =>
            M_PregReplace(str, new[] {rule}, new[] {to});

        public static string M_PregReplace(this string str, string[] rule, string[] to)
        {
            rule.ForEach((s, i) => {
                var replace = to.Length == rule.Length ? to[i] : to[0];
                str = Regex.Replace(str, s, replace);
            });
            return str;
        }

        public static string dataPathRoot(this Object obj)
        {
#if UNITY_EDITOR
            return obj == null ? string.Empty : AssetDatabase.GetAssetPath(obj)?.dataPathRoot();
#endif
            return null;
        }

        public static string Dirname(this string path, bool create = false)
        {
            path = Path.GetDirectoryName(path);
            if (create && !Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string FileExtension(this string path) => Path.GetExtension(path);
        public static string ResourcePath(this Object obj) => obj?.AssetPath().ResourcePath();

        public static string ResourcePath(this string path)
        {
            var sp = path.Replace('\\', '/').Split(new[] {"Resources/"}, StringSplitOptions.None);
            var name = sp.Length > 1 ? sp[1] : sp[0];
            if (Path.HasExtension(name)) {
                name = name.Replace(Path.GetExtension(name), "");
            }

            return name;
        }

        public static string AssetPath(this Object obj)
        {
#if UNITY_EDITOR
            return obj == null ? string.Empty : AssetDatabase.GetAssetPath(obj);
#endif
            return null;
        }

        public static string CreateDirFromFilePath(this string path, bool isFile = true)
        {
            var dir = isFile ? Path.GetDirectoryName(path) : path;
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            if (!Directory.Exists(dir)) {
                throw new Exception("dir created fail");
            }

            return path; //.dataPathRoot();
        }

        public static string GetPath(this GameObject gameObject) =>
            gameObject == null
                ? ""
                : string.Join("/",
                    gameObject.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());

        public static string GetPath(this Component component) => component?.gameObject.GetPath();
        public static string GetClassPath(this Component component) => component?.gameObject.GetClassPath();

        public static string M_VarName(this string theVar, bool toCaptical = true)
        {
            var str = theVar.Replace("(clone)", "").Trim().Split('.').First();
            str = Regex.Replace(str, @"[ _\-\.]+", " ");
            str = Regex.Replace(str, @"[\W\d]+", "");
            if (toCaptical) {
                str = Regex.Replace(str, @"\b[a-z]", m => m.Value.ToUpper());
            }

            return str.Replace(" ", toCaptical ? "" : "_");
        }

        public static string GetClassPath(this GameObject gameObject)
        {
            return gameObject == null
                ? ""
                : string.Join("/",
                    gameObject.GetComponentsInParent<Transform>().Select(t => t.name.M_VarName()).Reverse().ToArray());
        }

        //public static string GetPath(this Component component) => component?.gameObject.GetPath();
    }
}