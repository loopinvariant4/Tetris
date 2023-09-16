using Microsoft.Xna.Framework.Graphics;
using TetrisGame;
using TetrisGame.TetrisEngine;

namespace TestTetris
{
    [TestClass]
    public class TestBoardMatrix
    {
        const int COLS = 10;
        const int ROWS = 20;

        [TestMethod]
        public void TestBottomRowClear()
        {
            BoardMatrix bm = new BoardMatrix(new Dictionary<Shape, Texture2D> { { Shape.O, null } });
            bm.Level = 10;
            Assert.AreEqual(State.Generation, bm.BoardState);
            bm.UpdateBoard();
            Assert.AreEqual(State.Falling, bm.BoardState);
            for (int i = 0; i < ROWS; i++)
            {
                bm.UpdateBoard();
                bm.UpdateBoard();
                bm.UpdateBoard();
                bm.UpdateBoard();
            }
            Assert.AreEqual(State.Locking, bm.BoardState);
        }
    }
}