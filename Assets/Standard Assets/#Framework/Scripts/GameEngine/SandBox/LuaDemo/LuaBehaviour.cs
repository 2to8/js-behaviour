#if XLUA
/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace GameKit.Scripts.LuaDemo
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
        public AssetReference reference;
    }

    [LuaCallCSharp]
    [AddComponentMenu("App/Lua Behaviour")]
    public class LuaBehaviour : MonoBehaviour
    {
        public AssetReference reference;

        public List<AssetReference> references = new List<AssetReference>();

        [ShowInInspector]
        private TextAsset m_LuaScript;

        public Injection[] injections;

        internal static readonly LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGcTime = 0;
        internal const float GC_INTERVAL = 1; //1 second

        private Action m_LuaStart;
        private Action m_LuaUpdate;
        private Action m_LuaOnDestroy;

        private TextAsset m_Asset;

        private LuaTable m_ScriptEnv;
        private AsyncOperationHandle<TextAsset> m_Async;

        IEnumerator LoadScript()
        {
            Debug.Log($"{m_Async.IsValid()}");
            if(!m_Async.IsValid()){
                m_Async = Addressables.LoadAssetAsync<TextAsset>(reference);
                m_Async.Completed += (t) =>
                {
                    Debug.Log($"{m_Async.PercentComplete * 100f:F0}% LOADED!");

                    // At this point the scene is loaded and referenced in async.Result
                    //Debug.Log("");
                };
            }

            while(!m_Async.IsDone){
                //ProgressNumber.text = (async.PercentComplete * 100f).ToString("F0") + ("%");
                //ProgressSlider.value = async.PercentComplete;

                //Debug.Log(async.PercentComplete);
                Debug.Log($"{m_Async.PercentComplete * 100f:F0}%");

                yield return m_Async;
                if(m_LuaScript == null){
                    m_LuaScript = m_Async.Result;
                    InitLua();
                    Debug.Log("textasset loaded");
                }
            }

            //action?.Invoke();
        }

        void InitLua()
        {
            Debug.Log("init lua");
            m_ScriptEnv = luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            m_ScriptEnv.SetMetaTable(meta);
            meta.Dispose();


            m_ScriptEnv.Set("self", this);
            foreach(var injection in injections){
                if(injection.reference != null){
                    StartCoroutine(LoadPrefab(injection.reference));
                }

                m_ScriptEnv.Set(injection.name, injection.value);
            }

            luaEnv.DoString(m_LuaScript.text, "LuaTestScript", m_ScriptEnv);

            m_ScriptEnv.Get("start", out m_LuaStart);
            m_ScriptEnv.Get("update", out m_LuaUpdate);
            m_ScriptEnv.Get("ondestroy", out m_LuaOnDestroy);

            Action luaAwake = m_ScriptEnv.Get<Action>("awake");
            luaAwake?.Invoke();

            //luaAwake?.Invoke();
        }

        IEnumerator LoadPrefab(AssetReference refs)
        {
            var async = Addressables.InstantiateAsync(refs);
            async.Completed += (t) =>
            {
                Debug.Log($"{(async.PercentComplete * 100f):F0}% LOADED!");

                // At this point the scene is loaded and referenced in async.Result
                // Debug.Log("");
            };
            while(!async.IsDone){
                //ProgressNumber.text = (async.PercentComplete * 100f).ToString("F0") + ("%");
                //ProgressSlider.value = async.PercentComplete;

                //Debug.Log(async.PercentComplete);
                Debug.Log($"{async.PercentComplete * 100f:F0}%");

                yield return async;
            }
        }

        [Button]
        void Awake()
        {
            Debug.Log("awake...");

//            if(luaAwake != null) {
//                luaAwake();
//            }
        }

        // Use this for initialization
        [Button]
        IEnumerator Start()
        {
            yield return StartCoroutine(LoadScript());

            //yield return async;
            Debug.Log("start...");
            m_LuaStart?.Invoke();

//            if (luaStart != null)
//            {
//                luaStart();
//            }
        }

        // Update is called once per frame
        void Update()
        {
            //StartCoroutine(loadScript(() => {
            //Debug.Log("");
            if(m_Async.IsDone){
                m_LuaUpdate?.Invoke();
            }

            //}));

//            if (luaUpdate != null)
//            {
//                luaUpdate();
//            }
            if(Time.time - LuaBehaviour.lastGcTime > GC_INTERVAL){
                luaEnv.Tick();
                LuaBehaviour.lastGcTime = Time.time;
            }
        }

        void OnDestroy()
        {
            if(m_Async.IsDone){
                m_LuaOnDestroy?.Invoke();
            }

//            if (luaOnDestroy != null)
//            {
//                luaOnDestroy();
//            }
            m_LuaOnDestroy = null;
            m_LuaUpdate = null;
            m_LuaStart = null;
            m_ScriptEnv?.Dispose();
            injections = null;
        }
    }
}
#endif