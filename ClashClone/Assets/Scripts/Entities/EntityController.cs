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
    private Rigidbody rigidBody;
    private Animator animator;

    private float lastAttackTime = -999f;

    void Awake()
    {
        targetingController = GetComponent<TargetingController>();
        navigationController = GetComponent<NavigationController>();
        rigidBody = GetComponent<Rigidbody>();
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

        // O EntityController só decide O QUE fazer, o NavigationController decide COMO se orientar.

        bool isMoving = !navigationController.IsStoppedAtDestination;

        if (isMoving)
        {
            PlayAnimation(moveAnimationName);
        }
        else // Se está parado
        {
            bool isCurrentlyAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName);

            if (isCurrentlyAttacking)
            {
                // Se a animação de ataque está tocando, não fazemos nada,
                // apenas deixamos o NavigationController continuar girando o personagem.
                return;
            }

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                PlayAnimation(attackAnimationName);
                Debug.Log(gameObject.name + " está atacando com: " + attackAnimationName);
            }
            else
            {
                PlayAnimation(idleAnimationName);
            }
        }
    }

    private void PlayAnimation(string stateName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            animator.Play(stateName);
        }
    }

    void SetTeamConfigs()
    {
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