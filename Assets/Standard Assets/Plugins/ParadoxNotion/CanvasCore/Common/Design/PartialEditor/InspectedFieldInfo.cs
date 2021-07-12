#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Framework;
using UnityEditor;
using UnityEngine;

namespace ParadoxNotion.Design
{
    ///Contains info about inspected field rin regards to reflected inspector and object/attribute drawers
    public struct InspectedFieldInfo
    {
        ///The field inspected
        public FieldInfo field;

        ///the unityengine object serialization context
        public UnityEngine.Object unityObjectContext;

        ///the parent instance the field lives within
        public object parentInstanceContext;

        ///In case instance lives in wrapped context (eg lists) otherwise the same as parentInstanceContext
        public object wrapperInstanceContext;

        ///attributes found on field
        public object[] attributes;

        public UnityEditor.Editor editor;
        public List<UnityEditor.Editor> editors;
        public EdtDummy dummy;

        //...
        public InspectedFieldInfo(UnityEngine.Object unityObjectContext, FieldInfo field, object parentInstanceContext,
            object[] attributes)
        {
            this.unityObjectContext = unityObjectContext;
            this.field = field;
            this.parentInstanceContext = parentInstanceContext;
            this.wrapperInstanceContext = parentInstanceContext;
            this.attributes = attributes;
            editor = null;
            editors = null;
            dummy = EdtDummy.propCache.TryGetValue((this.parentInstanceContext, this.field), out var tmp)
                ? tmp
                : (EdtDummy.propCache[(this.parentInstanceContext, this.field)] =
                    ScriptableObject.CreateInstance<EdtDummy>());
        }
    }
}

#endif