using Puerts.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Puerts
{
    public static partial class JsEnvExt
    {
        public static void Call<T>(this JsEnv jsEnv, T obj, string fn) =>
            jsEnv.Eval<Action<T, string>>("$require").Invoke(obj, fn);

        public static bool DebuggerJoined(this JsEnv jsEnv) => PuertsDLL.InspectorTick(jsEnv.isolate);

        public static bool IsDisposed(this JsEnv jsEnv)
        {
            try {
                jsEnv?.CheckLiveness();
                return jsEnv == null;
            }
            catch (InvalidOperationException e) {
                return true;
            }
        }

        #region Extra

        //public static bool operator true(JsEnv mt) => mt != null && mt.isolate != IntPtr.Zero && !mt.disposed;
        //public static bool operator false(JsEnv mt) => mt == null || mt.isolate == IntPtr.Zero || mt.disposed;

        // public static bool operator ==(JsEnv mt, object o) => (mt?.disposed == true && (mt.disposed || o == null)) ||  mt.Equals(o);
        // public static bool operator !=(JsEnv mt, object o) => mt == null || mt.disposed;
        //public static bool operator !(JsEnv env) => env == null || env.isolate == IntPtr.Zero || env.disposed;
        //public static int defaultDebugPort = 9229;
        public static ILoader defaultLoader = new DefaultLoader();

        #endregion

        public static Action<JsEnv> Completed;
        public static List<Action<JsEnv>> CompletedActions = new List<Action<JsEnv>>();
        public static int DebugPort = -1;
        public static bool isDebugReady(this JsEnv jsEnv) => DebugPort == -1 || PuertsDLL.InspectorTick(Alive.isolate);
        static bool exited;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void RuntimeReload()
        {
            var i = 0;
            foreach (var env in JsEnv.jsEnvs.ToList()) {
                if (env != null) {
                    Debug.Log($"Clean JsEnv({i += 1})");
                    env.Dispose();
                }
            }
        }

        public static int AliveId => Alive.Idx;
        public static JsEnv m_Alive;
        public static bool IsAlive => Alive.isolate != IntPtr.Zero;
        public static ILoader Loader;

        public static JsEnv Alive {
            get {
                if (m_Alive != null && m_Alive.isolate != IntPtr.Zero) {
                    return m_Alive;
                }

                if (!JsEnv.jsEnvs.Any() || JsEnv.jsEnvs.All(env => env.isolate == IntPtr.Zero)) {
                    return m_Alive = new JsEnv(defaultLoader, defaultDebugPort);
                }

                return JsEnv.jsEnvs.FirstOrDefault(env => env.isolate != IntPtr.Zero);
            }
            set => m_Alive = value;
        }

        public static IEnumerator WaitDebuggerAsync(this JsEnv jsEnv, Action<JsEnv> action = null,
            bool emptyOnly = false)
        {
            var needWait = Completed == null || action == null;
            if (action != null && (!emptyOnly || needWait)) {
                Completed += action;
            }

            if (needWait) {
                var reconnect = false;
                while (!exited && DebugPort != -1 && !PuertsDLL.InspectorTick(Alive.isolate)) {
                    Debug.Log("wait debugger");
                    yield return null;
                    reconnect = true;
                }

                if (reconnect) {
                    Debug.Log("--------------- debugger connected ---------------------");
                }

                if (!exited) {
                    Completed?.Invoke(jsEnv);
                    Completed = null;
                }
            }
        }

        public static Action<JsEnv> OnInit;
        static int defaultDebugPort;

        static void ExtendInit(this JsEnv jsEnv, ILoader loader, int debugPort)
        {
            defaultLoader = loader;
            defaultDebugPort = debugPort;
            DebugPort = debugPort;
            Loader = loader;
            OnInit?.Invoke(jsEnv);

            // Filter.GetFilters<RegisterEnvAttribute, Action<JsEnv>>(li => {
            //         return li.OrderBy(mb => mb.GetCustomAttribute<RegisterEnvAttribute>().Order).ToList();
            //     })
            //     .ForEach(action => action?.Invoke(this));
        }
    }
}