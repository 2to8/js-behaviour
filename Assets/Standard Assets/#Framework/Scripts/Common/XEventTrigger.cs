using Puerts.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common {

public class XEventTrigger : SerializedMonoBehaviour {

    [OdinSerialize, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    public Dictionary<Component, Dictionary<string, EventTriggerData>> triggers =
        new Dictionary<Component, Dictionary<string, EventTriggerData>>();

#if UNITY_EDITOR
    [Button, PuertsIgnore]
    void assignTest()
    {
        var slide = GetComponent<Slider>();

        if (triggers == null) {
            triggers = new Dictionary<Component, Dictionary<string, EventTriggerData>>();
        }

        if (!triggers.ContainsKey(slide)) {
            triggers[slide] = new Dictionary<string, EventTriggerData>();
        }
        Assert.IsNotNull(slide);
        slide.GetType()
            .GetMembers(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p is PropertyInfo fieldInfo &&
                typeof(UnityEvent<float>).IsAssignableFrom(fieldInfo.PropertyType))
            .ForEach(p => {
                if (p is PropertyInfo fieldInfo) {
                    if (!triggers[slide].ContainsKey(fieldInfo.Name)) {
                        triggers[slide][fieldInfo.Name] = ScriptableObject.CreateInstance<EventTriggerData>();
                    }
                    var data = triggers[slide][fieldInfo.Name];
                    Debug.Log(fieldInfo.Name);
                    var ue = fieldInfo.GetValue(slide) as UnityEvent<float>;
                    var has = false;

                    for (var i = 0; i < ue.GetPersistentEventCount(); i++) {
                        ue.SetPersistentListenerState(i, UnityEventCallState.RuntimeOnly);

                        if (has = ue.GetPersistentTarget(i) == data) {
                            break;
                        }
                    }

                    if (!has) {
                        data.singleEvent = data.testEvent;
                        UnityEventTools.AddPersistentListener(ue, data.singleEvent);
                        Debug.Log("add new assign");
                    } else {
                        Debug.Log("already assigned");
                    }
                }
            });
    }
#endif

}

}