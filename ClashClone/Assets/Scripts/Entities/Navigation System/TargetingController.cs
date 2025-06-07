using UnityEngine;
using System.Collections; // Adicionado para usar Coroutine

// Garante que o objeto tenha os componentes necessários
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(NavigationController))]
public class TargetingController : MonoBehaviour
{
    [Header("Configurações de Alvo")]
    public LayerMask enemyLayerMask;
    public Transform enemyBaseTarget; // Alvo padrão (base inimiga)

    [Header("Configurações de Detecção")]
    [Tooltip("Com que frequência (em segundos) o controlador procura por novos alvos.")]
    public float targetEvaluationFrequency = 0.2f;

    // Componentes e Estado
    private NavigationController navigationController;
    private SphereCollider detectionCollider; // Usaremos o raio deste colisor para a detecção
    private Transform currentEntityTarget;

    void Awake()
    {
        navigationController = GetComponent<NavigationController>();
        detectionCollider = GetComponent<SphereCollider>();

        // Garante que o colisor seja um trigger para não causar colisões físicas
        detectionCollider.isTrigger = true;
    }

    void Start()
    {
        // Inicia a rotina de avaliação de alvos
        StartCoroutine(EvaluateTargetsCoroutine());
    }

    /// <summary>
    /// Coroutine que roda periodicamente para encontrar e definir o melhor alvo.
    /// Usar uma Coroutine é mais performático do que fazer isso a cada frame no Update().
    /// </summary>
    private IEnumerator EvaluateTargetsCoroutine()
    {
        while (true)
        {
            // 1. Procura pelo inimigo mais próximo dentro do raio de detecção
            FindClosestEnemy();

            // 2. Decide qual alvo o NavigationController deve seguir
            if (currentEntityTarget != null)
            {
                // Se encontrou uma entidade inimiga, define como alvo
                navigationController.SetTarget(currentEntityTarget);
            }
            else
            {
                // Se não há inimigos por perto, define a base como alvo
                navigationController.SetTarget(enemyBaseTarget);
            }

            // Espera um curto período antes de reavaliar, para otimização
            yield return new WaitForSeconds(targetEvaluationFrequency);
        }
    }

    /// <summary>
    /// Usa OverlapSphere para encontrar todos os inimigos no raio e define o mais próximo como alvo atual.
    /// </summary>
    private void FindClosestEnemy()
    {
        float detectionRadius = detectionCollider.radius;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayerMask);

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider potentialTarget in hits)
        {
            // Garante que não está se alvejando
            if (potentialTarget.transform == this.transform) continue;

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        // Atualiza o alvo atual
        currentEntityTarget = bestTarget;
    }

    // Opcional: Para visualizar o raio de detecção no Editor
    void OnDrawGizmosSelected()
    {
        if (detectionCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionCollider.radius);
        }
    }
}