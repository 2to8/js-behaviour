using GameEngine.Kernel;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using SqlCipher4Unity3D;
using SQLite.Attributes;
using System;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif
using UnityEngine;

namespace GameEngine.Models.Contracts {

//[Serializable]
public class TableBase : SerializedScriptableObject {
    [Unique, PrimaryKey, AutoIncrement, OdinSerialize, ReadOnly,FoldoutGroup("base")]
    public virtual int Id { get; set; }

    [OdinSerialize,FoldoutGroup("base")]
    public virtual bool IsUser { get; set; }

    [Ignore]
    public new string name { get => base.name; set => base.name = value; }

    [Ignore]
    public new HideFlags hideFlags { get => base.hideFlags; set => base.hideFlags = value; }

    [OdinSerialize, Title("是否启用 ", HorizontalLine = false),FoldoutGroup("base")]
    public bool Enabled { get; set; } = true;

    [OdinSerialize, ReadOnly,FoldoutGroup("base")]
    public virtual long Created { get; set; } = Core.TimeStamp();

    [OdinSerialize, ReadOnly,FoldoutGroup("base")]
    public virtual long Updated { get; set; } = Core.TimeStamp();

    long FriendlyTime(long timestamp, GUIContent label)
    {
    #if UNITY_EDITOR
        SirenixEditorGUI.BeginBox();

        //callNextDrawer(label);
        if (timestamp > 0) {
            EditorGUILayout.HelpBox(Core.RelativeFriendlyTime(timestamp), MessageType.None);
        }
        var result = EditorGUILayout.LongField(label ?? new GUIContent(""), timestamp);

        //var result = EditorGUILayout.Slider(value, this.From, this.To);
        SirenixEditorGUI.EndBox();

        return result;
    #endif
        return default;
    }

    public SQLiteConnection _db => IsUser ? Core.UserConnection : Core.Connection;

    //
    // public static T CreateOne<T>() where T : TModel<T>, new()
    // {
    //     return CreateInstance<T>();
    // }
}

}