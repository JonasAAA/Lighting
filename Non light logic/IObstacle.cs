namespace Game1
{
    public interface IObstacle : IShadowCastingObject
    {
        void Collide(DiskPlayer player);
    }
}
