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
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ProceduralEnvGenerator : MonoBehaviour
{
    [SerializeField]
    private EnvironmentAsset[] environmentObjectList;

    [SerializeField]
    private GameObject[] obstacleObjectList;

    [SerializeField]
    private int numAssetsToSpawnOnLoad = 10;

    [SerializeField]
    private Queue<EnvironmentAsset> spawnedEnvironmentSlices;

    //keeps track of the X coordinate at which to spawn the next environment slice
    private float currentSpawnPostitionX = 0.0f;
    private float spawnPositionY = 0.0f;

    //these should be the same
    [SerializeField]
    private int numSlicesToSpawnOnRenew = 5;

    //keeps track of expired slices, when this == numSlicesToSpawnOnRenew we generate another batch of the same amount
    private int expiredSlicesCount = 0;

    private static ProceduralEnvGenerator _sInstance;

    public int NumSlicesToSpawnOnRenew { get => numSlicesToSpawnOnRenew;}
    public static ProceduralEnvGenerator Get()
    {
        return _sInstance;
    }

    public void SpawnEnvironmentSlices(int numSlices)
    {
        for (int i = 0; i < numSlices; i++)
        {
            EnvironmentAsset RandomEnvironmentSlice = GetRandomEnvironmentAsset();
            GenerateEnvironmentSlice(RandomEnvironmentSlice);
        }
    }

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
        float spacing = EnvironmentAsset.GetEnvironmentLength();

        currentSpawnPostitionX += spacing;
        spawnedEnvironmentSlices.Enqueue(asset);
    }

    public void IncrementExpired()
    {
        expiredSlicesCount++;
        if (expiredSlicesCount >= NumSlicesToSpawnOnRenew)
        {
            expiredSlicesCount = 0;
            SpawnEnvironmentSlices(NumSlicesToSpawnOnRenew);
        }
    }

    private void Awake()
    {
        if (!_sInstance)
        {
            _sInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        spawnedEnvironmentSlices = new Queue<EnvironmentAsset>();

        //set first spawn position to leftmost coordinate of viewpoint
        currentSpawnPostitionX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        spawnPositionY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        SpawnEnvironmentSlices(numAssetsToSpawnOnLoad);
    }

}
