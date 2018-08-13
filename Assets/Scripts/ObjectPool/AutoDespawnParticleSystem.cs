public class AutoDespawnParticleSystem : AutoDestroyParticleSystem
{
    public Reusable Reusable;

    protected override void Start()
    {
        base.Start();
        if (Reusable == null)
            Reusable = GetComponent<Reusable>();
    }

    protected override void DestroyParticleSystem()
    {
        if (Reusable != null)
        {
            SimplePool.Despawn(Reusable);
        }
        else
        {
            if (transform.parent != null)
            {
                if (DestroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}