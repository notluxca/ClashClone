using UnityEngine;

public class HitReceiver : Damageable
{
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        // Debug.Log("Damage Detected");
    }

}
