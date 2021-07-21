using App.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sandbox
{
    [ExecuteInEditMode]
    public class TestJs : MonoBehaviour
    {
        [Button]
        void Awake()
        {
            Main.js.Eval("console.log('hello,js')");
        }

        public static void TestArray(int[] arg) { }
        public static void TestArray2(params int[] arg) => TestArray(arg);

    }
}