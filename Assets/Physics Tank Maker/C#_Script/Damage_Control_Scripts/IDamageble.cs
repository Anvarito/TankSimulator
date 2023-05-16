public interface IDamageble
{
    public float DamageTreshold { get; }
    public void DealDamage(float damage, int bulletType);
    public bool CheckBreackout(float damage, int bulletType);
}
