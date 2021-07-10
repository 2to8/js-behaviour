using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnParticleSystemStopped : MonoBehaviour {

    public UnityEvent trigger;

    void OnParticleSystemStopped() => trigger.Invoke();
}

}