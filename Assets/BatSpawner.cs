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

    Vector3 SetSpawnTransform()
    {

        float screenHeight = Camera.main.orthographicSize * 2f; // get the height of the screen
        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height * (2f / 3f), 0f)).y; // get the world position of the top of the middle third of the screen
        float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height / 3f, 0f)).y; // get the world position of the bottom of the middle third of the screen

        float randomY = Random.Range(screenBottom, screenTop); // get a random Y position within the middle third of the screen

        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        return new Vector3(screenBounds.x, randomY, 0); // set the new position of the object
    }

    void SpawnBats()
    {
        GameObject bat = Instantiate(batPrefab, SetSpawnTransform(), Quaternion.identity, transform);
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
            Debug.Log(elapsedTime);

            elapsedTime += Time.deltaTime;
            if (bShouldSpawnBats())
            {
                SpawnBats();
            }
        }

    }
}
