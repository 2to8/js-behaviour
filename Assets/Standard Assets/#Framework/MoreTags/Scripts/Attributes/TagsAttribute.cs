using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using GameEngine.Extensions;
using Org.BouncyCastle.Utilities.Collections;
using Sirenix.Utilities;
using TMPro;
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

        static HashSet<string> checkedScenes = new HashSet<string>();

//        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//        static void CheckTags() { }

        public void Invoke(MemberInfo memberInfo, Object target)
        {
            SceneManager.GetAllScenes().Where(scene => !checkedScenes.Contains(scene.path)).ForEach(scene => {
                if (!scene.isLoaded) {
                    return;
                }

                checkedScenes.Add(scene.path);
                var tags = new HashSet<string>();
                scene.GetRootGameObjects().ForEach(go => {
                    go.GetComponentsInChildren<Tags>(true).Where(t => t != null && t.ids.Any()).ForEach(t => {
                        t.gameObject.AddTag();
                        t.tags.ForEach(i => tags.Add(i));
                        //Debug.Log(t.gameObject.name, t.gameObject);
                        //Tags(t.ids.ToArray()).ForEach(t1 => ts.Add(t1));
                    });
                });
                Debug.Log($"check scene tags: {scene.name} tags: {tags.ToList().Join()}".ToBlue());
            });

//            var ts = new HashSet<string>();
//            Core.GetAllLoadedScenes().SelectMany(scene => scene.GetRootGameObjects()).ForEach(go => {
//                go.GetComponentsInChildren<Tags>(true).Where(t => t != null && t.ids.Any()).ForEach(t => {
//                    t.gameObject.AddTag(t.ids.ToArray());
//                    Debug.Log(t.gameObject.name, t.gameObject);
//                    //Tags(t.ids.ToArray()).ForEach(t1 => ts.Add(t1));
//                });
//            });
//            //Debug.Log("All Tags: "+ string.Join(", ",ts.ToArray()));
            var type = memberInfo is PropertyInfo propertyInfo
                ? propertyInfo.PropertyType
                : (memberInfo is FieldInfo fieldInfo ? fieldInfo.FieldType : null);
            if (!typeof(Object).IsAssignableFrom(type)) {
                Debug.LogError($"cannot bind type: {type} => {memberInfo.Name}".ToRed(), target);
                return;
            }

            var value = TagSystem.Find(type, null, tags.ToArray());
            if (value == null) {
#if UNITY_EDITOR
                Debug.Log(
                    $"loaded Scene: {string.Join(", ", SceneManager.GetAllScenes().Where(t => t.isLoaded).Select(t => t.name))} , current: {SceneManager.GetActiveScene().name}"
                        .ToRed());
#endif
                Debug.LogError(
                    $"[{memberInfo.DeclaringType?.FullName}] cannot find {string.Join(",", tags)}: {type} => {memberInfo.DeclaringType?.FullName}.{memberInfo.Name}"
                        .ToRed(), target);
            }

            if (memberInfo is FieldInfo field) {
                field.SetValue(target, value);
            }
            else if (memberInfo is PropertyInfo property) {
                property.SetValue(target, value);
            }
        }
    }
}