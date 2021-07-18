#region old
// using System;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace MoreTags
// {
//     public class TagGUI
//     {
//         public Action<string> OnAddItem;
//         public Action<string> OnClickItem;
//         public Action<Rect, string> OnRightClickItem;
//         public Func<string, GUIContent> OnItemString;
//         public Func<string, Color> OnItemColor;
//
//         private string m_NewItem = string.Empty;
//         private GUIStyle m_BgStyle = null;
//
//         public void InitStyle()
//         {
//             if (m_BgStyle != null) return;
//             m_BgStyle = new GUIStyle("CN CountBadge");
//             if (!EditorGUIUtility.isProSkin) return;
//
//             var res = Resources.FindObjectsOfTypeAll<Texture2D>();
//             foreach (var tex in res)
//                 if (tex.name.Equals("ConsoleCountBadge") && tex != m_BgStyle.normal.background)
//                     m_BgStyle.normal.background = tex;
//         }
//
//         public void OnGUI(IEnumerable<string> list, string header = null)
//         {
//             InitStyle();
//
//             var guicolor = GUI.color;
//             var tagstyle = new GUIStyle("OL Minus");
//             tagstyle.normal.textColor = Color.white;
//             tagstyle.font = EditorStyles.boldFont;
//             var addstyle = new GUIStyle("OL Plus");
//             var headstyle = new GUIStyle("label");
//             headstyle.font = EditorStyles.boldFont;
//
//             var xMax = EditorGUIUtility.currentViewWidth - 12;
//             var height = EditorGUIUtility.singleLineHeight + 2;
//             var newrect = EditorGUILayout.GetControlRect(GUILayout.Height(height));
//             var bgrect = new Rect(newrect);
//             bgrect.xMin = 16;
//
//             if (!string.IsNullOrEmpty(header))
//             {
//                 var gc = new GUIContent(header);
//                 var w = headstyle.CalcSize(gc).x + 4;
//                 bgrect.width = w;
//                 GUI.color = new Color(0, 0, 0, 0);
//                 GUI.Box(bgrect, GUIContent.none, m_BgStyle);
//
//                 GUI.color = guicolor;
//                 var rect = new Rect(bgrect);
//                 rect.position += new Vector2(1, 1);
//                 GUI.Label(rect, gc, headstyle);
//                 bgrect.xMin = bgrect.xMax;
//             }
//
//             foreach (var item in list)
//             {
//                 var s = item is string ? item as string : item.ToString();
//                 var gc = OnItemString != null ? OnItemString(item) : new GUIContent(s);
//                 var w = tagstyle.CalcSize(gc).x + 4;
//                 if (bgrect.xMin + w > xMax)
//                 {
//                     newrect = EditorGUILayout.GetControlRect(GUILayout.Height(height));
//                     bgrect = new Rect(newrect);
//                     bgrect.xMin = string.IsNullOrEmpty(header) ? 16 : 30;
//                 }
//                 bgrect.width = w;
//                 var col = Color.white;
//                 if (OnItemColor != null) col = OnItemColor(item);
//                 GUI.color = col;
//                 GUI.Box(bgrect, GUIContent.none, m_BgStyle);
//
//                 var lum = col.grayscale > 0.5 ? col.grayscale - 0.5f : col.grayscale + 0.5f;
//                 GUI.color = new Color(lum, lum, lum, 1.0f);
//                 var rect = new Rect(bgrect);
//                 rect.position += new Vector2(1, 1);
//                 if (GUI.Button(rect, gc, tagstyle))
//                 {
//                     if (Event.current.button == 0 && OnClickItem != null)
//                         OnClickItem(item);
//                     if (Event.current.button == 1 && OnRightClickItem != null)
//                         OnRightClickItem(rect, item);
//                 }
//                 bgrect.xMin = bgrect.xMax;
//                 GUI.color = guicolor;
//             }
//
//             if (OnAddItem != null)
//             {
//                 if (bgrect.xMin + 150 > xMax)
//                 {
//                     newrect = EditorGUILayout.GetControlRect(GUILayout.Height(height));
//                     bgrect = new Rect(newrect);
//                     bgrect.xMin = string.IsNullOrEmpty(header) ? 16 : 30;
//                 }
//                 bgrect.width = 150;
//                 GUI.Box(bgrect, GUIContent.none, m_BgStyle);
//                 var gc = GUIContent.none;
//                 var w = tagstyle.CalcSize(gc).x;
//                 var rect = new Rect(bgrect);
//                 rect.position += new Vector2(1, 1);
//                 rect.width = w;
//                 if (GUI.Button(rect, gc, addstyle))
//                 {
//                     OnAddItem(m_NewItem);
//                     m_NewItem = string.Empty;
//                 }
//                 rect = new Rect(bgrect);
//                 rect.yMin += 1;
//                 rect.yMax -= 1;
//                 rect.xMin += w;
//                 rect.xMax -= 8;
//                 m_NewItem = GUI.TextField(rect, m_NewItem);
//                 bgrect.xMin = bgrect.xMax;
//             }
//         }
//     }
// }
#endregion

#if UNITY_EDITOR
using System;
using System.Collections.Generic;

// using GameEngine.Extensions;
using Puerts.Attributes;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MoreTags {

[PuertsIgnore]
public class TagGUI {

    public Action<string> OnAddItem;
    public Action<string> OnClickItem;
    public Action<Rect, string> OnRightClickItem;
    public Func<string, GUIContent> OnItemString;
    public Func<string, Color> OnItemColor;
    static string m_NewItem = string.Empty;
    GUIStyle m_BgStyle = null;
    public float indentLeft = 12;
    public float maxWidth = float.NaN;

    public void InitStyle()
    {
        if (m_BgStyle != null) {
            return;
        }
        m_BgStyle = new GUIStyle("flow node 1"); // new GUIStyle("CN CountBadge");
        if (!EditorGUIUtility.isProSkin) {
            return;
        }
        var res = Resources.FindObjectsOfTypeAll<Texture2D>();
        foreach (var tex in res) {
            if (tex.name.Equals("ConsoleCountBadge") && tex != m_BgStyle.normal.background) {
                m_BgStyle.normal.background = tex;
            }
        }
    }

    public void OnGUI(IEnumerable<string> list, string header = null)
    {
        InitStyle();
        var guicolor = GUI.color;
        var tagstyle = new GUIStyle("Foldout"); // new GUIStyle("OL Minus");

        //tagstyle.alignment = TextAnchor.UpperLeft;
        tagstyle.normal.textColor = Color.white;
        tagstyle.font = EditorStyles.boldFont;
        tagstyle.richText = true;
        var addstyle = new GUIStyle("OL Plus");
        var headstyle = new GUIStyle("label");
        headstyle.font = EditorStyles.boldFont;
        var maxWidth = float.IsNaN(this.maxWidth)
            ? EditorGUIUtility.currentViewWidth - 7
            : this.maxWidth;
        var xMax = maxWidth - 12;
        var height = EditorGUIUtility.singleLineHeight + 2;
        var newrect = EditorGUILayout.GetControlRect(GUILayout.Height(height));
        var bgrect = new Rect(newrect);
        bgrect.xMin = string.IsNullOrEmpty(header) ? 10 + indentLeft : 18 + indentLeft;
        if (!string.IsNullOrEmpty(header)) {
            var gc = new GUIContent(header);
            var w = headstyle.CalcSize(gc).x + 4;
            bgrect.width = w;
            GUI.color = new Color(0, 0, 0, 0);
            GUI.Box(bgrect, GUIContent.none, m_BgStyle);
            GUI.color = guicolor;
            var rect = new Rect(bgrect);
            rect.position += new Vector2(1, 1);
            GUI.Label(rect, gc, headstyle);
            bgrect.xMin = bgrect.xMax;
        }
        foreach (var item in list) {
            var s = item is string ? item as string : item.ToString();
            var gc = OnItemString != null ? OnItemString(item) : new GUIContent(s);
            var w = tagstyle.CalcSize(gc).x + 4;
            if (bgrect.xMin + w > xMax) {
                newrect = EditorGUILayout.GetControlRect(GUILayout.Height(height));
                bgrect = new Rect(newrect);
                bgrect.xMin = string.IsNullOrEmpty(header) ? 10 + indentLeft : 30 + indentLeft;
            }
            bgrect.width = w;
            var col = Color.white;
            if (OnItemColor != null) {
                col = OnItemColor(item);
            }
            GUI.color = col;
            GUI.Box(bgrect, GUIContent.none, m_BgStyle);
            var lum = col.grayscale > 0.5 ? col.grayscale - 0.5f : col.grayscale + 0.5f;
            GUI.color = Color.white; // new Color(lum, lum, lum, 1.0f);

            //bgrect.xMin = 16;
            var rect = new Rect(bgrect);

            //rect.position += new Vector2(1, 1);
            //tagstyle.contentOffset = new Vector2(-1,-2);
            //gc = new GUIContent(".........");
            if (GUI.Button(rect, gc, tagstyle)) {
                if (Event.current.button == 0 && OnClickItem != null) {
                    OnClickItem(item);
                }
                if (Event.current.button == 1 && OnRightClickItem != null) {
                    OnRightClickItem(rect, item);
                }
            }

            // mo: add margin
            bgrect.xMin = bgrect.xMax + 3;
            GUI.color = guicolor;
        }
        if (OnAddItem != null) {
            if (bgrect.xMin + 150 > xMax) {
                newrect = EditorGUILayout.GetControlRect(GUILayout.Height(height));
                bgrect = new Rect(newrect);
                bgrect.xMin = string.IsNullOrEmpty(header) ? 10 + indentLeft : 30 + indentLeft;
            }
            bgrect.width = 150;

            //bgrect.height += 10;
            m_BgStyle.fixedHeight = 20;

            //m_BgStyle.padding.left = 20;
            GUI.Box(bgrect, GUIContent.none, m_BgStyle);
            var gc = GUIContent.none;
            var w = tagstyle.CalcSize(gc).x;
            var rect = new Rect(bgrect);

            // fix: 加号位置
            rect.position += new Vector2(2, 2);
            rect.width = w;
            if (GUI.Button(rect, gc, addstyle)) {
                OnAddItem(m_NewItem);
                m_NewItem = string.Empty;
            }
            rect = new Rect(bgrect);
            rect.yMin += 2.5f;
            rect.yMax -= 1;
            rect.xMin += w + 5;
            rect.xMax -= 8;
            m_NewItem = GUI.TextField(rect, m_NewItem);
            bgrect.xMin = bgrect.xMax;
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return) {
                Debug.Log("enter");
                OnAddItem(m_NewItem);
                m_NewItem = string.Empty;
            }
        }
    }

}

}
#endif