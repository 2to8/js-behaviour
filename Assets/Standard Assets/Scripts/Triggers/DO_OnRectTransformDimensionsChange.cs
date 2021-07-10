using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnRectTransformDimensionsChange : MonoBehaviour {
    public UnityEvent trigger;

    void OnRectTransformDimensionsChange() => trigger.Invoke();
}

}