using GameEngine.Kernel.Args;
using GameEngine.Models.Contracts;
using UnityEngine;

namespace GameEngine.Models {

public class TestGameTable : DbTable<TestGameTable> {

    int m_Level;

    public int Level {
        get => m_Level;
        set {
            value = Mathf.Clamp(value, 0, 100);

            if (value == m_Level) {
                return;
            }

            m_Level = value;
            var e = new LevelArgs { level = m_Level };

            // SendEvent(Consts.E_LevelChange, e);
        }
    }

    // public override object TableName => Consts.M_GameModel;

}

}