using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

namespace GameEngine.Domain {

public class LoadingProgressBar : MonoBehaviour {

    [SerializeField]
    public Slider m_Slider;

    public static LoadingProgressBar Instance;

    void Reset()
    {
        if (m_Slider == null) {
            m_Slider = GetComponent<Slider>();
        }
    }

    public AsyncOperationHandle<SceneInstance> Handle;

    void Awake()
    {
        Instance = this;

        if (m_Slider != null) {
            m_Slider.value = 0f;
            m_Slider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Handle.IsValid() && !Handle.IsDone && m_Slider != null) {
            var value = Handle.PercentComplete;
            m_Slider.value = value;
            var show = value > 0f && value < 1f;
            m_Slider.gameObject.SetActive(show);
        }
    }

}

}