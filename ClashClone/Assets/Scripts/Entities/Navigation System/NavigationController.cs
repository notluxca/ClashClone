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
        if (!thisAgent.enabled) return;
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

    /// <summary>
    /// Define um novo alvo para o agente e calcula um destino com deslocamento aleatório.
    /// O destino será um ponto em um anel ao redor do alvo, com uma distância entre minOffset e maxOffset.
    /// </summary>
    /// <param name="newTarget">O transform do novo alvo.</param>
    /// <param name="minOffset">A distância mínima do deslocamento a partir da posição do alvo.</param>
    /// <param name="maxOffset">A distância máxima do deslocamento a partir da posição do alvo.</param>
    // public void SetTargetWithRandomOffset(Transform newTarget, float minOffset = 2.0f, float maxOffset = 5.0f)
    // {
    //     target = newTarget;

    //     if (target != null)
    //     {
    //         // Passo 1: Gerar uma direção 2D aleatória.
    //         // Usamos .normalized para garantir que o vetor tenha um comprimento de exatamente 1.
    //         // Isso nos dá uma direção pura, sem influência da distância ainda.
    //         Vector2 randomDirection = Random.insideUnitCircle.normalized;

    //         // Passo 2: Escolher uma distância aleatória dentro do intervalo definido.
    //         // Random.Range(min, max) sorteia um número entre o mínimo e o máximo.
    //         float randomDistance = Random.Range(minOffset, maxOffset);

    //         // Passo 3: Calcular o vetor de deslocamento final.
    //         // Multiplicamos a direção (comprimento 1) pela distância aleatória.
    //         // Isso cria um vetor na direção certa e com o comprimento desejado.
    //         Vector3 offset = new Vector3(randomDirection.x, 0f, randomDirection.y) * randomDistance;

    //         // Passo 4: Aplicar o deslocamento à posição original do alvo.
    //         Vector3 destinationPosition = target.position + offset;

            

    //         // Passo 5: Definir o destino do agente.
    //         lastTargetPosition = destinationPosition;
    //         thisAgent.destination = lastTargetPosition;
    //     }
    // }

    private void RotateAgent()
    {
        FaceTarget();
        if (!thisAgent.enabled) return;
        // Se está parado no destino (idle OU atacando)...
        if (IsStoppedAtDestination)
        {
            
            
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

        Vector3 targetCenter = GetTargetCenter(target);
        Vector3 direction = (targetCenter - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private Vector3 GetTargetCenter(Transform t)
    {
        Collider col = t.GetComponent<Collider>();
        if (col != null)
        {
            return col.bounds.center;
        }

        Renderer renderer = t.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.center;
        }

        // Fallback: apenas a posição do Transform
        return t.position;
    }
}