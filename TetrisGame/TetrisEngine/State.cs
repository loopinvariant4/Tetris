namespace TetrisGame.TetrisEngine
{
    internal enum State
    {
        Generation,
        Falling,
        Locking,
        CheckPattern,
        MarkDestruction,
        Animate,
        Eliminate,
        Completion

    }
}
