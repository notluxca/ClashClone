using UnityEngine;

public static class GameInfo
{
    public static Transform CoguBaseTransform { get; private set; }
    public static Transform LwosBaseTransform { get; private set; }

    static GameInfo()
    {
        CoguBaseTransform = GameObject.FindWithTag("CoguTeamBase").transform;
        LwosBaseTransform = GameObject.FindWithTag("LwosTeamBase").transform;
    }
}

