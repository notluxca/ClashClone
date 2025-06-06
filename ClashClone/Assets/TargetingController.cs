using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class TargetingController : MonoBehaviour
{
    private NavigationController navigationController;
    public LayerMask enemyLayerMask;

    void Awake()
    {
        navigationController = GetComponent<NavigationController>();
    }


    void Start()
    {

    }

    private void EvaluateCurrentTarget()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if ((enemyLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("blabla");
            // The object is in the target layer
            Debug.Log("Detected object in layer: " + LayerMask.LayerToName(other.gameObject.layer));
            navigationController.SetTarget(other.transform);
        }
    }


}
