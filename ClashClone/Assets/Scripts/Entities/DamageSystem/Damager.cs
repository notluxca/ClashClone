using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour
{

    EntityController entityController;
    [SerializeField] private LayerMask enemyLayerMask;

    void Awake()
    {
        entityController = GetComponentInParent<EntityController>();
    }

    void Start()
    {
        enemyLayerMask = entityController.targetingController.enemyLayerMask;
    }


    public virtual void ApplyDamage(IDamageable damageable, int amount)
    {
        damageable.TakeDamage(amount);
    }

    void OnTriggerEnter(Collider other)
    {
        
        // Debug.Log("Collision on layer");
        // filter colliders that are not enemies
        if (!((enemyLayerMask.value & (1 << other.gameObject.layer)) != 0)) return;
        Damageable damageable = other.GetComponentInChildren<Damageable>();
        if (damageable != null)
        {
            ApplyDamage(damageable, 1);
        }
    }
}
