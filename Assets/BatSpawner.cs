using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    [SerializeField] GameObject batPrefab;

    [SerializeField] float minTimeBetweenBats = 5.0f;
    [SerializeField] float maxTimeBetweenBats = 10.0f;

    float elapsedTime = 0.0f;
    float timeToSpawn;

    GameManager gm;
    ProceduralEnvGenerator env;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Get();
        env = ProceduralEnvGenerator.Get();
    }

    float GetRandomTimeToSpawn()
    {
        return Random.Range(minTimeBetweenBats, maxTimeBetweenBats + 1);
    }

    void SpawnBats()
    {
        GameObject bat = Instantiate(batPrefab);
        elapsedTime = 0.0f;
        timeToSpawn = GetRandomTimeToSpawn();
    }

    bool bShouldSpawnBats()
    {
        if(elapsedTime >= timeToSpawn)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(env.GetCurrentBiomeType() == EBiomeType.Cave)
        {
            if (bShouldSpawnBats())
            {
                SpawnBats();
            }
        }

    }
}
