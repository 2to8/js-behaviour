using Tetris;
using Tetris.Managers;

namespace Common
{
    public partial class View
    {
        protected static TetrisManager tetris => TetrisManager.instance;
        protected static Grid grid => Grid.instance;
        protected static Map map => Map.instance;
    }
}