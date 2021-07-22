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

        public static void TestArray(int[] arg)
        {
            Debug.Log("[TestInt]" + string.Join(", ", arg));
        }

        public static void TestArray2(params int[] arg) => TestArray(arg);
        public static T[] ToArray<T>(params T[] values) => values;
        public static int[] ToIntArray(params int[] values) => values;
        public static string[] ToStrArray(params string[] values) => values;
    }
}