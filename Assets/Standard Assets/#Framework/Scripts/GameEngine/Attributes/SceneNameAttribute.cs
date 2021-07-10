using GameEngine.Extensions;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;

// namespace GameEngine.Attributes
// {
//     [AttributeUsage(AttributeTargets.Class)]
//     public class SceneNameAttribute : Attribute
//     {
//         public string Name;
//         public Type Type;
//         public List<string> Tags = new List<string>();
//
//         public SceneNameAttribute(object name, Type component = null, params object[] tags)
//         {
//             Name = $"{name}";
//             Type = component;
//             tags.ForEach(t => Tags.AddOnce($"{t}"._TagKey()));
//         }
//     }
// }