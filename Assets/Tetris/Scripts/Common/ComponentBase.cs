using Tetris;
using Tetris.Managers;

namespace Common
{
    public partial class ComponentBase
    {
        protected static TetrisManager tetris => TetrisManager.instance;
        protected static Grid grid => Grid.instance;
        protected static Map map => Map.instance;
    }
}