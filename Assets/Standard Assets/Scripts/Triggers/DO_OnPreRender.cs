using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnPreRender : MonoBehaviour {
    public UnityEvent trigger;

    void OnPreRender() => trigger.Invoke();
}

}