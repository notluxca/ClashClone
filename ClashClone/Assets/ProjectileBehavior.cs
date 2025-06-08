using UnityEngine;

public class ProjectileBehavior : Damager
{
    public float speed = 10f;
    public float lifetime = 10f;

    private void Start()
    {
        // Destroy the bullet after 'lifetime' seconds if it doesn't hit anything
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the bullet forward every frame
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public override void ApplyDamage(IDamageable damageable, int amount)
    {
        base.ApplyDamage(damageable, amount);

        // Destroy the bullet if it hits an enemy
        Destroy(gameObject);
    }


}
