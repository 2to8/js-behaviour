// using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreTags {

public static class Extensions {

    public static Color TagColor(this Color color) => color == Color.black ? Color.white : color;

    // public static void AddTag(this GameObject go, params string[] tags)
    // {
    //     TagSystem.AddGameObjectTag(go, tags);
    // }

    // public static void AddTag(this Node go, params string[] tags)
    // {
    //     TagSystem.AddGameObjectTag(go, tags);
    // }

    // public static void RemoveTag(this GameObject go, params string[] tags)
    // {
    //     TagSystem.RemoveGameObjectTag(go, tags);
    // }

    // public static void RemoveTag(this Node go, params string[] tags)
    // {
    //     TagSystem.RemoveGameObjectTag(go, tags);
    // }

    public static void ChangeTag(this GameObject go, string old, string tag)
    {
        if (!go.HasTag(old)) {
            return;
        }
        go.RemoveTag(old);
        go.AddTag(tag);
    }

    // public static void ChangeTag(this Node go, string old, string tag)
    // {
    //     if (!go.HasTag(old)) {
    //         return;
    //     }
    //     go.RemoveTag(old);
    //     go.AddTag(tag);
    // }

    public static bool HasTag(this GameObject go, string tag) =>
        TagSystem.GameObjectTags(go).Contains(tag);

    // public static bool HasTag(this Node go, string tag) =>
    //     TagSystem.GameObjectTags(go).Contains(tag);

    public static bool AnyTags(this GameObject go, params string[] tags) =>
        TagSystem.GameObjectTags(go).Intersect(tags).Any();

    // public static bool AnyTags(this Node go, params string[] tags) =>
    //     TagSystem.GameObjectTags(go).Intersect(tags).Any();

    public static bool BothTags(this GameObject go, params string[] tags) =>
        TagSystem.GameObjectTags(go).Intersect(tags).Count() == tags.Length;

    // public static bool BothTags(this Node go, params string[] tags) =>
    //     TagSystem.GameObjectTags(go).Intersect(tags).Count() == tags.Length;

    public static IEnumerable<string> GetTags(this GameObject go) => TagSystem.GetTags(go?.GetComponent<Tags>()?.ids);// TagSystem.GameObjectTags(go).ToArray();
    // public static string[] GetTags(this Node go) => TagSystem.GameObjectTags(go).ToArray();

    public static IEnumerable<string> FindTags(this GameObject go, TagNames tags) =>
        TagSystem.GameObjectTags(go).Intersect(tags);

    // public static string[] FindTags(this Node go, TagNames tags) =>
    //     TagSystem.GameObjectTags(go).Intersect(tags).ToArray();

    public static IEnumerable<GameObject> GetChildrenList(this GameObject go, bool self = true)
    {
        if (self) {
            yield return go;
        }
        var stack = new Stack<IEnumerator>();
        stack.Push(go.transform.GetEnumerator());
        while (stack.Count > 0) {
            var enumerator = stack.Peek();
            if (enumerator.MoveNext()) {
                var t = enumerator.Current as Transform;
                yield return t.gameObject;

                stack.Push(t.GetEnumerator());
            } else {
                stack.Pop();
            }
        }
    }

    // public static IEnumerable<Node> GetChildrenList(this Node go, bool self = true)
    // {
    //     if (self) {
    //         yield return go;
    //     }
    //     var stack = new Stack<IEnumerator>();
    //     stack.Push(go.GetChildNodes().GetEnumerator());
    //     while (stack.Count > 0) {
    //         var enumerator = stack.Peek();
    //         if (enumerator.MoveNext()) {
    //             var t = enumerator.Current as Node;
    //             yield return t;
    //
    //             stack.Push(t.GetChildNodes().GetEnumerator());
    //         } else {
    //             stack.Pop();
    //         }
    //     }
    // }

}

}