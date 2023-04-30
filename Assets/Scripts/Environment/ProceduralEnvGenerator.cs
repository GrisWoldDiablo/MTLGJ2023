using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EBiomeType
{
	VolcanoArea = 0,
	Cave = 1,
	NightSky = 2
};

[System.Serializable]
public class Biome
{
	[SerializeField] EBiomeType biomeType;
	[SerializeField] private EnvironmentAsset[] environmentObjectList;
    [SerializeField] private EnvironmentAsset biomeInitSlice;
    [SerializeField] private GameObject[] obstacleObjectList;
	[SerializeField] private GameObject parallax;

	[SerializeField] float distanceToSpawnNext = 1000.0f;

    public EBiomeType BiomeType
	{
		get => biomeType;
		set => biomeType = value;
	}

	public EnvironmentAsset[] EnvironmentObjectList
	{
		get => environmentObjectList;
		set => environmentObjectList = value;
	}

	public GameObject[] ObstacleObjectList
	{
		get => obstacleObjectList;
		set => obstacleObjectList = value;
	}

	public GameObject Parallax
	{
		get => parallax;
		set => parallax = value;
    }
    public EnvironmentAsset BiomeInitSlice
    {
        get => biomeInitSlice;
    }
    public float DistanceToSpawnNext 
	{ 
		get => distanceToSpawnNext;
	}
}

public class ProceduralEnvGenerator : MonoBehaviour
{
	private static ProceduralEnvGenerator _sInstance;
	[SerializeField] private bool cheat_NEXTBIOME = false;

	[Header("EnvironmentLists")] [SerializeField]
	List<Biome> biomeList = new List<Biome>();
	private Biome currentBiome;

	[SerializeField] private int numAssetsToSpawnOnLoad = 10;
	[SerializeField] private int numSlicesToSpawnOnRenew = 5;
	private Parallax parallax;

	private GameManager gm;

	//keeps track of the X coordinate at which to spawn the next environment slice
	private float currentSpawnPostitionX = 0.0f;
	private float spawnPositionY = 0.0f;

	private int expiredSlicesCount = 0;

	[Header("Obstacles")] [SerializeField] private int minSegmentsBetweenObstacles = 18;
	[SerializeField] private int maxSegmentsBetweenObstacles = 64;
	private float randomizedNumSegmentsBetweenObstacles; //uses range & above value
	private int numSegmentsSinceLastObstacles = 0;
	[Header("Debug")] [SerializeField] bool disableObstacle = false;
	[SerializeField] float ySpawnModifier = -1.0f;

	private int _currentBiomeIndex = -1;

	public int NumSlicesToSpawnOnRenew
	{
		get => numSlicesToSpawnOnRenew;
	} //does this need to be exposed?

	public int NumSegmentsSinceLastObstacles
	{
		get => numSegmentsSinceLastObstacles;
		set => numSegmentsSinceLastObstacles = value;
	}

	public float RandomizedNumSegmentsBetweenObstacles
	{
		get => randomizedNumSegmentsBetweenObstacles;
	}

	//cycle through available biomes and find the one that matches our set biome
	private Biome GetCurrentBiome()
	{
		return biomeList[_currentBiomeIndex];
	}

	private void Update()
	{
		if (cheat_NEXTBIOME)
		{
			cheat_NEXTBIOME = false;
			GetandAdvanceToNextBiome();
		}

		if (ShouldAdvanceBiome())
		{
            GetandAdvanceToNextBiome();
        }
    }

	bool ShouldAdvanceBiome()
	{
		if(_currentBiomeIndex < biomeList.Count - 1 && gm.RunningDistance >= currentBiome.DistanceToSpawnNext)
		{
			return true;
		}
		return false;
	}

	private void InitializeBiome()
	{
		var newParallax = Instantiate(currentBiome.Parallax, Camera.main.transform).GetComponent<Parallax>();
		newParallax.transform.localPosition = Vector3.zero;

		if (parallax)
		{
			var startPosX = parallax.StartPosX;
			var startPosY = parallax.StartPosY;
			for (var i = 0; i < parallax.Layer_Objects.Length; i++)
			{
				newParallax.Layer_Objects[i].transform.localPosition = parallax.Layer_Objects[i].transform.localPosition;
			}

			newParallax.StartPosX = startPosX;
			newParallax.StartPosY = startPosY;

			Destroy(parallax.gameObject);
			newParallax.Update();
			UIManager.Get().SwapBiome();
		}
		parallax = newParallax;

		//handle spawning of special slice if exists
		if(currentBiome.BiomeInitSlice != null)
		{
            EnvironmentAsset slice = currentBiome.BiomeInitSlice;
            GenerateEnvironmentSlice(slice);
        }
    }

    private Biome GetandAdvanceToNextBiome()
	{
		_currentBiomeIndex++;
		currentBiome = GetCurrentBiome();

		InitializeBiome();
		return currentBiome;
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
		if (disableObstacle || currentBiome.ObstacleObjectList.Length <= 0)
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
		gm = GameManager.Get();

		currentBiome = GetandAdvanceToNextBiome();

		RandomizeNumberSegmentsBetweenObstacles();

		//set first spawn position to leftmost coordinate of viewpoint
		currentSpawnPostitionX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
		spawnPositionY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 0.7f; //expose this
		SpawnEnvironmentSlices(numAssetsToSpawnOnLoad);
	}
}