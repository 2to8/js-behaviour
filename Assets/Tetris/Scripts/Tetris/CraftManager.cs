using GameEngine.Attributes;
using GameEngine.Extensions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Consts;
using Tetris;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Scenes;

namespace UnityRoyale
{
    [SceneBind(SceneName.Main)]
    public class CraftManager : Manager<CraftManager>
    {
        public CraftData data;

        [FormerlySerializedAs("panel")]
        public GameObject SkillPanel;

        [SerializeField]
        LinkedList<CraftItemData> crafts = new LinkedList<CraftItemData>();

        [Button]
        void SetIconPosition()
        {
            data.IconPostion.Clear();
            SkillPanel!.Childs().ForEach(t => {
                data.IconPostion.Add(new RectTransformData(t.GetComponent<RectTransform>()));
            });
        }

        [ButtonGroup("gen")]
        void Gen0() { }

        [ButtonGroup("gen")]
        void Gen1() { }

        [ButtonGroup("gen")]
        void Gen2() { }

        [ButtonGroup("gen")]
        void Gen3() { }
    }
}