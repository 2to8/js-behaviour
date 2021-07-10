using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnPostRender : MonoBehaviour {
    public UnityEvent trigger;

    void OnPostRender() => trigger.Invoke();
}

}