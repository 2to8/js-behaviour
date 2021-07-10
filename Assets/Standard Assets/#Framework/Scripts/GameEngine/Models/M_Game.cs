using GameEngine.Kernel;
using GameEngine.Kernel.Consts;
using UnityEngine;

namespace GameEngine.Models {

public class M_Game : Model<M_Game> {

    int m_Level;

    public int Level {
        get => m_Level;
        set {
            value = Mathf.Clamp(value, 0, 100);

            if (value == m_Level) {
                return;
            }

            m_Level = value;
            SendEvent<E_LevelChange>(new E_LevelChange { level = m_Level });
        }
    }

    // public override string Name
    // {
    //     get{  return Consts.M_GameModel;  }
    // }

}

}