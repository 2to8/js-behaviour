using System.Collections;
using System.Collections.Generic;
using Consts;
using MoreTags.Attributes;
using Tetris;
using UnityEngine;

namespace UnityRoyale
{
    [SceneBind(SceneName.Main), ExecuteAlways]
    public class UIManager : ViewManager<UIManager>
    {
        public GameObject healthBarPrefab;

        [Tags(Id.GameOverUI)]
        public GameObject gameOverUI;

        private List<HealthBar> healthBars;

        [SerializeField, Tags(Id.HealthBarContainer)]
        private Transform healthBarContainer;

        protected override void Awake()
        {
            base.Awake();
            healthBars = new List<HealthBar>();
            healthBarContainer ??= (GameObject.Find("/HealthBarContainer") ?? new GameObject("HealthBarContainer"))
                .transform;
        }

        public void AddHealthUI(ThinkingPlaceable p)
        {
            GameObject newUIObject = Instantiate<GameObject>(healthBarPrefab, p.transform.position, Quaternion.identity,
                healthBarContainer);
            p.healthBar = newUIObject.GetComponent<HealthBar>(); //store the reference in the ThinkingPlaceable itself
            p.healthBar.Initialise(p);
            healthBars.Add(p.healthBar);
        }

        public void RemoveHealthUI(ThinkingPlaceable p)
        {
            healthBars.Remove(p.healthBar);
            Destroy(p.healthBar.gameObject);
        }

        public void ShowGameOverUI()
        {
            gameOverUI.SetActive(true);
        }

        public void OnRetryButton()
        {
            Core.ReloadScene();
//            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
//                .name);
        }

        private void LateUpdate()
        {
            for (int i = 0; i < healthBars.Count; i++) {
                healthBars[i].Move();
            }
        }
    }
}