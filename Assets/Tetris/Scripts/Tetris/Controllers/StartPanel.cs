using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris
{
    public class StartPanel : View<StartPanel>
    {
        [SerializeField]
        Button goBtn;

        void OnEnable()
        {
            // goBtn = GetComponent<Button>();
            var transition = new SquaresTransition();
            if (goBtn)
                goBtn.onClick.AddListener(() => { SceneTransitionMgr.instance.StartTransition(transition, 1); });
        }

        void TestLineRenderer() { }

        void OnDisable()
        {
            if (goBtn) goBtn.onClick.RemoveAllListeners();
        }
    }
}