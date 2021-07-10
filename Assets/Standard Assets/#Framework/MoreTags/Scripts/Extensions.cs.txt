using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreTags
{
    public static class Extensions
    {
        public static void AddTag(this GameObject go, params string[] tags)
        {
            TagSystem.AddGameObjectTag(go, tags);
        }

        public static void RemoveTag(this GameObject go, params string[] tags)
        {
            TagSystem.RemoveGameObjectTag(go, tags);
        }

        public static void ChangeTag(this GameObject go, string old, string tag)
        {
            if (!go.HasTag(old)) return;
            go.RemoveTag(old);
            go.AddTag(tag);
        }

        public static bool HasTag(this GameObject go, string tag)
        {
            return TagSystem.GameObjectTags(go).Contains(tag);
        }

        public static bool AnyTags(this GameObject go, params string[] tags)
        {
            return TagSystem.GameObjectTags(go).Intersect(tags).Any();
        }

        public static bool BothTags(this GameObject go, params string[] tags)
        {
            return TagSystem.GameObjectTags(go).Intersect(tags).Count() == tags.Length;
        }

        public static string[] GetTags(this GameObject go)
        {
            return TagSystem.GameObjectTags(go).ToArray();
        }

        public static string[] FindTags(this GameObject go, TagNames tags)
        {
            return TagSystem.GameObjectTags(go).Intersect(tags).ToArray();
        }

        public static IEnumerable<GameObject> GetChildrenList(this GameObject go, bool self = true)
        {
            if (self) yield return go;
            var stack = new Stack<IEnumerator>();
            stack.Push(go.transform.GetEnumerator());
            while (stack.Count > 0)
            {
                var enumerator = stack.Peek();
                if (enumerator.MoveNext())
                {
                    var t = enumerator.Current as Transform;
                    yield return t.gameObject;
                    stack.Push(t.GetEnumerator());
                }
                else
                {
                    stack.Pop();
                }
            }
        }
    }
}