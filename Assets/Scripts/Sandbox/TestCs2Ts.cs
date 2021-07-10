using System;
using App.Runtime;
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
      
            Main.js.Eval<Action<string>>("global.hello").Invoke(GetType().FullName);
            Main.js.Eval<Action<TestCs2Ts>>("global.testBind").Invoke(this);
        }

        public void testSetValue() { }
    }
}