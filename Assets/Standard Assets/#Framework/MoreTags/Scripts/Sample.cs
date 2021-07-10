using UnityEngine;

namespace MoreTags {

public class Sample : MonoBehaviour {

    // public GameObject Target;
    //
    // private Rect m_AreaRect = new Rect(10, 10, 500, 120);
    // private Dictionary<string, bool> m_TagOn;
    // private Dictionary<string, bool> m_TagOff;
    // private HashSet<GameObject> m_InTarget = new HashSet<GameObject>();
    // private string m_Pattern = "Object.Cube & (Color.Blue | Color.Yellow)";
    // private bool m_UsePattern;
    //
    // void Start()
    // {
    //     m_TagOn = Tag.all.ToDictionary(k => k, v => false);
    //     m_TagOff = Tag.all.ToDictionary(k => k, v => false);
    //     m_TagOn["All"] = false;
    //
    //     var tags = new[] { Tag.Color.Red, Tag.Color.Green, Tag.Color.Blue, Tag.Color.Yellow, Tag.Color.Magenta, Tag.Color.Cyan };
    //     var colors = new[] { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };
    //     var lookup = Enumerable.Range(0, tags.Length).ToDictionary(k => tags[k], v => colors[v]);
    //     foreach (var tag in tags)
    //         foreach (var go in ((TagPattern)tag).GameObjects())
    //         {
    //             var mpb = new MaterialPropertyBlock();
    //             mpb.SetColor("_Color", lookup[tag]);
    //             go.GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
    //         }
    // }
    //
    // void Update()
    // {
    // }
    //
    // void OnGUI()
    // {
    //     bool on = false, off = false;
    //     GUILayout.BeginArea(m_AreaRect, new GUIStyle("box"));
    //     GUILayout.BeginHorizontal();
    //     GUI.changed = false;
    //     m_TagOn["All"] = GUILayout.Toggle(m_TagOn["All"], "All", "button", GUILayout.ExpandWidth(false));
    //     on |= GUI.changed;
    //     foreach (TagName tag in Tag.Object.all)
    //     {
    //         GUI.changed = false;
    //         m_TagOn[tag] = GUILayout.Toggle(m_TagOn[tag], tag.last, "button", GUILayout.ExpandWidth(false));
    //         on |= GUI.changed;
    //         GUI.changed = false;
    //         m_TagOff[tag] = GUILayout.Toggle(m_TagOff[tag], "X", "button", GUILayout.ExpandWidth(false));
    //         off |= GUI.changed;
    //     }
    //     GUILayout.EndHorizontal();
    //     GUILayout.BeginHorizontal();
    //     foreach (TagName tag in Tag.Color.all)
    //     {
    //         GUI.changed = false;
    //         m_TagOn[tag] = GUILayout.Toggle(m_TagOn[tag], tag.last, "button", GUILayout.ExpandWidth(false));
    //         on |= GUI.changed;
    //         GUI.changed = false;
    //         m_TagOff[tag] = GUILayout.Toggle(m_TagOff[tag], "X", "button", GUILayout.ExpandWidth(false));
    //         off |= GUI.changed;
    //     }
    //     GUILayout.EndHorizontal();
    //     m_Pattern = GUILayout.TextField(m_Pattern);
    //     var usepattern = GUILayout.Toggle(m_UsePattern, "Use Pattern", "button", GUILayout.ExpandWidth(false));
    //
    //     if (on)
    //         foreach (var kv in m_TagOn)
    //             if (kv.Value) m_TagOff[kv.Key] = false;
    //     if (off)
    //         foreach (var kv in m_TagOff)
    //             if (kv.Value) m_TagOn[kv.Key] = false;
    //     if (usepattern != m_UsePattern || on || off)
    //     {
    //         var pat = TagSystem.pattern;
    //         m_UsePattern = usepattern != m_UsePattern ? usepattern : m_UsePattern;
    //         if (usepattern)
    //             pat = m_Pattern;
    //         else
    //         {
    //             pat = m_TagOn["All"] ? Tag.all.either : pat;
    //             pat.Or();
    //             foreach (var kv in m_TagOn)
    //                 if (kv.Value) pat.With(kv.Key);
    //             if (pat.GameObjects().Any())
    //             {
    //                 pat.Exclude();
    //                 foreach (var kv in m_TagOff)
    //                     if (kv.Value) pat.With(kv.Key);
    //             }
    //         }
    //         foreach (var go in pat.GameObjects())
    //         {
    //             var agent = go.GetComponent<NavMeshAgent>();
    //             if (agent == null) continue;
    //             m_InTarget.Add(go);
    //             agent.destination = Target.transform.position;
    //         }
    //         foreach (var go in (Tag.all.either - pat).GameObjects().Intersect(m_InTarget))
    //         {
    //             var agent = go.GetComponent<NavMeshAgent>();
    //             if (agent == null) continue;
    //             m_InTarget.Remove(go);
    //             Vector3 pos;
    //             do
    //                 pos = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
    //             while (Vector3.Distance(pos, Target.transform.position) < 1.5f);
    //             agent.destination = pos;
    //         }
    //     }
    //     GUILayout.EndArea();
    // }

}

}