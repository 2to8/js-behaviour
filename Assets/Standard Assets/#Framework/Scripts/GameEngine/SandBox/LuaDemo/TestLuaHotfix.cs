#if XLUA
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace GameKit.Scripts.LuaDemo
{
    [LuaCallCSharp]
    public class TestLuaHotfix : MonoBehaviour
    {
        public AssetReference reference;
        internal static readonly LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!

        IEnumerator  Start()
        {
            var async = Addressables.LoadAssetAsync<TextAsset>(reference);
            yield return async;
            using(var m_ScriptEnv = luaEnv.NewTable()){
                luaEnv.DoString(async.Result.text, "LuaTestScript", m_ScriptEnv);
            }
        }

        [Hotfix]
        public void ShowVersion()
        {
            Debug.Log("test");
        }
    }
}
#endif