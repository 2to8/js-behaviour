using GameEngine.Attributes;
using GameEngine.Extensions;
// using Google.Protobuf.Reflection;
using Puerts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Consts;
using MoreTags.Attributes;
using Puerts.Attributes;
using TeamDev.Redis;
using Tetris.Blocks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityRoyale;
using Object = UnityEngine.Object;

namespace Tetris
{
    //[ExecuteAlways]
    [SceneBind(SceneName.Main)]
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    public class Game : ViewManager<Game>
    {
        //static Game m_Instance;

        //public Tetris config;
        //const string SceneName = "Gaming";

        // public static Game instance => m_Instance ??= Core.FindOrCreateManager<Game>(SceneName);

        //   set => m_Instance = value;
        public float StartTime {
            get => startTime;
            set => startTime = value;
        }

        [HideInInspector]
        GameState m_State = GameState.Pause;

        [ShowInInspector]
        public GameState State {
            get => m_State;
            set => m_State = value;
        }

        public int level = 3;

        [SerializeField]
        public Block[] blocks;

        [SerializeField, Tags(Id.CurrentBlockRoot)]
        public Transform CurrentBlockRoot;

        [Title("Roots")]
        [SerializeField, Tags(Id.PreviewRoot)]
        public Transform PreviewRoot;

        [SerializeField, Tags(Id.MovableRoot)]
        public Transform MovableRoot;

        [Tags(Id.ActorRoot)]
        public GameObject ActorRoot;

        [SerializeField]
        Transform[] previeField = new Transform[4];

        // [OdinSerialize,DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        // public Dictionary<CellType, Transform> previewField = new Dictionary<CellType, Transform>() {
        //     [CellType.Stone1] = null,
        //     [CellType.Stone2] = null,
        //     [CellType.Wood1] = null,
        //     [CellType.Wood2] = null,
        // };

        // [SerializeField]
        // LineRenderer linePrefab;

        [FoldoutGroup("events")]
        public UnityEvent OnPauseCallback;

        [FoldoutGroup("events")]
        public UnityEvent OnGammingCallback;

        [SerializeField]
        TetrisManager m_tetris;

        Block defaultBlock => CurrentBlockRoot.GetComponentInChildren<Block>();
        public static TetrisManager tetris => instance.m_tetris;
        static Block m_CurrentBlock;

        public static Block currentBlock {
            get {
#if UNITY_EDITOR
                return m_CurrentBlock ??= instance.defaultBlock;
#endif
                return m_CurrentBlock;
            }
            set => m_CurrentBlock = value;
        }

        [SerializeField]
        BlockSpawner m_Spawner;

        public static BlockSpawner spawner {
            get => instance.m_Spawner;
            set => instance.m_Spawner = value;
        }

        [SerializeField]
        VFX m_VFX;

        // [SerializeField]
        // Transform m_HoldParent;
        //
        // [SerializeField]
        // public Transform m_NextParent;

        //public static Transform holdParent => instance.m_HoldParent;
        const float offset = .5f;
        bool left_pressed;
        bool right_pressed;
        bool up_pressed;
        float startTime = .15f;
        float lastStartTime;
        float inputDelta = .03f;
        float lastInputTime;

        public enum GameState
        {
            Pause = 0,
            Gamming
        }

        public bool GameStarted { get; set; }
        float timerStart;
        public UnityEvent OnFirstBlock = new UnityEvent();
        public bool IsDefaultGameStart = true;

        public void DoGameStart()
        {
            State = GameState.Gamming;
            Debug.Log("[StartGame]".ToYellow());
        }

        void Start()
        {
            // if (IsDefaultGameStart) {
            //     OnFirstBlock.AddListener(DoGameStart);
            // }
            // if (!Application.isPlaying) {
            //     Management();
            //
            //     return;
            // }

            //m_VFX = GameObject.Find("VFX & SFX").GetComponent<VFX>();
            // Tetirs Constructor

            // if (linePrefab != null) {
            //     // draw row line
            //     for (var i = 0; i <= TetrisManager.Height + TetrisManager.ExtraHeight - 1; i++) {
            //         var row = Core.Instantiate(linePrefab, transform).Of(t => t.setDynamicRoot("Line"));
            //         row.positionCount = 2;
            //         row.SetPosition(0, new Vector3(-offset, i - offset));
            //         row.SetPosition(1, new Vector3(TetrisManager.Width - offset, i - offset));
            //     }
            //
            //     // draw col line
            //     for (var i = 0; i <= TetrisManager.Width; i++) {
            //         var col = Core.Instantiate(linePrefab, transform).Of(t => t.setDynamicRoot("Line"));
            //         col.positionCount = 2;
            //         col.SetPosition(0, new Vector3(i - offset, -offset));
            //         col.SetPosition(1,
            //             new Vector3(i - offset, TetrisManager.Height + TetrisManager.ExtraHeight - 1 - offset));
            //     }
            // }

            //            holdParent.position = m_tetris.holdPlace;

            //state = GameState.Pause;
            spawner?.Init();

            //StartGame();

            //StartCoroutine(checkBool());
        }

        // bool myBool;
        //
        // IEnumerator checkBool()
        // {
        //     while (true) {
        //         yield return new WaitForSeconds(1f);
        //         myBool = !myBool;
        //     }
        // }

        void Update()
        {
            // if (!Application.isPlaying) {
            //     return;
            // }
            if (!GameManager.instance.HudUI.enabled) return;
            if (m_State == GameState.Gamming) Debug.Log($"Gaming {currentBlock != null}");
            if (m_State == GameState.Gamming && currentBlock != null) {
                if (!GameStarted) {
                    timerStart += Time.deltaTime;
                    if (timerStart >= 2f)
                        GameStarted = true;
                    else
                        return;
                }

                ProcessBlockInput();

                // CheckHoldPos();
                //
                // if (m_tetris.UpdateNextPost) {
                //     CheckNextPos();
                // }

                // auto drop
                m_tetris.Fall(Time.deltaTime);
            }
        }

        // void CheckHoldPos()
        // {
        //     if (holdParent != null) {
        //         var pos = holdParent.position;
        //         var a = (false, false);
        //
        //         if (a.Item1 = Math.Abs(pos.x - m_tetris.holdedViewPosX) > float.Epsilon) {
        //             m_tetris.holdedViewPosX = pos.x;
        //         }
        //
        //         if (a.Item2 = Math.Abs(pos.z - m_tetris.holdedViewPosY) > float.Epsilon) {
        //             m_tetris.holdedViewPosY = pos.z;
        //         }
        //
        //         if (a.Item1 || a.Item2) {
        //             m_tetris.SetHold();
        //             m_tetris.holdPlace = holdParent.position;
        //
        //             // #if UNITY_EDITOR
        //             //     EditorUtility.SetDirty(m_tetris);
        //             //     AssetDatabase.SaveAssets();
        //             // #endif
        //         }
        //     }
        // }

        // void CheckNextPos()
        // {
        //     if (m_NextParent != null) {
        //         var pos = m_NextParent.position;
        //
        //         if (Math.Abs(pos.x - m_Spawner.startPosX) > float.Epsilon
        //             || Math.Abs(pos.y - m_Spawner.startPosY) > float.Epsilon) {
        //             m_Spawner.SetSlotPos();
        //             m_tetris.nextPos = m_NextParent.position;
        //         }
        //     }
        // }

        // [ButtonGroup("game")]
        // public void Management()
        // {
        //     m_NextParent.position = m_tetris.nextPos;
        //     holdParent.position = m_tetris.holdPlace;
        // }

        void ProcessBlockInput()
        {
            // hard & soft drop
            // 下移
            if (m_tetris.k_HardDrop.IsKeyDown()) {
                m_tetris.HardDrop(); // execute in one frame
            }
            else if (!up_pressed && m_tetris.k_Down.IsKeyDown()) {
                up_pressed = true;
                m_tetris.SoftDrop();
            }
            else if (up_pressed && m_tetris.k_Down.IsKeyUp()) {
                up_pressed = false;
                m_tetris.NormalDrop();
            }

            // move left
            // 左移
            if (m_tetris.k_Left.IsKeyDown()) {
                m_tetris.MoveLeft();
                left_pressed = true;
            }

            if (!right_pressed && left_pressed && m_tetris.k_Left.IsKey()) {
                if (lastStartTime >= startTime) {
                    if (lastInputTime >= inputDelta) {
                        m_tetris.MoveLeft();
                        lastInputTime = 0;
                    }
                    else {
                        lastInputTime += Time.deltaTime;
                    }
                }
                else {
                    lastStartTime += Time.deltaTime;
                }
            }

            if (left_pressed && m_tetris.k_Left.IsKeyUp()) {
                left_pressed = false;
                lastStartTime = 0;
                lastInputTime = 0;
            }

            // move right
            // 右移
            if (m_tetris.k_Right.IsKeyDown()) {
                m_tetris.MoveRight();
                right_pressed = true;
            }

            if (!left_pressed && right_pressed && m_tetris.k_Right.IsKey()) {
                if (lastStartTime >= startTime) {
                    if (lastInputTime >= inputDelta) {
                        m_tetris.MoveRight();
                        lastInputTime = 0;
                    }
                    else {
                        lastInputTime += Time.deltaTime;
                    }
                }
                else {
                    lastStartTime += Time.deltaTime;
                }
            }

            if (right_pressed && m_tetris.k_Right.IsKeyUp()) {
                right_pressed = false;
                lastStartTime = 0;
                lastInputTime = 0;
            }

            // rotate
            // 左转
            if (m_tetris.k_AntiClockwiseRotation.IsKeyDown()) m_tetris.AntiClockwiseRotation();

            // 右转
            if (m_tetris.k_ClockwiseRotation.IsKeyDown()) m_tetris.ClockwiseRotation();

            // hold
            // 备用
            if (m_tetris.k_Hold.IsKeyDown()) {
                //m_tetris.HoldBlock();
            }

            // logger
            //if (Input.GetKeyDown(KeyCode.Return))
            //{
            //    Tetris.logger.Print();
            //}
        }

        [ButtonGroup("game")]
        public void PauseGame()
        {
            m_State = GameState.Pause;
            OnPauseCallback?.Invoke();
        }

        [ButtonGroup("game")]
        public void StartGame()
        {
            StartCoroutine(Gamming());
        }

        [ButtonGroup("rotate")]
        public void testRotate()
        {
            var minZ = 0;

            //currentBlock.direction = (currentBlock.direction + 1) % 4;
            var oldMinZ = 0;
            currentBlock?.pieces.ForEach(t => {
                if (t.Z(false, 0) < oldMinZ) oldMinZ = t.Z(false, 0);
                t.localPosition = new Vector3(t.Z(false, 0), 0, -t.X(false, 0));
                if (t.Z(false, 0) < minZ) minZ = t.Z(false, 0);
            });
            if (Mathf.Abs(minZ - oldMinZ) > 0) currentBlock.ZAdd(oldMinZ - minZ);
        }

        IEnumerator Gamming()
        {
            if (Application.isPlaying) yield return new WaitForSeconds(1f);
            OnGammingCallback?.Invoke();
            m_VFX?.TextVFX_Start();
            if (Application.isPlaying) yield return new WaitForSeconds(5f);
            m_State = GameState.Gamming;
            if (m_VFX)
                m_tetris.Init(m_Spawner.Create(blocks), m_VFX);
            else
                m_tetris.Init(m_Spawner.Create(blocks));

            //m_tetris.NextBlock();
            m_VFX?.PlayBG(Consts.BGMainTheme);
        }

        public void QuitGame()
        {
            Core.Quit();
        }

        // protected override void Awake()
        // {
        //     base.Awake();
        // }
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (m_tetris != null && Application.isPlaying) m_tetris.DrawGizmos();
        }
#endif
    }
}