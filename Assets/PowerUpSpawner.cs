using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;

    public GameObject SpawnPoint { set => spawnPoint = value; }

    // Start is called before the first frame update
    void Start()
    {
      
       SpawnItem();
    }

    public void SpawnItem()
    {
        if (spawnPoint == null)
        {
            return;
        }
        int r = Random.Range(1,5);
        if (r == 3)
        {
            var item = Resources.Load<GameObject>("Hammer");
            Instantiate(item,spawnPoint.transform);
        }
        if (r == 4)
        {
            var item = Resources.Load<GameObject>("Sandals");
            Instantiate(item,spawnPoint.transform);
        }
    }

}
