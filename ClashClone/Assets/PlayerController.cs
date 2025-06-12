using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask placeableGroundLayerMask;
    public GameObject[] entityPrefabs; // Lista com os 3 personagens
    private int currentPrefabIndex = 0;

    EnergyController energyController; // Referência ao controlador de energia
    

    private Camera mainCamera;

    public static event System.Action<int> CharacterIndexSelected;

    void Awake()
    {
        energyController = GetComponent<EnergyController>();
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleCharacterSelection();

        if (Input.GetMouseButtonDown(0))
        {
            
            if (energyController.TrySpendEnergy(GetCurrentEnemyCost())) // return false if not enough energy
            {

                InstantiateEntityPrefab();
                Debug.Log("Character instantiated successfully.");
            }
            
        }
    }

    public float GetCurrentEnemyCost()
    {
        switch (currentPrefabIndex)
        {
            case 0:
                return 10f; // Custo do primeiro personagem
            case 1:
                return 20f; // Custo do segundo personagem
            case 2:
                return 30f; // Custo do terceiro personagem
            default:
                Debug.LogWarning("Invalid character index");
                break;
        }
        return 0f;
    }

    private void InstantiateEntityPrefab()
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
            CharacterIndexSelected?.Invoke(index); // Notifica outros componentes sobre a seleção do personagem
            Debug.Log($"Selected character: {index + 1}");
        }
        else
        {
            Debug.LogWarning("Invalid character index");
        }
    }
}
