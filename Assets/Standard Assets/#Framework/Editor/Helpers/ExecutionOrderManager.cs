using Common.EditorHelpers;
using System;
using System.Linq;
using UnityEditor;

namespace Helpers
{
    [InitializeOnLoad]
    public class ExecutionOrderManager : Editor
    {
        static ExecutionOrderManager()
        {
            foreach (var monoScript in MonoImporter.GetAllRuntimeMonoScripts().Where(t => t != null)) {
                var type = monoScript?.GetClass();
                if (type == null) {
                    continue;
                }

                var attributes = type.GetCustomAttributes(typeof(ScriptExecutionOrderAttribute), true);
                if (attributes.Length == 0) {
                    continue;
                }

                var attribute = (ScriptExecutionOrderAttribute) attributes[0];
                if (MonoImporter.GetExecutionOrder(monoScript) != attribute.GetOrder()) {
                    MonoImporter.SetExecutionOrder(monoScript, attribute.GetOrder());
                }
            }
        }
    }
}