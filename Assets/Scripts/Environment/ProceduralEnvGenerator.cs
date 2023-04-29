using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;
using UnityEngine.U2D;

public class ProceduralEnvGenerator : MonoBehaviour
{
    [SerializeField]
    private EnvironmentAsset[] environmentObjectList;

    [SerializeField]
    private GameObject[] obstacleObjectList;

    [SerializeField]
    public int numAssetsToSpawnOnLoad = 10;
    //[SerializeField]
    //public float spacing = 2f;

    //keeps track of the X coordinate at which to spawn the next environment slice
    private float currentSpawnPostitionX = 0.0f;
    private float spawnPositionY = 0.0f;

    //might not need to be exposed if flat ground?

    //benefits of EnvironmentSliceClass ? what functionality would this hold
    
    private EnvironmentAsset GetRandomEnvironmentAsset()
    {
        int randomAssetIndex = Random.Range(0, environmentObjectList.Length);
        return environmentObjectList[randomAssetIndex];
    }

    private void GenerateEnvironmentSlice(EnvironmentAsset EnvironmentAsset)
    {
        Vector2 spawnPosition = new Vector2(currentSpawnPostitionX, spawnPositionY);
        EnvironmentAsset asset = Instantiate(EnvironmentAsset, spawnPosition, Quaternion.identity, this.gameObject.transform);
        asset.transform.position = new Vector2(spawnPosition.x, spawnPosition.y);
        //this spacing value should be the length of our chosen slice of environment  (ideally they are all the same)
        float spacing = EnvironmentAsset.GetEnvironmentLength();

        currentSpawnPostitionX += spacing;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //set first spawn position to leftmost coordinate of viewpoint

        //currentSpawnPostitionX = this.gameObject.transform.position.x;

        currentSpawnPostitionX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        spawnPositionY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        for (int i = 0; i < numAssetsToSpawnOnLoad; i++)
        {
            EnvironmentAsset RandomEnvironmentSlice = GetRandomEnvironmentAsset();
            GenerateEnvironmentSlice(RandomEnvironmentSlice);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
