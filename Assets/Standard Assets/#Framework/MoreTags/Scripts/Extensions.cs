// using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using UnityEngine;

namespace MoreTags {

public static class Extensions {

    public static Color TagColor(this Color color) => color == Color.black ? Color.white : color;

    // public static void AddTag(this GameObject go, params string[] tags)
    // {
    //     TagSystem.AddGameObjectTag(go, tags);
    // }

//    public static void AddTag(this Node go, params string[] tags)
//    {
//        TagSystem.AddGameObjectTag(go, tags);
//    }

    // public static void RemoveTag(this GameObject go, params string[] tags)
    // {
    //     TagSystem.RemoveGameObjectTag(go, tags);
    // }

    public static void RemoveTag(this Node node, params string[] tags)
    {
        TagSystem.RemoveNodeTag(node, tags);
    }

    public static void ChangeTag(this GameObject go, string old, string tag)
    {
        if (!go.HasTag(old)) {
            return;
        }
        go.RemoveTag(old);
        go.AddTag(tag);
    }

    public static void ChangeTag(this Node node, string old, string tag)
    {
        if (!node.HasTag(old)) {
            return;
        }
        node.RemoveTag(old);
        node.AddTag(tag);
    }

    public static bool HasTag(this GameObject go, string tag) =>
        TagSystem.GameObjectTags(go).Contains(tag);

    public static bool HasTag(this Node node, string tag) =>
        TagSystem.GameObjectTags(node).Contains(tag);

    public static bool AnyTags(this GameObject go, params string[] tags) =>
        TagSystem.GameObjectTags(go).Intersect(tags).Any();

    public static bool AnyTags(this Node node, params string[] tags) =>
        TagSystem.GameObjectTags(node).Intersect(tags).Any();

    public static bool BothTags(this GameObject go, params string[] tags) =>
        TagSystem.GameObjectTags(go).Intersect(tags).Count() == tags.Length;

    public static bool BothTags(this Node node, params string[] tags) =>
        TagSystem.GameObjectTags(node).Intersect(tags).Count() == tags.Length;

    public static IEnumerable<string> GetTags(this GameObject go) => TagSystem.GetTags(go?.GetComponent<Tags>()?.ids);// TagSystem.GameObjectTags(go).ToArray();
     public static string[] GetTags(this Node go) => TagSystem.GameObjectTags(go).ToArray();

    public static IEnumerable<string> FindTags(this GameObject go, TagNames tags) =>
        TagSystem.GameObjectTags(go).Intersect(tags);

    public static string[] FindTags(this Node node, TagNames tags) =>
        TagSystem.GameObjectTags(node).Intersect(tags).ToArray();

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

    public static IEnumerable<Node> GetChildrenList(this Node node, bool self = true)
    {
        if (self) {
            yield return node;
        }
        var stack = new Stack<IEnumerator>();
        stack.Push(node.GetChildNodes().GetEnumerator());
        while (stack.Count > 0) {
            var enumerator = stack.Peek();
            if (enumerator.MoveNext()) {
                var t = enumerator.Current as Node;
                yield return t;
    
                stack.Push(t.GetChildNodes().GetEnumerator());
            } else {
                stack.Pop();
            }
        }
    }

}

}