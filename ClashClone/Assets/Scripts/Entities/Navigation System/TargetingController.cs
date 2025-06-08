using UnityEngine;
using System.Collections; // Adicionado para usar Coroutine

// Garante que o objeto tenha os componentes necessários
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
    private EntityController entityController;
    private NavigationController navigationController;
    public SphereCollider detectionCollider; // Usaremos o raio deste colisor para a detecção
    private Transform currentEntityTarget;

    void Awake()
    {
        navigationController = GetComponent<NavigationController>();
        // detectionCollider = GetComponent<SphereCollider>();
        entityController = GetComponent<EntityController>();

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
            // 1. Procura pelo inimigo mais próximo.
            FindClosestEnemy();

            // 2. Decide qual alvo seguir.
            if (currentEntityTarget != null)
            {
                // Se um inimigo foi encontrado, ele é a prioridade.
                // Verificamos se o alvo já não é este inimigo para evitar chamadas desnecessárias.
                if (navigationController.target != currentEntityTarget)
                {
                    navigationController.SetTarget(currentEntityTarget);
                }
            }
            else
            {

                if (navigationController.target != enemyBaseTarget)
                {

                    navigationController.SetTargetWithRandomOffset(enemyBaseTarget, 20f);
                    // A entidade recebe seu "posto de ataque" na base e se compromete com ele.
                    // Ajuste o raio (5f) conforme necessário.
                }
            }

            // Espera para a próxima avaliação.
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