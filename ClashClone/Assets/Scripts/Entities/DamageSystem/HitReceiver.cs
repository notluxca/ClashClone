using UnityEngine;

public class HitReceiver : Damageable
{
    public override void TakeDamage(int amount)
    {

        // Debug.Log("Damage Detected");
        base.TakeDamage(amount);
    }

}
