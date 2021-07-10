using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnParticleTrigger : MonoBehaviour {
    public UnityEvent trigger;

    void OnParticleTrigger() => trigger.Invoke();
}

}