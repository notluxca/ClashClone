using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    public void Shoot()
    {
        Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
    }
}
