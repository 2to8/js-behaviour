using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using GameEngine.Extensions;
using Org.BouncyCastle.Utilities.Collections;
using Sirenix.Utilities;
using TMPro;
using UniRx.Async;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MoreTags.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class TagsAttribute : Attribute
    {
        public HashSet<string> tags = new HashSet<string>();

        public TagsAttribute(params object[] tags)
        {
            tags.ForEach(t => this.tags.Add((t is Enum @enum ? @enum.GetFullName() : $"{t}")._TagKey()));
        }

        static HashSet<MemberInfo> caches = new HashSet<MemberInfo>();

//        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//        static void CheckTags() { }

        public void Invoke(MemberInfo memberInfo, Component target)
        {


//            var ts = new HashSet<string>();
//            Core.GetAllLoadedScenes().SelectMany(scene => scene.GetRootGameObjects()).ForEach(go => {
//                go.GetComponentsInChildren<Tags>(true).Where(t => t != null && t.ids.Any()).ForEach(t => {
//                    t.gameObject.AddTag(t.ids.ToArray());
//                    Debug.Log(t.gameObject.name, t.gameObject);
//                    //Tags(t.ids.ToArray()).ForEach(t1 => ts.Add(t1));
//                });
//            });
//            //Debug.Log("All Tags: "+ string.Join(", ",ts.ToArray()));

//            if (memberInfo is PropertyInfo pi) {
//                if (pi.GetValue(target) != null) {
//                    Debug.Log(pi.Name + " not null");
//                    return;
//                }
//            }
//            else if (memberInfo is FieldInfo fi) {
//                if (fi.GetValue(target) != null){
//                    Debug.Log(fi.Name + " not null");
//                   return; 
//                }
//            }
            if (caches.Contains(memberInfo) && Application.isPlaying) return;
            var type = memberInfo is PropertyInfo propertyInfo
                ? propertyInfo.PropertyType
                : (memberInfo is FieldInfo fieldInfo ? fieldInfo.FieldType : null);
            if (!typeof(Object).IsAssignableFrom(type)) {
                Debug.LogError($"cannot bind type: {type} => {memberInfo.Name}".ToRed(), target);
                return;
            }

            var value = TagSystem.Find(type, null, tags.ToArray());
            if (value == null) {
//#if UNITY_EDITOR
//                Debug.Log(
//                    $"loaded Scene: {string.Join(", ", SceneManager.GetAllScenes().Where(t => t.isLoaded).Select(t => t.name))} , current: {SceneManager.GetActiveScene().name}"
//                        .ToRed());
//#endif
//                Debug.Log(
//                    $"[{memberInfo.DeclaringType?.FullName}] cannot find {string.Join(",", tags)}: {type} => {memberInfo.DeclaringType?.FullName}.{memberInfo.Name}"
//                        .ToRed(), target);
                return;
            }

            if (memberInfo is FieldInfo field) {
                field.SetValue(target, value);
            }
            else if (memberInfo is PropertyInfo property) {
                property.SetValue(target, value);
            }

            Debug.Log($"[Binding] {tags.Join()} => {memberInfo.DeclaringType?.FullName}.{memberInfo.Name}".ToBlue(),
                target.gameObject);
            caches.Add(memberInfo);
        }
    }
}