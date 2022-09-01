using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    [SerializeField] GameObject prey, predator;
        
    public float lastPredSpawn;

    int lastSpawnWall; //0 - Left, 1 - Down, 2 - Right, 3 - Up

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnWall = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > GameplayManager.instance.predSpawnRate + lastPredSpawn && GameplayManager.instance.playing)
        {
            lastPredSpawn = Time.timeSinceLevelLoad;

            SpawnPredator();
        }
    }

    void SpawnPredator()
    {
        lastSpawnWall += Random.Range(0, 4);

        if (lastSpawnWall >= 4)
        {
            lastSpawnWall -= 4;
        }

        Vector3 spawnPos = new Vector3(0, 6, 0);

        switch (lastSpawnWall)
        {
            case 0:
                spawnPos = new Vector3(-12, Random.Range(-4.7f, 4.7f), 0);
                break;

            case 1:
                spawnPos = new Vector3(Random.Range(-8.7f, 8.7f), 8, 0);
                break;

            case 2:
                spawnPos = new Vector3(12, Random.Range(-4.7f, 4.7f), 0);
                break;

            case 3:
                spawnPos = new Vector3(Random.Range(-8.7f, 8.7f), -8, 0);
                break;
        }

        Transform pred = Instantiate(predator, spawnPos, Quaternion.identity).transform;

        UIManager.instance.PredatorWarning(pred);
    }

    public bool SpawnPrey(Vector3 parentPosition)
    {
        Vector3 spawnPosition;

        if(parentPosition.magnitude < 1)
        {
            spawnPosition = parentPosition + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0);
        }
        else
        {
            spawnPosition = parentPosition - (0.5f * parentPosition.normalized);
        }

        Instantiate(prey, spawnPosition, Quaternion.identity);

        return true;
    }  
}
