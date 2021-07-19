using MoreTags;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MainScene.Menu
{
    public class LevelManager : SerializedMonoBehaviour
    {
        public void GoButtonClick()
        {
            TagSystem.Find<Transform>("Id.Stages")?.gameObject.SetActive(false);
            TagSystem.Find<Transform>("Id.Choice")?.gameObject.SetActive(true);
        }
    }
}