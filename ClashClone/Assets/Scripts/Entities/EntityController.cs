using UnityEngine;


// decide team
public class EntityController : MonoBehaviour
{
    public GameTeams thisEntityTeam;
    public TargetingController targetingController;

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
