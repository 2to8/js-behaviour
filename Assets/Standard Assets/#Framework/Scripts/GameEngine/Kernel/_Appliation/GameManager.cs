namespace GameEngine.Kernel._Appliation {

public class GameManager : Provider<GameManager> {

#if XLUA
        #region LUA
        private static LuaEnv m_Lua;

        public static Dictionary<string, TextAsset> luaAssets = new Dictionary<string, TextAsset>();

        // [LabelText("血量"),HorizontalGroup("hp"),ShowInInspector]
        // public int hpMin => 1;
        //
        // [HorizontalGroup("hp"),LabelText(""), ShowInInspector]
        // public int hpMax => 100;



        public static LuaEnv luaEnv {
            get {
                if(m_Lua == null) {
                    m_Lua = new LuaEnv(); //all lua behaviour shared one luaenv only!
                    m_Lua.AddLoader((ref string filename) => {
                        var path = filename;
                        filename = $"{Application.dataPath}/XLua/Lua/{filename}.lua";

                        if(luaAssets.ContainsKey($"Lua/{path}.lua")) {
                            return luaAssets[$"Lua/{path}.lua"].bytes;
                        }

                        // var handle = Addressables.LoadAssetAsync<TextAsset>($"Lua/{path}.lua");
                        // DateTime nowTime = DateTime.Now;
                        // while(!handle.IsDone) { }

                        // TextAsset result = null;
                        // handle.Completed += (t => {
                        //     result = t.Result;
                        //
                        //     //Debug.Log(t.Result.text);
                        //     Debug.Log($"len: {t.Result.text.Length} path: {path} time: {(DateTime.Now - nowTime).Seconds}");
                        // });
                        // Debug.Log("start");
                        //
                        // Debug.Log("done");

                        // var wait = true;

                        // while(wait && (DateTime.Now - nowTime).Seconds < 10) {
                        //     //System.Threading.Thread.Sleep(20);
                        // }

                        // return result?.bytes;

                        //     LuaLoader(path).Wait();
                        //  var re = LuaLoader(path).GetAwaiter().GetResult();
                        //
                        // return re;
                        //return LuaLoader(path).Result;

                        return null;
                    });
                }

                return m_Lua;
            }
        }

        private static async Task<byte[]> LuaLoader(string filePath) {
            string absPath = Application.dataPath + "/XLua/Lua/" + filePath + ".lua";
            Debug.Log(absPath);
            if(File.Exists(absPath)) {
                //return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(absPath));
            }

            var re = await Addressables.LoadAssetAsync<TextAsset>($"Lua/{filePath}.lua").Task;

            return re?.bytes;

            return LoadAsync(filePath).Result?.bytes;

            //
            // var result = loadAsync(path);
            // // var tcs = new TaskCompletionSource<TextAsset>();
            // // var task = Task.Run(async () => tcs.SetResult(await loadAsync(path))  );
            // // TextAsset result = null;
            // // tcs.Task.ConfigureAwait(true).GetAwaiter().OnCompleted(() => {
            // //     result = tcs.Task.Result;
            // // });
            //
            // if(result.Result != null) {
            //     return result.Result.bytes;
            // }
            //
            // return null;
        }

        private static async Task<TextAsset> LoadAsync(string path) {
            return await Addressables.LoadAssetAsync<TextAsset>($"Lua/{path}.lua").Task;
        }

        private void LoadLua(string path) {
            ScriptableObject.CreateInstance<LuaProxy>().Run(path);
        }

        public void LoadLua(AssetReference reference) {
            ScriptableObject.CreateInstance<LuaProxy>().Run(reference);
        }

        #endregion
#endif

    void OnDestroy()
    {
        //m_Lua?.Dispose();
    }

}

}