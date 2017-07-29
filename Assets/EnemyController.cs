using Assets.Scripts;

public abstract class EnemyController : BaseMover
{
    public void Kill()
    {
        Destroy(gameObject);
    }
}