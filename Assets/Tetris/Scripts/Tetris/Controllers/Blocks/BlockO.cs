namespace Tetris.Blocks
{
    public class BlockO : Block
    {
        public override bool AntiClockwiseRotation()
        {
            return false;
        }

        public override bool ClockwiseRotation()
        {
            return false;
        }
    }
}