using System;
using UnityEngine;

namespace GameEngine.Extensions
{
    public static class GameObjectExt
    {
        public static void SetParent(this GameObject gameObject, Transform parent) =>
            gameObject.transform.SetParent(parent);

        public static void SetParent(this GameObject gameObject, GameObject parent) =>
            gameObject.transform.SetParent(parent.transform);

        public static T AddComponentOnce<T>(this GameObject gameObject) where T : Component =>
            gameObject?.GetComponent<T>() ?? gameObject?.AddComponent<T>();

        public static MonoBehaviour AddComponentOnce(this GameObject gameObject, Type type) =>
            (MonoBehaviour) (gameObject?.GetComponent(type) ?? gameObject?.AddComponent(type));

        public static bool ToggleShow(this GameObject gameObject)
        {
            var ret = !gameObject.activeInHierarchy;
            gameObject.SetActive(ret);
            return ret;
        }

        public static T AddComonentOnce<T>(this Transform transform) where T : Component =>
            transform?.GetComponent<T>() ?? transform?.gameObject.AddComponent<T>();

        public static MonoBehaviour AddComonentOnce(this Transform transform, Type type) =>
            (MonoBehaviour) (transform?.GetComponent(type) ?? transform?.gameObject.AddComponent(type));

        public static T AddComonentOnce<T>(this MonoBehaviour mb) where T : Component =>
            mb?.GetComponent<T>() ?? mb?.gameObject.AddComponent<T>();

        public static MonoBehaviour AddComonentOnce(this MonoBehaviour mb, Type type) =>
            (MonoBehaviour) (mb?.GetComponent(type) ?? mb?.gameObject.AddComponent(type));

        public static T AddComponentOnce<T>(this Component component) where T : Component =>
            component?.GetComponent<T>() ?? component?.gameObject.AddComponent<T>();

        public static MonoBehaviour AddComponentOnce(this Component component, Type type) =>
            (MonoBehaviour) (component?.GetComponent(type) ?? component?.gameObject.AddComponent(type));
    }
}