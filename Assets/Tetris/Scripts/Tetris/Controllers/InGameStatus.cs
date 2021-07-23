using Common;
using Tetris.Managers;
using TMPro;
using UnityEngine;

namespace Tetris
{
    public class InGameStatus : View<InGameStatus>
    {
        public TMP_Text score;
        public TMP_Text level;
        public TMP_Text goal;
        public TMP_Text time;
        public GameObject gameOverPanel;

        protected override void OnEnable()
        {
            base.OnEnable();
            TetrisManager.OnScoreChanged += UpdateScore;
            TetrisManager.OnLevelChanged += UpdateLevel;
            TetrisManager.OnGoalChanged += UpdateGoal;
            TetrisManager.OnTimeChanged += UpdateTime;
            TetrisManager.OnGameOver += OnGameOver;
            gameOverPanel.SetActive(false);
        }

        void OnDisable()
        {
            TetrisManager.OnScoreChanged -= UpdateScore;
            TetrisManager.OnLevelChanged -= UpdateLevel;
            TetrisManager.OnGoalChanged -= UpdateGoal;
            TetrisManager.OnTimeChanged -= UpdateTime;
            TetrisManager.OnGameOver -= OnGameOver;
        }

        int m, s, ms;

        void UpdateTime(float val)
        {
            m = (int) val / 60;
            s = (int) val % 60;
            ms = (int) (val * 1000) % 1000 / 10;
            time.text = $"{m:00}:{s:00}.{ms:00}";
        }

        void UpdateGoal(int val)
        {
            goal.text = val.ToString();
        }

        void OnGameOver()
        {
            gameOverPanel.SetActive(true);
        }

        void UpdateLevel(int val)
        {
            level.text = val.ToString();
        }

        void UpdateScore(int val)
        {
            score.text = val.ToString();
        }
    }
}