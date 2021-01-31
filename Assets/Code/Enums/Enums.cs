namespace Asteroids
{
    public enum GameState
    {
        PreGame,
        Game,
        Pause,
        GameOver
    }

    public enum Axis
    {
        X,
        Y
    }

    public enum LogLevel
    {
        OnlyErrors,
        ErrorsAndWarnings,
        All,
        None
    }

    public enum TargetMode
    {
        CursorPosition,
        Custom,
        Player
    }

    public enum ShootMode
    {
        Automatic,
        InputBased
    }

    public enum BordersBehaviour
    {
        Disable,
        MoveToOppositePoint
    }
    
    public enum MeshType
    {
        Triangle,
        Quad,
        Circle
    }

    public enum MotionMode
    {
        Automatic,
        InputBased,
        None
    }
}