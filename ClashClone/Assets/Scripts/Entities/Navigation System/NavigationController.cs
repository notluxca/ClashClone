using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent thisAgent;

    private float updateThreshold = 0.1f;
    private Vector3 lastTargetPosition;
    private float stoppingDistanceBuffer = 0.05f;

    public bool IsStoppedAtDestination { get; private set; }

    void Awake()
    {
        thisAgent = GetComponent<NavMeshAgent>();
        thisAgent.updateRotation = false;
    }

    void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
            thisAgent.destination = lastTargetPosition;
        }
    }

    void Update()
    {
        if (target == null) return;

        // Atualiza destino se o alvo se mover significativamente
        if (Vector3.Distance(lastTargetPosition, target.position) > updateThreshold)
        {
            lastTargetPosition = target.position;
            thisAgent.destination = lastTargetPosition;
        }

        // A lógica de estado e rotação agora roda a cada frame, sem interrupção.
        UpdateAgentState();
        RotateAgent();
    }

    private void UpdateAgentState()
    {
        if (target == null)
        {
            IsStoppedAtDestination = true;
            return;
        }

        if (!thisAgent.pathPending && thisAgent.remainingDistance <= thisAgent.stoppingDistance)
        {
            IsStoppedAtDestination = true;
        }
        else
        {
            IsStoppedAtDestination = false;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            lastTargetPosition = target.position;
            thisAgent.destination = lastTargetPosition;
        }
    }

    private void RotateAgent()
    {
        // Se está parado no destino (idle OU atacando)...
        if (IsStoppedAtDestination)
        {
            // ...SEMPRE encara o alvo.
            FaceTarget();
        }
        // Se está se movendo...
        else if (thisAgent.velocity.sqrMagnitude > 0.01f)
        {
            // ...gira na direção do movimento.
            Vector3 direction = thisAgent.velocity.normalized;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }

    private void FaceTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}