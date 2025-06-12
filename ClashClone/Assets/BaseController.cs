using System;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public static Action OnBaseDestroyed;


    void OnDestroy()
    {
        OnBaseDestroyed?.Invoke();
    }
}
