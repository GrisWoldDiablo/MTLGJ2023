using UnityEngine;

//Slice of the environment
public class EnvironmentAsset : MonoBehaviour
{
	//split the sprite up into positions that can be selected randomly to spawn obstacles at
	const int NUM_OBSTACLE_SEGMENTS = 3;

	Transform[] segmentTransforms = new Transform[NUM_OBSTACLE_SEGMENTS];

	[SerializeField] private SpriteRenderer spriteRenderer;

	private ProceduralEnvGenerator envGenerator;

	private void Awake()
	{
		if (spriteRenderer == null)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		
		envGenerator = ProceduralEnvGenerator.Get();
	}

	private void Start()
	{
		DivideObstacleSegments();
	}

	public float GetEnvironmentLength()
	{
		Sprite sprite = GetEnvironmentSprite();
		return sprite.bounds.size.x;
	}

	public Sprite GetEnvironmentSprite()
	{
		return spriteRenderer.sprite;
	}


	private void DivideObstacleSegments()
	{
		float obstacleSpacing = GetEnvironmentLength() / (float)NUM_OBSTACLE_SEGMENTS;

		for (int i = 0; i < NUM_OBSTACLE_SEGMENTS; i++)
		{
			GameObject segmentObject = new GameObject("Segment " + (i + 1));
			segmentObject.transform.parent = gameObject.transform;
			segmentTransforms[i] = segmentObject.transform;

			segmentTransforms[i].position = new Vector3(gameObject.transform.position.x + (i * obstacleSpacing), gameObject.transform.position.y + GetEnvironmentSprite().bounds.max.y, 0f);

			//Generate obstacles for slice
			//keep a count of segments between slices, ensure that there's at least X segments between obstacles
			if (envGenerator.NumSegmentsSinceLastObstacles >= envGenerator.RandomizedNumSegmentsBetweenObstacles)
			{
				//eventually something to prevent same obstacle from spawning in  
				envGenerator.GenerateRandomObstacle(segmentTransforms[i].position, segmentTransforms[i].transform);

				//move this to generator so it handles its own value resets?
				envGenerator.NumSegmentsSinceLastObstacles = 0;
			}
			else
			{
				envGenerator.NumSegmentsSinceLastObstacles++;
			}

		}

	}

	void Update()
	{
		// Check if any tiles have moved off the left side of the screen
		float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 10;

		if (transform.position.x < screenLeft)
		{
			envGenerator.IncrementExpired(this);
			Destroy(gameObject);
		}

	}
}