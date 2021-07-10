using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnParticleUpdateJobScheduled : MonoBehaviour {
    public UnityEvent trigger;

    void OnParticleUpdateJobScheduled() => trigger.Invoke();
}

}