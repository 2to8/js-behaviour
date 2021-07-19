using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris
{
    public class StartPanel : Component<StartPanel>
    {
        [SerializeField]
        Button goBtn;

        void OnEnable()
        {
            // goBtn = GetComponent<Button>();
            var transition = new SquaresTransition();
            if (goBtn)
                goBtn.onClick.AddListener(() => { SceneTransitionMgr.Instance.StartTransition(transition, 1); });
        }

        void TestLineRenderer() { }

        void OnDisable()
        {
            if (goBtn) goBtn.onClick.RemoveAllListeners();
        }
    }
}