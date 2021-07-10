using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameEngine.Extensions
{
#if UNITY_EDITOR
#endif

    /// <summary>
    ///     unity Component Extention
    /// </summary>
    public static class UnityComponentExtention
    {
        /// <summary>
        ///     get / add component to target GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T RequireComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null) {
                comp = go.AddComponent<T>();
            }

            return comp;
        }

        /// <summary>
        ///     get / add component to target Transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static T RequireComponent<T>(this Transform trans) where T : Component =>
            RequireComponent<T>(trans.gameObject);

        public static MonoBehaviour RequireComponent(this Transform trans, Type type) =>
            RequireComponent(trans.gameObject, type);

        public static T RequireComponent<T>(this Component trans) where T : Component =>
            RequireComponent<T>(trans.gameObject);

        public static MonoBehaviour RequireComponent(this Component trans, Type type) =>
            RequireComponent(trans.gameObject, type);

        /// <summary>
        ///     get / add component to target GameObject, ref tips if is add
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T RequireComponent<T>(this GameObject go, ref bool isAddNewComp) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (isAddNewComp = comp == null) {
                comp = go.AddComponent<T>();
            }

            return comp;
        }

        public static MonoBehaviour RequireComponent(this GameObject go, Type type)
        {
            if (type == null) {
                return null;
            }

            var comp = go.GetComponent(type);
            if (comp == null) {
                comp = go.AddComponent(type);
            }

            return comp as MonoBehaviour;
        }

        /// <summary>
        ///     get / add component to target Transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static T RequireComponent<T>(this Transform trans, ref bool isAddNewComp) where T : Component =>
            RequireComponent<T>(trans.gameObject, ref isAddNewComp);

        /// <summary>
        ///     Get Target Component in Transform layer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trans"></param>
        /// <param name="layerStr"></param>
        /// <returns></returns>
        public static T FindFirstComponentInChild<T>(this Transform trans, string layerStr) where T : Component
        {
            var tagTrans = trans.Find(layerStr);
            if (tagTrans != null) {
                return tagTrans.GetComponent<T>();
            }

            return null;
        }

        /// <summary>
        ///     Get Target Component in GameObject layer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="layerStr"></param>
        /// <returns></returns>
        public static T FindFirstComponentInChild<T>(this GameObject go, string layerStr) where T : Component =>
            go.transform.FindFirstComponentInChild<T>(layerStr);

        /// <summary>
        ///     Wrapped Func for auto check unreferenced component... suppose is should be deprecated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="monoBehaviour"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool TryFindFirstComponentInChild<T>(this MonoBehaviour monoBehaviour, ref T comp)
            where T : Component
        {
            if (comp == null) {
                comp = monoBehaviour.GetComponentInChildren<T>();
            }

            return comp != null;
        }

        /// <summary>
        ///     Wrapped Get Child GameObject
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="layerStr"></param>
        /// <returns></returns>
        public static GameObject FindChildGameObject(this Transform trans, string layerStr)
        {
            var tagTrans = trans.Find(layerStr);
            return tagTrans != null ? tagTrans.gameObject : null;
        }

        /// <summary>
        ///     Wrapped Set active
        /// </summary>
        /// <param name="go"></param>
        /// <param name="active"></param>
        public static void SetActive(this GameObject go, bool active, bool recursively, bool includeInactive = true)
        {
            go.SetActive(active);
            if (recursively) {
                var allTrans = go.GetComponentsInChildren<Transform>(includeInactive);
                for (var i = 0; i < allTrans.Length; i++) {
                    allTrans[i].gameObject.SetActive(active);
                }
            }
        }

        /// <summary>
        ///     Find with type def
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="layerStr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Component FindFirstComponentInChild(this Transform trans, string layerStr, Type type)
        {
            var tagTrans = trans.Find(layerStr);
            if (tagTrans != null) {
                return tagTrans.GetComponent(type);
            }

            return null;
        }

        /// <summary>
        ///     Find with type def
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layerStr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Component FindFirstComponentInChild(this GameObject go, string layerStr, Type type) =>
            FindFirstComponentInChild(go.transform, layerStr, type);

        public static bool DestroySelf(this Transform go, float delay = 0.0f) => DestroySelf(go?.gameObject, delay);

        public static bool DestroySelf(this Object go, float delay = 0.0f)
        {
            if (!go) {
                return false;
            }

            if (delay > 0.0f) {
                if (Application.isPlaying) {
                    Object.Destroy(go, delay);
                }
                else {
                    Object.DestroyImmediate(go, false);
                }
            }
            else {
                DestroyObject(ref go);
            }

            return true;
        }

        // destroy test
        public static bool DestroySelf(this GameObject go, float delay = 0.0f)
        {
            if (!go) {
                return false;
            }

            if (delay > 0.0f) {
                if (Application.isPlaying) {
                    Object.Destroy(go, delay);
                }
                else {
                    Object.DestroyImmediate(go, false);
                }
            }
            else {
                DestroyGameObject(ref go);
            }

            return true;
        }

        public static void SetLayer(this GameObject go, int layer, bool withChildren)
        {
            if (go != null && go.layer != layer) {
                go.layer = layer;
                if (withChildren) {
                    foreach (var tran in go.GetComponentsInChildren<Transform>(true)) {
                        tran.gameObject.layer = layer;
                    }
                }
            }
        }

        /// <summary>
        ///     wrapped func for destroy gameobject
        /// </summary>
        /// <param name="go"></param>
        public static void DestroySelf(this Object obj)
        {
            if (obj != null) {
                if (Application.isPlaying) {
                    Object.Destroy(obj);
                }
                else {
                    Object.DestroyImmediate(obj);
                }
            }
        }

        /// <summary>
        ///     wrapped func for destroy gameobject
        /// </summary>
        /// <param name="go"></param>
        public static void DestroyGameObject(ref GameObject go)
        {
            if (go != null) {
                if (Application.isPlaying) {
                    Object.Destroy(go);
                }
                else {
#if UNITY_EDITOR
                    if (PrefabUtility.IsPartOfPrefabInstance(go.transform)) {
                        var prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                        PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.Completely,
                            InteractionMode.AutomatedAction);
                    }

#endif
                    Object.DestroyImmediate(go);
                }
            }

            go = null;
        }

        /// <summary>
        ///     wrapped func for destroy gameobject via Transform
        /// </summary>
        /// <param name="go"></param>
        public static void DestroyGameObject<T>(ref T comp) where T : Component
        {
            if (comp != null) {
                var go = comp.gameObject;
                DestroyGameObject(ref go);
            }

            comp = null;
        }

        /// <summary>
        ///     wrapped func for destroy any UnityObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void DestroyObject<T>(ref T obj) where T : Object
        {
            if (obj != null) {
                if (Application.isPlaying) {
                    Object.Destroy(obj);
                }
                else {
                    Object.DestroyImmediate(obj);
                }
            }

            obj = null;
        }

        /// <summary>
        ///     ResetTransform base info
        /// </summary>
        /// <param name="trans"></param>
        public static void ResetTransform(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.rotation = Quaternion.identity;
            trans.localScale = Vector3.one;
        }

        /// <summary>
        ///     ResetTransform base info
        /// </summary>
        /// <param name="go"></param>
        public static void ResetTransform(this GameObject go)
        {
            go.transform.ResetTransform();
        }

        /// <summary>
        ///     Temp Copy
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static Transform CopyTempTransform(this Transform trans)
        {
            var retVal = new GameObject("temp trans").transform;
            retVal.position = trans.position;
            retVal.rotation = trans.rotation;
            return retVal;
        }

        public static void ResetParent(this Transform trans, Transform parent, bool resetTransform = false)
        {
            if (parent != null) {
                trans.parent = parent;
                if (resetTransform) {
                    trans.ResetTransform();
                }
            }
        }

        /// <summary>
        ///     Look At 2D Version
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="pos"></param>
        public static void LookAt_2D(this Transform trans, Vector3 pos)
        {
            trans.LookAt(new Vector3(pos.x, trans.position.y, pos.z));
        }

        /// <summary>
        ///     Look At Direction
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="pos"></param>
        public static void LookAtDir(this Transform trans, Vector3 dir)
        {
            var pos = trans.position + dir;
            trans.LookAt(pos);
        }

        public static void LookAtDir_2D(this Transform trans, Vector3 dir)
        {
            var pos = trans.position + dir;
            pos.y = trans.position.y;
            trans.LookAt(pos);
        }

        // RectTransform set parent wrapped func, the parent changed will make anchoredposition changed
        public static void WrappedSetParent(this RectTransform rectTrans, Transform parent)
        {
            var oldPos = rectTrans.anchoredPosition;
            rectTrans.SetParent(parent);
            rectTrans.ResetTransform();
            rectTrans.anchoredPosition = oldPos;
        }

        public static AnimationCurve Clear(this AnimationCurve curve)
        {
            var clearCount = curve.length;
            for (var i = clearCount - 1; i >= 0; i--) {
                curve.RemoveKey(i);
            }

            return curve;
        }

        public static float GetCurrentClipLength(this Animator animator, float defaultVal = 1.0f)
        {
            var animationClips = animator.GetCurrentAnimatorClipInfo(0);
            if (animationClips != null && animationClips.Length > 0) {
                return animationClips[0].clip.length;
            }

            return defaultVal;
        }

        public static float GetNextClipLength(this Animator animator, float defaultVal = 1.0f)
        {
            var animationClips = animator.GetNextAnimatorClipInfo(0);
            if (animationClips != null && animationClips.Length > 0) {
                return animationClips[0].clip.length;
            }

            return defaultVal;
        }

        #region Factory

        // wrapped func to create a new GameObject whitch component attatched
        public static T NewComponent<T>(string objectName = null, Vector3? setPos = null) where T : Component
        {
            var go = new GameObject(objectName ?? "NewObject_" + typeof(T).Name);
            var comp = go.AddComponent<T>();
            if (setPos.HasValue) {
                comp.transform.position = setPos.Value;
            }

            return comp;
        }

        public static string PlayerPrefs_GetString(string key, string defaultString)
        {
            if (PlayerPrefs.HasKey(key) == false) {
                PlayerPrefs.SetString(key, defaultString);
                PlayerPrefs.Save();
            }

            return PlayerPrefs.GetString(key);
        }

        public static int PlayerPrefs_GetInt(string key, int defaultInt)
        {
            if (PlayerPrefs.HasKey(key) == false) {
                PlayerPrefs.SetInt(key, defaultInt);
                PlayerPrefs.Save();
            }

            return PlayerPrefs.GetInt(key);
        }

        public static float PlayerPrefs_GetFloat(string key, float defaultFloat)
        {
            if (PlayerPrefs.HasKey(key) == false) {
                PlayerPrefs.GetFloat(key, defaultFloat);
                PlayerPrefs.Save();
            }

            return PlayerPrefs.GetFloat(key);
        }

        #endregion
    }
}