using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent thisAgent;

    private float updateThreshold = 0.1f;
    private Vector3 lastTargetPosition;
    private float stoppingDistanceBuffer = 0.05f;

    void Awake()
    {
        thisAgent = GetComponent<NavMeshAgent>();
        thisAgent.updateRotation = false; // desativa rotação automática
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

        RotateAgent();
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
        // Está parado no destino
        if (!thisAgent.pathPending && thisAgent.remainingDistance <= thisAgent.stoppingDistance + stoppingDistanceBuffer)
        {
            FaceTarget(); // olha para o alvo
        }
        else if (thisAgent.velocity.sqrMagnitude > 0.01f)
        {
            // gira na direção da velocidade (movimento)
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
