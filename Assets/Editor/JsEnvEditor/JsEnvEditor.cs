﻿using System;
using System.Collections.Generic;
using Base.Runtime;
using Newtonsoft.Json;
using Puerts;
using UnityEngine;

namespace JsEnvEditor
{
    public class JsEnvEditor : IDisposable
    {
        public JsEnvEditor()
        {
            try {
                jsEnv.Eval("editor = require('index');");
                ParseJsBehaviourInfos();
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public void Tick()
        {
            if (jsEnv?.IsDisposed() == false) {
                jsEnv.Tick();
            }
        }

        public void Dispose()
        {
            jsEnv.Dispose();
        }

        /// <summary>
        /// 获取指定名字的组件信息
        /// </summary>
        /// <param name="componentName">组件名</param>
        /// <returns>组件信息</returns>
        public ComponentInfo GetJsComponentInfo(string componentName)
        {
            if (string.IsNullOrEmpty(componentName)) {
                return null;
            }

            return componentInfos.TryGetValue(componentName, out var componentInfo) ? componentInfo : null;
        }

        /// <summary>
        /// 所有的组件信息
        /// </summary>
        public Dictionary<string, ComponentInfo> ComponentInfos => componentInfos;

        private void ParseJsBehaviourInfos()
        {
            var jsonString = jsEnv.Eval<string>("editor.compInfoMgr.getJsonString();");
            componentInfos = JsonConvert.DeserializeObject<Dictionary<string, ComponentInfo>>(jsonString);
        }

        private readonly JsEnv jsEnv = new JsEnv(new JsLoaderEditor());
        private Dictionary<string, ComponentInfo> componentInfos;
    }
}