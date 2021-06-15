using Base.Runtime;
using UnityEngine;

namespace App.Runtime
{
    public class Main : MonoBehaviour
    {
        private void Awake()
        {
            JsEnvRuntime.Inst.Init();
            JsEnvRuntime.Inst.JsEnv.Eval("var app = require('index').default || require('index');");
        }

        private void OnDestroy()
        {
            JsEnvRuntime.Inst.Shut();
        }

        private void Update()
        {
            JsEnvRuntime.Inst.Update();
        }
    }
}