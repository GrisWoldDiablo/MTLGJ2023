using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [Range(0,1)]
    [SerializeField] private float _powerUpChance = 0.7f;
    [Range(0,1)]
    [SerializeField] private float _sandalsChance = 0.5f;

    private ProceduralEnvGenerator _generator;
    private Character charRef;

    public GameObject SpawnPoint { set => spawnPoint = value; }

    // Start is called before the first frame update
    void Start()
    {
        charRef = GameManager.Get().Player;
        _generator =  Object.FindObjectOfType<ProceduralEnvGenerator>();
        _sandalsChance = _generator.CurrentBiomeIndex/(float)_generator.BiomeCount;
        if (_sandalsChance > 0.8)
        {
            _sandalsChance = 0.8f;
        }
        SpawnItem();
    }

    public void SpawnItem()
    {
        if (spawnPoint == null)
        {
            return;
        }
        int r = Random.Range(0, 10);
        if (r < (_powerUpChance * 10)) 
        { 
            int r2 = Random.Range(1, 11);
        
            if (r2 > _sandalsChance * 10)
            {
                if (charRef.HasHammer)
                {
                    //maybe only spawn sandals when we have a hammer?
                    return;
                }
                var item = Resources.Load<GameObject>("Hammer");
                Instantiate(item, spawnPoint.transform);
            
            }else {
                var item = Resources.Load<GameObject>("Sandals"); 
                Instantiate(item,spawnPoint.transform);
            }
        }
    }

}
