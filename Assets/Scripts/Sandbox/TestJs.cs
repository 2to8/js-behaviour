using App.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

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
    }
}