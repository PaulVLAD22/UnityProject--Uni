using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Powerup
{
    public GameObject gameObject;
    [Range(0,100)]
    [Tooltip("Chance of spawning at a certain period of time - items are spawned every 10 seconds")]
    public float spawnRate;
}

[Serializable]
public class SpawnPoint
{
    public GameObject gameObject;
    public bool isOccupied;
}

[Serializable]
public class PowerupSpawnScript : MonoBehaviour
{
    public List<Powerup> Powerups;
    private List<SpawnPoint> SpawnPoints;
    //public List<SpawnPoint> SpawnPoints; // sum of spawnRates should be 100%
    public float SpawnDelay = 10; // seconds

    // Start is called before the first frame update
    void Start()
    {
        SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("PowerupPoint")).Select(x => new SpawnPoint
        {
            gameObject = x,
            isOccupied = false
        }).ToList();

        Debug.Log($"Spawn Points - {SpawnPoints.Count}");
        Debug.Log($"Powerups - {Powerups.Count}");
        InvokeRepeating("HandlePowerupSpawn", SpawnDelay, SpawnDelay);
        //SpawnPowerup(Powerups[0], SpawnPoints[0]);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void HandlePowerupSpawn()
    {
        foreach (Powerup powerup in Powerups)
        {
            if(powerup.spawnRate <= Random.Range(0, 100))
            {
                var available = SpawnPoints.Where(x => !x.isOccupied).ToList();
                if (available.Any())
                {
                    var spawnPoint = available[Random.Range(0, available.Count())];
                    SpawnPowerup(powerup, spawnPoint);
                }

            }
        }
    }

    private void SpawnPowerup(Powerup powerup, SpawnPoint spawn)
    {
        var @object = Instantiate(powerup.gameObject, spawn.gameObject.transform);
    }
}
