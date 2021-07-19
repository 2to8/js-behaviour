using Consts;
using Sirenix.OdinInspector;

namespace Tetris.Managers
{
    [SceneBind(SceneName.Main)]
    public class GridManager : ViewManager<GridManager>
    {
        [ShowInInspector]
        [ListDrawerSettings(ShowIndexLabels = true, Expanded = true)]
        public Grid data => Grid.instance;
    }
}