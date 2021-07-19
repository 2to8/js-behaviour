using System;
using System.Collections.Generic;
using Battle;
using GameEngine.Models.Contracts;
using MoreTags;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using SQLite.Attributes;
using SQLiteNetExtensions.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(menuName = "Tetris/" + nameof(LevelData))]
    public partial class LevelData : DbTable<LevelData>
    {
        [OdinSerialize]
        public List<CellData[]> data { get; set; } = new List<CellData[]>();

        [SerializeField]
        [HideInInspector]
        int m_Money;

        // [OdinSerialize]
        // public ActorData actors { get; set; }

        [OdinSerialize]
        Dictionary<(int x, int y), TheActor> m_Actors = new Dictionary<(int x, int y), TheActor>();

        [OdinSerialize /* TextBlob(nameof(m_ActorsBlob))*/]
        public Dictionary<(int x, int y), TheActor> actors {
            get => m_Actors ??= new Dictionary<(int x, int y), TheActor>();
            set => m_Actors = value;
        }

        [OdinSerialize]
        public string m_ActorsBlob { get; set; }

        [ShowInInspector]
        [PropertyRange(0, 10000)]
        public int money {
            get => m_Money;
            set => OnChange.Invoke(nameof(money), m_Money = value);
        }

        [FormerlySerializedAs("m_Credit")]
        [SerializeField]
        [HideInInspector]
        int m_Score;

        [ShowInInspector]
        [PropertyRange(0, 10000)]
        public int score {
            get => m_Score;
            set => OnChange.Invoke(nameof(score), m_Score = value);
        }

        [SerializeField]
        [HideInInspector]
        int m_Lines;

        [ShowInInspector]
        [PropertyRange(0, 100)]
        public int lines {
            get => m_Lines;
            set => OnChange.Invoke(nameof(lines), m_Lines = value);
        }

        [SerializeField]
        [HideInInspector]
        int m_Rescue;

        [ShowInInspector]
        [PropertyRange(0, 100)]
        public int rescue {
            get => m_Rescue;
            set => OnChange.Invoke(nameof(rescue), m_Rescue = value);
        }

        [SerializeField]
        [HideInInspector]
        int m_Mission;

        [ShowInInspector]
        [PropertyRange(0, 100)]
        [PropertyOrder(-1)]
        public int mission {
            get => m_Mission;
            set => OnChange.Invoke(nameof(mission), m_Mission = value);
        }

        [ShowInInspector]
        [ProgressBar(0, 100)]
        [HideLabel]
        [PropertyOrder(-2)]
        int missionBar => mission;

        [SerializeField]
        [HideInInspector]
        int m_Health;

        [ShowInInspector]
        [PropertyRange(0, 100)]
        [PropertyOrder(-3)]
        public int health {
            get => m_Health;
            set => OnChange.Invoke(nameof(health), m_Health = value);
        }

        [ShowInInspector]
        [ProgressBar(0, 100)]
        [HideLabel]
        [PropertyOrder(-4)]
        int hpBar => health;

        Action<string, object> m_OnChange;

        [Ignore]
        public Action<string, object> OnChange {
            get => m_OnChange ??= TagSystem.DoChange;
            set => m_OnChange = value;
        }

        [SerializeField]
        bool isDisplayDebug {
            get => TagSystem.isDisplayDebug;
            set => TagSystem.isDisplayDebug = value;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GetType().GetProperties().ForEach(info => {
                if (info.CanWrite) info.SetValue(this, info.GetValue(this));
            });
        }
    }
}