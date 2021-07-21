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
using UnityEngine;
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

        public void Invoke(MemberInfo memberInfo, Object target)
        {
            var type = memberInfo is PropertyInfo propertyInfo
                ? propertyInfo.PropertyType
                : (memberInfo is FieldInfo fieldInfo ? fieldInfo.FieldType : null);
            if (!typeof(Object).IsAssignableFrom(type)) {
                Debug.LogError($"cannot bind type: {type} => {memberInfo.Name}".ToRed(), target);
                return;
            }

            var value = TagSystem.Find(type, null, tags.ToArray());
            if (value == null) {
                Debug.LogError($"cannot find {string.Join(",", tags)}: {type} => {memberInfo.Name}".ToRed(), target);
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