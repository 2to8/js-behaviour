using System.Linq;
using Battle;
using GameEngine.Extensions;
using GameEngine.Kernel;
using GameEngine.Models.Contracts;
using Sirenix.OdinInspector;
using Tetris;
using Tetris.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityRoyale;
using Grid = Tetris.Grid;
using LevelData = Data.LevelData;

namespace Admin
{
    [CreateAssetMenu(fileName = "GameStatus", menuName = "Assets/GameStatus", order = 0)]
    public class Status : DbTable<Status>
    {
        public TetrisManager tetris => TetrisManager.instance;

        //[ShowInInspector]
        public Game.GameState State {
            get => Game.instance.State;
            set => Game.instance.State = value;
        }

        // [ShowInInspector]
        int GridCount => Grid.instance.Count;

        [ShowInInspector]
        int bottom {
            get => Map.instance.bottom;
            set => Map.instance.bottom = value;
        }

        [ShowInInspector]
        Vector3Int pos {
            get => Game.instance?.MovableRoot != null
                    ? Vector3Int.RoundToInt(Game.instance.MovableRoot.localPosition)
                    : Vector3Int.zero;
            set => Game.instance.MovableRoot.localPosition = value;
        }

        [PropertyRange(1, 20)]
        [ShowInInspector]
        public int level {
            get => Level.instance.level;
            set => Level.instance.level = value;
        }

        bool debugDB = false;

        [ButtonGroup("level")]
        [ShowIf(nameof(debugDB))]
        void _测试数据库()
        {
            var t = LevelData.instance.FirstOrDefault();
            DB.Insert(t);
        }

        [ButtonGroup("level")]
        void _角色数据()
        {
            var levelData = LevelData.instance;
            levelData.actors[(0, 1)] = new TheActor() {
                actorId = 1
            };
            levelData._保存表();
        }

        [ButtonGroup("level")]
        void _清除关卡()
        {
            DB.Table<LevelData>().Delete(t => t.Id == level);
        }

        [ButtonGroup("level")]
        void _加载关卡()
        {
            Level.instance._加载关卡();
            _新卡牌();
        }

        [ButtonGroup("debug")]
        void _生成预览()
        {
            //
            Preview.instance.InitPreview();
            Preview.instance.UpdatePreview();
        }

        // [ButtonGroup("debug")]
        // void _生成地图() { }
        //
        // [ButtonGroup("debug"), GUIColor(0 / 255f, 255 / 255f, 240 / 255f)]
        // void _重新开局() { }

        [ButtonGroup("debug")]
        [GUIColor(0 / 255f, 255 / 255f, 240 / 255f)]
        void _复位()
        {
            tetris.landed = false;
            tetris.HardDrop(-1);
        }

        [ButtonGroup("control")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        void _左()
        {
            tetris.landed = false;
            tetris.MoveLeft();
        }

        [ButtonGroup("control")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        void _下落()
        {
            tetris.landed = false;
            tetris.HardDrop();
            tetris.Fall(0);
        }

        [ButtonGroup("control")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        void _右()
        {
            tetris.landed = false;
            tetris.MoveRight();
        }

        [ButtonGroup("control")]
        [Button(ButtonSizes.Large)]
        [GUIColor(255 / 255f, 0 / 255f, 240 / 255f)]
        void _旋转()
        {
            tetris.ClockwiseRotation();
        }

        [ButtonGroup("control")]
        [Button(ButtonSizes.Large)]
        [GUIColor(1, 0, 0)]
        void _1()
        {
            CardManager.instance.HoldCard(0);
        }

        [ButtonGroup("control")]
        [Button(ButtonSizes.Large)]
        [GUIColor(1, 0, 0)]
        void _2()
        {
            CardManager.instance.HoldCard(1);
        }

        [ButtonGroup("control")]
        [Button(ButtonSizes.Large)]
        [GUIColor(1, 0, 0)]
        void _3()
        {
            CardManager.instance.HoldCard(2);
        }

        [ButtonGroup("card")]
        void _新卡牌()
        {
            GameManager.instance.Start();
        }

        [ButtonGroup("card")]
        void _下张牌()
        {
            CardManager.instance.LoadDeck(false);
        }
    }
}