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
        int r = Random.Range(0,5);
        
        switch (r)
        {
            case 1 or 2:
            {
                var item = Resources.Load<GameObject>("Hammer");
                Instantiate(item,spawnPoint.transform);
               break;
            }
            case 3 or 4:
            {
             var item = Resources.Load<GameObject>("Sandals");
             Instantiate(item,spawnPoint.transform);
                break;

            }
        }
    }

}
