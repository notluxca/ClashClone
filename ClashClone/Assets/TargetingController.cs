using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class TargetingController : MonoBehaviour
{
    private NavigationController navigationController;
    public LayerMask enemyLayerMask;
    private bool haveEntityTarget = false;
    private Transform currentEntityTarget;
    public Transform enemyBaseTarget;

    void Awake()
    {
        navigationController = GetComponent<NavigationController>();
    }

    void Update()
    {
        EvaluateCurrentTarget();
    }

    private void EvaluateCurrentTarget()
    {
        if (currentEntityTarget == null)
        {
            navigationController.SetTarget(enemyBaseTarget);
            haveEntityTarget = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (haveEntityTarget) return;
        if ((enemyLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            haveEntityTarget = true;
            currentEntityTarget = other.transform;
            navigationController.SetTarget(other.transform);
        }
    }


}
