using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int threshold;
    public float spawnDistance;
    public Camera camera;
    public LayerMask groundLayer;

    [Header ("Enemies")]
    public GameObject skeletonEnemy;
    public GameObject robotEnemy;


    private List<GameObject> enemies;

    void Awake() 
    {
        enemies = new List<GameObject>();

        if (skeletonEnemy) {
            enemies.Add(skeletonEnemy);
        }

        Debug.Log(skeletonEnemy);
        if (robotEnemy) {
            enemies.Add(robotEnemy);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        GameObject[] presentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        int numberOfEnemies = presentEnemies.Length;

        if (numberOfEnemies <= threshold) {
            Debug.LogFormat("Number of enemies is {0}, trying to spawn a new enemy", numberOfEnemies);

            Vector3 point = GetRandomSpawnLocation();
            if (Physics.Raycast(point, -transform.up, 2f, groundLayer)) {
                Instantiate(enemies[Random.Range(0, enemies.Count)], point, Quaternion.identity);
                Debug.Log("New enemy instantiated");
            }
        }
    }

    Vector3 GetRandomSpawnLocation()
    {
        UnityEngine.AI.NavMeshTriangulation navMeshData = UnityEngine.AI.NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);
        
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }

    bool CanBeSpawned(Vector3 point) {
        Vector3 pointOnCamera = camera.WorldToScreenPoint(point);
 
        // Check if point is on camera
        if ((pointOnCamera.x < 0) || (pointOnCamera.x > Screen.width) || (pointOnCamera.y < 0) || (pointOnCamera.y > Screen.height) || pointOnCamera.z < 0)
        {
            return false;
        }
        
        return (Vector3.Distance(point, camera.transform.position) > spawnDistance);
    }
}
