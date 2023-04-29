using UnityEngine;
using UnityEngine.Serialization;

public class Parallax : MonoBehaviour
{
	[FormerlySerializedAs("Layer_Speed")] [Header("Layer Setting")]
	public float[] Layer_SpeedX;

	[Header("Layer Setting")] public float[] Layer_SpeedY;
	public GameObject[] Layer_Objects;

	private Transform _camera;
	private float[] startPosX;
	private float boundSizeX;
	private float[] startPosY;
	private float boundSizeY;
	private float sizeX;
	private float sizeY;

	void Start()
	{
		_camera = Camera.main.transform;

		sizeX = Layer_Objects[0].transform.localScale.x;
		boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		startPosX = new float[Layer_Objects.Length];

		sizeY = Layer_Objects[0].transform.localScale.y;
		boundSizeY = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		startPosY = new float[Layer_Objects.Length];

		var cameraPosition = _camera.position;
		for (int i = 0; i < Layer_Objects.Length; i++)
		{
			startPosX[i] = cameraPosition.x;
			startPosY[i] = cameraPosition.y;
		}
	}

	void Update()
	{
		for (int i = 0; i < Layer_Objects.Length; i++)
		{
			var cameraPosition = _camera.position;
			float temp = cameraPosition.x * (1 - Layer_SpeedX[i]);
			float distance = cameraPosition.x * Layer_SpeedX[i];
			float distanceY = cameraPosition.y * Layer_SpeedY[i];

			Layer_Objects[i].transform.position = new Vector2(startPosX[i] + distance, startPosY[i] + distanceY);

			if (temp > startPosX[i] + boundSizeX * sizeX)
			{
				startPosX[i] += boundSizeX * sizeX;
			}
			else if (temp < startPosX[i] - boundSizeX * sizeX)
			{
				startPosX[i] -= boundSizeX * sizeX;
			}

			temp = _camera.position.y * (1 - Layer_SpeedY[i]);

			if (temp > startPosY[i] + boundSizeY * sizeY)
			{
				startPosY[i] += boundSizeY * sizeY;
			}
			else if (temp < startPosY[i] - boundSizeY * sizeY)
			{
				startPosY[i] -= boundSizeY * sizeY;
			}
		}
	}
}