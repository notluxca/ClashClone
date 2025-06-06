using UnityEngine;

public static class GameInfo
{
    public static Transform CoguBaseTransform { get; private set; }
    public static Transform LwosBaseTransform { get; private set; }
    public static LayerMask CoguTeamEntityLayer;
    public static LayerMask LwoasTeamEntityLayer;

    static GameInfo()
    {
        CoguTeamEntityLayer = LayerMask.NameToLayer("CoguEntity");
        LwoasTeamEntityLayer = LayerMask.NameToLayer("LwoasEntity");
        Debug.Log(CoguTeamEntityLayer.value + " " + LwoasTeamEntityLayer.value);
        CoguBaseTransform = GameObject.FindWithTag("CoguTeamBase").transform;
        LwosBaseTransform = GameObject.FindWithTag("LwoasTeamBase").transform;
    }
}

