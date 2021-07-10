using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnAudioFilterRead : MonoBehaviour {
    public UnityEvent<float[], int> trigger;

    void OnAudioFilterRead(float[] data, int channels) => trigger.Invoke(data, channels);
}

}