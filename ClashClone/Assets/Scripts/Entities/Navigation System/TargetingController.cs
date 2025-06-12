using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(NavigationController))]
public class TargetingController : MonoBehaviour
{
    [Header("Configurações de Alvo")]
    public LayerMask enemyLayerMask;
    public Transform enemyBaseTarget;

    [Header("Configurações de Detecção")]
    public float targetEvaluationFrequency = 0.2f;
    public SphereCollider detectionCollider;

    [Header("Evento")]
    public UnityEvent onReachEnemyBase; // <- Evento chamado ao chegar na base

    [Header("Chegada")]
    public float baseArrivalThreshold = 9f; // Distância considerada como "chegou"

    private EntityController entityController;
    private NavigationController navigationController;
    private Transform currentEntityTarget;
    public bool hasReachedBase = false; // Para evitar chamadas múltiplas

    void Awake()
    {
        navigationController = GetComponent<NavigationController>();
        entityController = GetComponent<EntityController>();
        detectionCollider.isTrigger = true;
    }

    void Start()
    {
        StartCoroutine(EvaluateTargetsCoroutine());
    }

    void Update()
    {
        // Debug.Log(Vector3.Distance(transform.position, enemyBaseTarget.position));
        CheckArrivalAtBase();
    }

    private void CheckArrivalAtBase()
    {
        if (enemyBaseTarget == null ) return;
            float distance = Vector3.Distance(transform.position, enemyBaseTarget.position);
            if (distance <= baseArrivalThreshold && !hasReachedBase)
            {
                hasReachedBase = true;
                onReachEnemyBase?.Invoke(); // Dispara o evento
            }
        
    }

    private IEnumerator EvaluateTargetsCoroutine()
    {
        while (true)
        {
            if (navigationController.thisAgent.enabled == false) yield break;
            FindClosestEnemy();

            if (currentEntityTarget != null)
            {
                if (navigationController.target != currentEntityTarget)
                {
                    navigationController.SetTarget(currentEntityTarget);
                    hasReachedBase = false; // Reset se mudar de alvo
                }
            }
            else
            {
                if (navigationController.target != enemyBaseTarget)
                {
                    navigationController.SetTarget(enemyBaseTarget);
                    hasReachedBase = false; // Reset ao definir a base como destino
                }
            }


            yield return new WaitForSeconds(targetEvaluationFrequency);
        }
    }

    private void FindClosestEnemy()
    {
        float detectionRadius = detectionCollider.radius;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayerMask);

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider potentialTarget in hits)
        {
            if (potentialTarget.transform == this.transform) continue;

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        currentEntityTarget = bestTarget;
    }

    void OnDrawGizmosSelected()
    {
        if (detectionCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionCollider.radius);
        }
    }
}
