using Common.JSRuntime;
using DG.Tweening;
using MainScene.BootScene.Utils;
using MoreTags;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainScene.Menu
{
    public class LevelCard : Manager<LevelCard>
    {
        [SerializeField]
        public bool isAvatar;

        bool m_Inited;
        public RectTransform parent => transform.parent.GetComponent<RectTransform>();

        //public Button button => GetComponent<Button>();
        static Vector2 top;
        static Dictionary<int, LevelCard> m_Cards = new Dictionary<int, LevelCard>();
        static bool inited => instance?.m_Inited == true;
        Slider m_Progress;

        static Slider progress {
            get => instance?.m_Progress;
            set => instance.m_Progress = value;
        }

        void Update()
        {
            if (transform.GetSiblingIndex() != 0) return;
            if (loadHandle.IsValid() && progress != null) progress.value = loadHandle.PercentComplete;
        }

        public override void Start()
        {
            base.Start();
            //button.onClick.AddListener(OnClick);
            if (!inited) {
                parent.anchoredPosition = new Vector2(-15f, 164.5f);
                instance.m_Inited = true;
                progress = TagSystem.Find<Slider>("Id.LevelLoad");
                progress?.gameObject.SetActive(false);
                top = GetComponent<RectTransform>().anchoredPosition;
                transform.parent.GetComponentsInChildren<LevelCard>(true).ForEach(t => {
                    m_Cards[t.transform.GetSiblingIndex()] = t;
                    t.gameObject.SetActive(!t.isAvatar);
                });
            }
        }

        public void OnClick()
        {
            if (!isAvatar) {
                Debug.Log(gameObject.name);
                parent.DOAnchorPos(new Vector2(-15f, -10f), .2f).SetEase(Ease.OutQuad).OnComplete(() => {
                    m_Cards.Values.ForEach(t => t.gameObject.SetActive(t.isAvatar));
                    parent.DOAnchorPos(new Vector2(-15f, 164.5f), .2f).SetEase(Ease.OutQuad).OnComplete(() => { });
                });
            }
            else {
                parent.DOAnchorPos(new Vector2(-15f, -10f), .2f).SetEase(Ease.OutQuad).OnComplete(() => {
                    progress = TagSystem.Find<Slider>("Id.LevelLoad");
                    if (progress != null) {
                        progress.gameObject.SetActive(true);
                        progress.value = 0f;
                    }

                    loadHandle = Addressables.LoadSceneAsync("Main", LoadSceneMode.Single, false);
                    loadHandle.Completed += handle => {
                        progress?.gameObject.SetActive(false);
                        m_Cards.Values.ForEach(t => t.gameObject.SetActive(!t.isAvatar));

                        //parent.anchoredPosition = new Vector2(-15f, 164.5f);
                        handle.Result.ActivateAsync().completed += operation => {
                            handle.Result.Scene.GetRootGameObjects().ForEach(go => go.CheckTagsFromRoot());
                            Addressables.ResourceManager.Acquire(handle);
                            Addressables.Release(handle);
                        };
                    };
                });
            }
        }

        public static AsyncOperationHandle<SceneInstance> loadHandle { get; set; }
    }
}