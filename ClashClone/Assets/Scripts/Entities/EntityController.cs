using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EntityController : MonoBehaviour
{
    [Header("Configurações de Time")]
    public GameTeams thisEntityTeam;

    [Header("Configurações de Combate")]
    public string attackAnimationName = "Attack";
    public string moveAnimationName = "Walk";
    public string idleAnimationName = "Idle";

    public float attackCooldown = 2f;

    [Header("Referências")]
    public TargetingController targetingController;
    private NavigationController navigationController;
    // private NavMeshObstacle navMeshObstacle;
    Rigidbody thisRigidbody;
    public Animator animator;

    // Controle de estado interno
    private float lastAttackTime = -999f;

    void Awake()
    {
        // navMeshObstacle = GetComponent<NavMeshObstacle>();
        // navMeshObstacle.enabled = false; // Desabilita o NavMeshObstacle para evitar interferência com a navegação
        targetingController = GetComponent<TargetingController>();
        navigationController = GetComponent<NavigationController>();
        thisRigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        targetingController.onReachEnemyBase.AddListener(OnEnemyBaseReach);
        SetTeamConfigs();
    }

    private void OnEnemyBaseReach()
    {

        StartCoroutine(BaseReachSequence());
    }

    IEnumerator BaseReachSequence()
    {
        // navigationController.thisAgent.enabled = false;
        thisRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(0.1f);
        // navMeshObstacle.enabled = true;

    }

    private void Update()
    {
        UpdateAnimatorState();
    }

    private void UpdateAnimatorState()
    {
        if (navigationController == null || animator == null) return;

        // AQUI ESTÁ A MUDANÇA: A definição de "movimento" agora é mais robusta.
        // A entidade é considerada "em movimento" se ela AINDA NÃO CHEGOU ao seu destino.
        bool isMoving = !navigationController.IsStoppedAtDestination;

        // --- LÓGICA SEM ALTERAÇÕES ---

        // 1. Se a entidade está se movendo, toque a animação de movimento.
        if (isMoving)
        {
            PlayAnimation(moveAnimationName);
        }
        // 2. Se a entidade está PARADA...
        else
        {
            // Verificamos se uma animação de ataque já está tocando.
            // Se estiver, não fazemos NADA para não interrompê-la.
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName))
            {
                // Deixa a animação de ataque terminar.
                return;
            }

            // Se não estamos atacando, podemos iniciar um novo ataque?
            // A verificação 'IsStoppedAtDestination' aqui ainda é útil para garantir 
            // que o ataque só aconteça no estado de "chegada".
            if (navigationController.IsStoppedAtDestination && Time.time >= lastAttackTime + attackCooldown || 
            targetingController.hasReachedBase && Time.time >= lastAttackTime + attackCooldown)
            {
                Debug.Log("Solicitando ataque caralho");
                // Sim, então atacamos.
                lastAttackTime = Time.time;
                PlayAnimation(attackAnimationName);
                
            }
            // Se não estamos atacando e não podemos atacar (em cooldown), ficamos parados.
            else
            {
                PlayAnimation(idleAnimationName);
            }
        }
    }

    /// <summary>
    /// Método auxiliar que toca uma animação somente se ela já não estiver tocando.
    /// </summary>
    private void PlayAnimation(string stateName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            animator.Play(stateName);
        }
    }

    void SetTeamConfigs()
    {
        // ... (seu método SetTeamConfigs existente, sem alterações) ...
        switch (thisEntityTeam)
        {
            case GameTeams.CogumeloTeam:
                targetingController.enemyLayerMask = GameInfo.LwoasTeamEntityLayer;
                targetingController.enemyBaseTarget = GameInfo.LwosBaseTransform;
                break;

            case GameTeams.LwosTeam:
                targetingController.enemyLayerMask = GameInfo.CoguTeamEntityLayer;
                targetingController.enemyBaseTarget = GameInfo.CoguBaseTransform;
                break;
        }
    }
}