// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using Random = UnityEngine.Random;
//
// public static partial class Core {
//
//     public static TA RandomEnumValue<TA>() where TA : Enum
//     {
//         var v = Enum.GetValues(typeof(TA));
//
//         return (TA)v.GetValue(Random.Range(0, v.Length));
//     }
//
//     public static string NewGuid() => Guid.NewGuid().ToString("N");
//
//     public static List<Component> FindObjectsOfTypeAll(Type type)
//     {
//         return SceneManager.GetActiveScene()
//             .GetRootGameObjects()
//             .SelectMany(g => g.GetComponentsInChildren(type, true))
//             .ToList();
//     }
//
//     public static T FindInRoot<T>() where T : Component => FindInRoot(typeof(T)) as T;
//
//     public static Component FindInRoot(Type type)
//     {
//         return SceneManager.GetActiveScene()
//             .GetRootGameObjects()
//             .Select(t => t.GetComponent(type))
//             .FirstOrDefault(t => t != null);
//     }
//
//     public static List<T> FindObjectsOfTypeAll<T>()
//     {
//         return SceneManager.GetActiveScene()
//             .GetRootGameObjects()
//             .SelectMany(g => g.GetComponentsInChildren<T>(true))
//             .ToList();
//     }
//
//     public static Component FindObjectOfTypeAll(Type type)
//     {
//         if (!SceneManager.GetActiveScene().isLoaded) {
//             return null;
//         }
//
//         return SceneManager.GetActiveScene()
//             .GetRootGameObjects()
//             .SelectMany(g => g.GetComponentsInChildren(type, true))
//             .FirstOrDefault();
//
//         //.ToList();
//     }
//
//     public static T FindObjectOfTypeAll<T>()
//     {
//         return SceneManager.GetActiveScene()
//             .GetRootGameObjects()
//             .SelectMany(g => g.GetComponentsInChildren<T>(true))
//             .FirstOrDefault();
//
//         //.ToList();
//     }
//
// }