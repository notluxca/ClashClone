using UnityEngine;
using UnityEngine.AI;

// Essa classe é responsavel pelo sistema de navegação de todas as entidades do jogo
public class NavigationController : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent thisAgent;

    private float updateThreshold = 0.1f; // Tolerância para atualizar o destino
    private Vector3 lastTargetPosition;

    void Awake()
    {
        thisAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
            thisAgent.SetDestination(lastTargetPosition);
        }
    }

    void Update()
    {
        if (target == null) return;

        // Só atualiza o destino se o alvo se moveu além do threshold
        if (Vector3.Distance(lastTargetPosition, target.position) > updateThreshold)
        {
            lastTargetPosition = target.position;
            thisAgent.SetDestination(lastTargetPosition);
        }
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
}