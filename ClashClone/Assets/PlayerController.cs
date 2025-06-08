using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask placeableGroundLayerMask;
    public GameObject[] entityPrefabs; // Lista com os 3 personagens
    private int currentPrefabIndex = 0;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null || !mainCamera.orthographic)
        {
            Debug.LogError("Main Camera not found or is not orthographic. Please tag your orthographic camera as 'MainCamera'.");
            enabled = false;
        }
    }

    void Update()
    {
        HandleCharacterSelection();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placeableGroundLayerMask))
            {
                Vector3 placementPosition = hit.point;

                if (entityPrefabs != null && entityPrefabs.Length > currentPrefabIndex && entityPrefabs[currentPrefabIndex] != null)
                {
                    Instantiate(entityPrefabs[currentPrefabIndex], placementPosition, Quaternion.identity);
                    Debug.Log($"Instantiated character {currentPrefabIndex + 1} at {placementPosition}");
                }
                else
                {
                    Debug.LogError("Entity prefab is missing or not assigned.");
                }

                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 2f);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 300f, Color.red, 2f);
                Debug.Log("Clicked on nothing on the PlaceableGround layer.");
            }
        }
    }

    private void HandleCharacterSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectCharacter(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectCharacter(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectCharacter(2);
    }

    private void SelectCharacter(int index)
    {
        if (index >= 0 && index < entityPrefabs.Length)
        {
            currentPrefabIndex = index;
            Debug.Log($"Selected character: {index + 1}");
        }
        else
        {
            Debug.LogWarning("Invalid character index");
        }
    }
}
