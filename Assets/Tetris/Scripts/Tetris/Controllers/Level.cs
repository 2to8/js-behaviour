using System.Linq;
using Battle;
using Consts;
using Data;
using GameEngine.Extensions;
using GameEngine.Kernel;
using Sirenix.OdinInspector;
using SQLiteNetExtensions.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris
{
    [SceneBind(SceneName.Main)]
    public class Level : ViewManager<Level>
    {
        public int blockId = 100;

        [Range(1, 20)]
        public int level = 1;

        public LevelData LevelData;

        [ButtonGroup("level")]
        public void _加载关卡()
        {
            var arena = instance;
            var levelData = LevelData.instance.FirstOrDefault(t => t.Id == level);
            //levelData.actors = ActorData.instance.FirstOrDefault(t => t.Id == this.level);
            arena.LevelData = levelData;
            Debug.Log(levelData.Id);
            var isEmpty = levelData.data.All(t => t.All(cell => cell == null));
            if (levelData.Id == 0 || isEmpty) {
                levelData.Id = level;
                levelData.data.Clear();
                //levelData.actors = DB.Table<ActorData>().FirstOrDefault(t => t.Id == this.level);
                Debug.Log("create".ToRed());
                Grid.instance.Clear();
                var grid = Grid.Create(levelData);
                for (var row = 0; row < grid.Count; row++) {
                    var line = new CellData[grid.width];
                    for (var col = 0; col < grid.width; col++)
                        line[col] = grid[row][col] == null
                            ? null
                            : new CellData() {
                                id = grid[row][col].id,
                                colorId = grid[row][col].colorId
                            };
                    levelData.data.Add(line);
                }

                levelData._保存表();

                //if (levelData.Id == 0) {
                //levelData.actors.Id = this.level;
                //Core.Connection.InsertOrReplace(levelData.actors);

//                Core.Connection.InsertOrReplace(levelData);
//                Core.Connection.UpdateWithChildren(levelData);

                //Core.Connection.InsertWithChildren(levelData);

                // }
                // else {
                //     Core.Connection.Update(levelData);
                // }

                //DB.Insert(levelData);
            }
            else {
                Debug.Log($"count: {levelData.data.Count}");
                Grid.Create(levelData);
            }

            //SceneManager.GetActiveScene().SetDirty();
        }
    }
}