using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("Configurações de Time")]
    public GameTeams thisEntityTeam;

    [Header("Configurações de Combate")]
    public string attackAnimationName = "CapsuleAttack";
    public string moveAnimationName = "CapsuleWalk";
    public string idleAnimationName = "CapsuleIdle";

    public float attackCooldown = 2f;

    [Header("Referências")]
    public TargetingController targetingController;
    private NavigationController navigationController;
    private Animator animator;

    // Controle de estado interno
    private float lastAttackTime = -999f;

    void Awake()
    {
        targetingController = GetComponent<TargetingController>();
        navigationController = GetComponent<NavigationController>();
        animator = GetComponent<Animator>();
        SetTeamConfigs();
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
            if (navigationController.IsStoppedAtDestination && Time.time >= lastAttackTime + attackCooldown)
            {
                // Sim, então atacamos.
                lastAttackTime = Time.time;
                PlayAnimation(attackAnimationName);
                Debug.Log(gameObject.name + " está atacando com: " + attackAnimationName);
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