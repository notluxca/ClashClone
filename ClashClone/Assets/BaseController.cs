using System;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public static Action<String> OnBaseDestroyed;


    void OnDestroy()
    {
        if (this.gameObject.CompareTag("LwoasTeamBase")) OnBaseDestroyed?.Invoke("Lwoas");
        if (this.gameObject.CompareTag("CoguTeamBase")) OnBaseDestroyed?.Invoke("Cogu");

    }
}
