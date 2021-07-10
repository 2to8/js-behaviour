using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnParticleCollision : MonoBehaviour {

    public UnityEvent<GameObject> trigger;

    void OnParticleCollision(GameObject other) => trigger.Invoke(other);
}

}