using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour
{

    EntityController entityController;
    LayerMask enemyLayerMask;

    void Awake()
    {
        entityController = GetComponentInParent<EntityController>();
    }

    void Start()
    {
        enemyLayerMask = entityController.targetingController.enemyLayerMask;
    }


    public void ApplyDamage(IDamageable damageable, int amount)
    {
        damageable.TakeDamage(amount);
    }

    void OnTriggerEnter(Collider other)
    {
        // filter colliders that are not enemies
        if (!((enemyLayerMask.value & (1 << other.gameObject.layer)) != 0)) return;

        Debug.Log("Detectei algo para dar dano");

        Damageable damageable = other.GetComponentInChildren<Damageable>();
        if (damageable != null)
        {
            ApplyDamage(damageable, 1);
        }
    }
}
