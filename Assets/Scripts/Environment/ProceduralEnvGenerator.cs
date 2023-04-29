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
    private static ProceduralEnvGenerator _sInstance;

    [Header("EnvironmentLists")]
    [SerializeField] private EnvironmentAsset[] environmentObjectList;
	[SerializeField] private GameObject[] obstacleObjectList;
	[SerializeField] private int numAssetsToSpawnOnLoad = 10;
	[SerializeField] private int numSlicesToSpawnOnRenew = 5;

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

	public int NumSlicesToSpawnOnRenew{ get => numSlicesToSpawnOnRenew; } //does this need to be exposed?
    public int NumSegmentsSinceLastObstacles { get => numSegmentsSinceLastObstacles; set => numSegmentsSinceLastObstacles = value; }
    public float RandomizedNumSegmentsBetweenObstacles { get => randomizedNumSegmentsBetweenObstacles;}

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
	public GameObject GetRandomObstacle()
	{
        int randomAssetIndex = Random.Range(0, obstacleObjectList.Length);
        return obstacleObjectList[randomAssetIndex];
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
        RandomizeNumberSegmentsBetweenObstacles();
        GameObject obstacle = Instantiate(GetRandomObstacle(), SpawnPosition, Quaternion.identity, Parent);
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

		RandomizeNumberSegmentsBetweenObstacles();

		//set first spawn position to leftmost coordinate of viewpoint
		currentSpawnPostitionX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
		spawnPositionY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 0.7f; //expose this

		SpawnEnvironmentSlices(numAssetsToSpawnOnLoad);
	}
}