#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Puerts.Attributes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Debug = UnityEngine.Debug;

namespace NodeCanvas.Framework {

[CreateAssetMenu(menuName = "NodeCanvas/" + nameof(EdtDummy), fileName = nameof(EdtDummy))]
[PuertsIgnore]
public class EdtDummy : SerializedScriptableObject {

    public static Dictionary<object, EdtDummy> cache = new Dictionary<object, EdtDummy>();
   // public object value;
     public AssetReference value;
    public List<AssetLabelReference> assetLabelReferences;
    public AssetLabelReference assetLabelReference;
    public AssetReferenceGameObject assetReferenceGameObject;
    public AssetReferenceSprite assetReferenceSprite;
    public AssetReferenceTexture assetReferenceTexture;
    public AssetReferenceTexture2D assetReferenceTexture2D;
    public AssetReferenceTexture3D assetReferenceTexture3D;
    public SerializedObject so;
    public SerializedProperty sp;
    public List<string> SavedNamespace = new List<string>();
    static EdtDummy m_Instance;
    public static Dictionary<(object, FieldInfo), EdtDummy> propCache = new Dictionary<(object, FieldInfo), EdtDummy>();

    public static EdtDummy Instance {
        get {
            if(m_Instance == null && (m_Instance = Resources.Load<EdtDummy>(nameof(EdtDummy))) == null) {
                // var path = Path.GetDirectoryName(typeof(EdtDummy).Assembly.Location)?.Replace(Application.dataPath,"Assets");
                // Debug.Log(path);
                // AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<EdtDummy>(),$"{path}/Resources/EdtDummy.asset");
                // AssetDatabase.SaveAssets();
                // AssetDatabase.Refresh();
                // m_Instance = Resources.Load<EdtDummy>("EdtDummy");
                Debug.Log(nameof(EdtDummy) + " Not Found");
                m_Instance = CreateInstance<EdtDummy>();
            }

            return m_Instance;
        }
        set => m_Instance = value;
    }

    private void Awake()
    {
        so = new SerializedObject(this);
        sp = so.FindProperty(nameof(value));
    }

}

}

#endif
