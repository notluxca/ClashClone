using UnityEngine;


// decide team
public class EntityController : MonoBehaviour
{
    public GameTeams thisEntityTeam;
    TargetingController targetingController;

    void Awake()
    {
        targetingController = GetComponent<TargetingController>();
        SetTeamConfigs();
    }

    void SetTeamConfigs()
    {
        switch (thisEntityTeam)
        {
            case GameTeams.CogumeloTeam:
                targetingController.enemyLayerMask = LayerMask.GetMask("LwoasEntity");
                targetingController.enemyBaseTarget = GameInfo.LwosBaseTransform;
                break;

            case GameTeams.LwosTeam:
                targetingController.enemyLayerMask = LayerMask.GetMask("CoguEntity");
                targetingController.enemyBaseTarget = GameInfo.CoguBaseTransform;
                break;
        }
    }
}
