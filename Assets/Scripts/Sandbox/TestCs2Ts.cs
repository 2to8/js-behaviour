using System;
using App.Runtime;
using App.Support;
using Puerts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sandbox
{
    public class TestCs2Ts : MonoBehaviour
    {
        public int num;

        [Button]
        void _测试cs绑定到cs()
        {
            this.Js().Eval<Action<string>>("$hello").Invoke(GetType().FullName);
            this.JsCall("test2");
        }
    }
}