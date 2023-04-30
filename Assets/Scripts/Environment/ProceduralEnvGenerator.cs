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
using System.Linq;

public enum EBiomeType
{
	VolcanoArea = 0,
	Cave		= 1,
	NightSky	= 2
};

[System.Serializable]
public class Biome
{
	[SerializeField] EBiomeType biomeType;
    [SerializeField] private EnvironmentAsset[] environmentObjectList;
    [SerializeField] private GameObject[] obstacleObjectList;

    public EBiomeType BiomeType { get => biomeType; set => biomeType = value; }
    public EnvironmentAsset[] EnvironmentObjectList { get => environmentObjectList; set => environmentObjectList = value; }
    public GameObject[] ObstacleObjectList { get => obstacleObjectList; set => obstacleObjectList = value; }
}

public class ProceduralEnvGenerator : MonoBehaviour
{
    private static ProceduralEnvGenerator _sInstance;

	[Header("EnvironmentLists")]
	[SerializeField] List<Biome> biomeList = new List<Biome>();
	[SerializeField] private int numAssetsToSpawnOnLoad = 10;
	[SerializeField] private int numSlicesToSpawnOnRenew = 5;

	private Biome currentBiome;

    //keeps track of the X coordinate at which to spawn the next environment slice
    private float currentSpawnPostitionX = 0.0f;
	private float spawnPositionY = 0.0f;

	//these should be the same

	private int expiredSlicesCount = 0;

	[Header("Obstacles")]
    [SerializeField]
    private int minSegmentsBetweenObstacles = 18;
    [SerializeField]
    private int maxSegmentsBetweenObstacles = 64;
    private float randomizedNumSegmentsBetweenObstacles; //uses range & above value
    private int numSegmentsSinceLastObstacles = 0;
	[Header("Debug")]
	[SerializeField]
	bool disableObstacle = false;
    [SerializeField]
    float ySpawnModifier = -1.0f;

    public int NumSlicesToSpawnOnRenew{ get => numSlicesToSpawnOnRenew; } //does this need to be exposed?
    public int NumSegmentsSinceLastObstacles { get => numSegmentsSinceLastObstacles; set => numSegmentsSinceLastObstacles = value; }
    public float RandomizedNumSegmentsBetweenObstacles { get => randomizedNumSegmentsBetweenObstacles;}

	//cycle through available biomes and find the one that matches our set biome
	private Biome GetCurrentBiome()
	{
		return biomeList[0];
    }

	private void AdvanceToNextBiome()
	{
        biomeList.Remove(currentBiome);
		currentBiome = GetCurrentBiome();
    }

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
		int randomAssetIndex = Random.Range(0, currentBiome.EnvironmentObjectList.Length);
		return currentBiome.EnvironmentObjectList[randomAssetIndex];
	}
	public GameObject GetRandomObstacle()
	{
        int randomAssetIndex = Random.Range(0, currentBiome.ObstacleObjectList.Length);
        return currentBiome.ObstacleObjectList[randomAssetIndex];
    }

	private void RandomizeNumberSegmentsBetweenObstacles()
	{
        randomizedNumSegmentsBetweenObstacles = Random.Range(minSegmentsBetweenObstacles, maxSegmentsBetweenObstacles);
    }

	//called by asset on appropriate segment, maybe better to just contain this in the slice class directly
	public void GenerateRandomObstacle(Vector2 SpawnPosition, Transform Parent)
	{
        if (disableObstacle)
        {
            return;
        }

		//lower obstacle a bit to compensate for overextending grass mesh
		Vector2 adjustedVector = new Vector2(SpawnPosition.x, SpawnPosition.y + ySpawnModifier);

        RandomizeNumberSegmentsBetweenObstacles();
        GameObject obstacle = Instantiate(GetRandomObstacle(), adjustedVector, Quaternion.identity, Parent);
    }

    private void GenerateEnvironmentSlice(EnvironmentAsset EnvironmentAsset)
	{
		Vector2 spawnPosition = new Vector2(currentSpawnPostitionX, spawnPositionY);
		EnvironmentAsset slice = Instantiate(EnvironmentAsset, spawnPosition, Quaternion.identity, gameObject.transform);
		
		float spacing = slice.GetEnvironmentLength();

		currentSpawnPostitionX += spacing;
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
		}
		else
		{
			DestroyImmediate(this);
		}
	}

	// Start is called before the first frame update
	private void Start()
	{
		currentBiome = GetCurrentBiome();

        RandomizeNumberSegmentsBetweenObstacles();

		//set first spawn position to leftmost coordinate of viewpoint
		currentSpawnPostitionX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
		spawnPositionY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 0.7f; //expose this
		SpawnEnvironmentSlices(numAssetsToSpawnOnLoad);
	}
}