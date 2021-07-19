using System.Linq;
using Consts;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tetris.Blocks;
using Unity.Assertions;
using UnityEngine;

namespace Tetris.Managers
{
    [SceneBind(SceneName.Main)]
    public class Preview : ViewManager<Preview>
    {
        [NotNull]
        Block currentBlock => TetrisManager.instance.currentBlock;

        public GameObject root;

        #region Block Preview

        [ButtonGroup("debug")]
        public void InitPreview()
        {
            Assert.IsNotNull(currentBlock, "currentBlock != null");
            // m_preview = Core.Instantiate(currentBlock).Of(t => t.setDynamicRoot("Preview"));
            //
            // foreach (Transform child in m_preview.transform) {
            //     var sr = child.GetComponent<SpriteRenderer>();
            //
            //     if (sr) {
            //         sr.color = new Color(1, 1, 1, .3f);
            //         sr.sortingOrder = 15;
            //     }
            // }
            //preview = GameManager.instance.PreviewRoot.GetComponent<Block>();
            var block = root.GetComponent<Block>();
            currentBlock.pieces.Where(t => t != null).ForEach((t, i) => {
                if (block.pieces[i] != null) {
                    block.pieces[i].localPosition = t.localPosition;
                    block.pieces[i].GetComponent<Cell>().m_OldState = -1;
                }
            });
            root.SetActive(true);
        }

        [ButtonGroup("debug")]
        public void UpdatePreview()
        {
            // if (!preview) {
            //     return;
            // }
            root.transform.localPosition = currentBlock.transform.localPosition;
            root.transform.rotation = currentBlock.transform.rotation;
            root.GetComponentInChildren<Block>().state = currentBlock.state;
            TetrisManager.instance.LandingPoint(root.GetComponent<Block>());
        }

        [ButtonGroup("debug")]
        public void DestroyPreview()
        {
            // if (preview != null) {
            root.SetActive(false);

            //Destroy(m_preview.gameObject);
            //}
        }

        #endregion
    }
}