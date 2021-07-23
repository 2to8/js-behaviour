using Common.JSRuntime;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainScene.BootScene.Utils
{
    public class VersionText : Manager<VersionText>
    {
        [SerializeField]
        Button m_Button;

        public override void Start()
        {
            base.Start();
            if (TryGetComponent<TMP_Text>(out var component))
                component.text = Application.version;
            else if (TryGetComponent<Text>(out var text)) text.text = Application.version;
            m_Button ??= GetComponentInChildren<Button>() ?? GetComponentInParent<Button>();
            m_Button?.onClick.AddListener(() => {
                Env.forceReload = true;
                SceneManager.LoadScene(0);
            });
        }
    }
}