using System;
using App.Runtime;
using Base.Runtime;
using Puerts;
using UnityEngine;

namespace Sandbox
{
    public class TestScript : MonoBehaviour
    {
        public JSObject script;

        void Start()
        {
            var js = Main.JsEnv.Eval<Action<TestScript>>("global.testScript");
            Debug.Log(js != null);
            js?.Invoke(this);
        }
    }
}