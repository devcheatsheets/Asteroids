namespace Asteroids.Misc
{
    /// <summary>
    /// Interface that lets redefine functionality associated with different events
    /// </summary>
    public interface ISpawnable
    {
        void OnSpawned();
        void OnExplode();
    }
}