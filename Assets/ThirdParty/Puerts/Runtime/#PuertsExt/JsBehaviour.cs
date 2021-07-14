using System;
using Puerts;
using UnityEngine;

namespace Base.Runtime
{
    public class JsBehaviour : MonoBehaviour
    {
        protected void Awake()
        {
            JsBehaviourMgr.Instance.Add(this);
            if (string.IsNullOrEmpty(JsComponentName)) {
                return;
            }

            if (JsEnvRuntime.instance.env?.IsDisposed() != false) return;
            jsComponentId = JsEnvRuntime.instance.env.Eval<int>($"app.compInstMgr.newComponent('{JsComponentName}')");
            if (jsComponentId < 0) {
                return;
            }

            jsAwake = JsEnvRuntime.instance.env.Eval<Action>(
                $"app.compInstMgr.getComponentMethod({jsComponentId}, 'Awake')");
            jsStart = JsEnvRuntime.instance.env.Eval<Action>(
                $"app.compInstMgr.getComponentMethod({jsComponentId}, 'Start')");
            jsOnEnable =
                JsEnvRuntime.instance.env.Eval<Action>($"app.compInstMgr.getComponentMethod({jsComponentId}, 'OnEnable')");
            jsOnDisable =
                JsEnvRuntime.instance.env.Eval<Action>($"app.compInstMgr.getComponentMethod({jsComponentId}, 'OnDisable')");
            jsOnDestroy =
                JsEnvRuntime.instance.env.Eval<Action>($"app.compInstMgr.getComponentMethod({jsComponentId}, 'OnDestroy')");
            jsBindProperty =
                JsEnvRuntime.instance.env.Eval<Action<int>>(
                    $"app.compInstMgr.getComponentMethod({jsComponentId}, 'BindProperty')");
            jsUnbindProperty =
                JsEnvRuntime.instance.env.Eval<Action>(
                    $"app.compInstMgr.getComponentMethod({jsComponentId}, 'UnbindProperty')");
            jsBindProperty?.Invoke(GetInstanceID());
            jsAwake?.Invoke();
        }

        protected void Start()
        {
            if (JsEnvRuntime.instance.env?.IsDisposed() == false)
                jsStart?.Invoke();
        }

        protected void OnEnable()
        {
            if (JsEnvRuntime.instance.env?.IsDisposed() == false)
                jsOnEnable?.Invoke();
        }

        protected void OnDisable()
        {
            if (JsEnvRuntime.instance.env?.IsDisposed() == false)
                jsOnDisable?.Invoke();
        }

        protected void OnDestroy()
        {
            if (JsEnvRuntime.instance.env?.IsDisposed() != false) return;
            jsOnDestroy?.Invoke();
            jsUnbindProperty?.Invoke();
            jsAwake = null;
            jsStart = null;
            jsOnEnable = null;
            jsOnDisable = null;
            jsOnDestroy = null;
            JsEnvRuntime.instance.env.Eval($"app.compInstMgr.delComponent({jsComponentId})");
            JsBehaviourMgr.Instance.Remove(GetInstanceID());
        }

        [SerializeField]
        public string JsComponentName;

        [SerializeField]
        public JsComponentProp JsComponentProp;

        /// <summary>
        /// js组件实例ID
        /// </summary>
        private int jsComponentId;

        private Action jsAwake;
        private Action jsStart;
        private Action jsOnEnable;
        private Action jsOnDisable;
        private Action jsOnDestroy;
        private Action<int> jsBindProperty;
        private Action jsUnbindProperty;
    }
}