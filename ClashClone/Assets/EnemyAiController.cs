using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnergyController energyController;

    [Header("Enemy Prefabs")]
    public GameObject enemy10Prefab;
    public GameObject enemy20Prefab;
    public GameObject enemy30Prefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    private float spawnTimer = 0f;

    [Header("Spawn Area (Plane)")]
    public Transform spawnArea;
    private Vector3 areaSize;

    private GameObject nextEnemyPrefab;
    private float nextEnemyCost;

    void Start()
    {
        areaSize = Vector3.Scale(spawnArea.localScale, new Vector3(10f, 0, 10f));
        ChooseNextEnemy();
    }

    void Awake()
    {
        BaseController.OnBaseDestroyed += TurnOff;
    }

    private void TurnOff()
    {
        BaseController.OnBaseDestroyed -= TurnOff; // Desinscreve do evento para evitar chamadas futuras 
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            TrySpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void TrySpawnEnemy()
    {
        if (energyController.CurrentEnergy >= nextEnemyCost)
        {
            Vector3 spawnPos = GetRandomPositionInArea();
            Instantiate(nextEnemyPrefab, spawnPos, Quaternion.identity);
            energyController.TrySpendEnergy(nextEnemyCost);
            ChooseNextEnemy(); // Escolhe o próximo inimigo aleatoriamente
        }
        // Se não tiver energia suficiente, aguarda até o próximo tick
    }

    void ChooseNextEnemy()
    {
        float roll = Random.value; // 0.0 a 1.0

        if (roll < 0.6f)
        {
            nextEnemyPrefab = enemy10Prefab;
            nextEnemyCost = 10f;
        }
        else if (roll < 0.9f)
        {
            nextEnemyPrefab = enemy20Prefab;
            nextEnemyCost = 20f;
        }
        else
        {
            nextEnemyPrefab = enemy30Prefab;
            nextEnemyCost = 30f;
        }

        // Debug.Log($"Próximo inimigo escolhido custa {nextEnemyCost}");
    }

    Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float z = Random.Range(-areaSize.z / 2f, areaSize.z / 2f);
        return spawnArea.position + new Vector3(x, 0f, z);
    }
}
