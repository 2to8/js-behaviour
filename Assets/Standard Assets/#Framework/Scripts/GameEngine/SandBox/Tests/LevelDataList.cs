using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.SandBox.Tests {

[CreateAssetMenu(fileName = "LevelUp", menuName = "LevelUp", order = 0)]
public class LevelDataList : SerializedScriptableObject {

    [ShowInInspector]
    bool down;

    [TableList(ShowIndexLabels = true), LabelText("装备强化")]
    public List<LevelData> LevelDatas = new List<LevelData>();

    float totalFee;
    int totalTimes;

    public static int choice(float[] probs)
    {
        float total = 0;

        foreach (var elem in probs) {
            total += elem;
        }

        var randomPoint = Random.value * total;
        Debug.Log(randomPoint);

        for (var i = 0; i < probs.Length; i++) {
            if (randomPoint < probs[i]) {
                return i;
            }

            randomPoint -= probs[i];
        }

        return probs.Length - 1;
    }

    [Button("测试强化次数")]
    void TestRoll()
    {
        totalFee = 0;
        totalTimes = 0;

        for (var i = 0; i < LevelDatas.Count; i++) {
            var t = i;

            while (true) {
                totalFee += LevelDatas[t].Cost;
                totalTimes += 1;

                //choice(new[] {LevelDatas[t].Rate, 1 - LevelDatas[t].Rate}) == 0
                if (Random.value < LevelDatas[t].Rate) {
                    if (t == i) {
                        LevelDatas[t].TotalCast = totalFee;
                        LevelDatas[t].Times = totalTimes;

                        break;
                    }

                    if (down) {
                        t += 1;
                    }
                } else if (down && t > 0) {
                    t -= 1;
                }

                if (totalTimes >= 100000000) {
                    Debug.Log("times out");

                    return;
                }
            }
        }

        Debug.Log("finish");
    }

}

}