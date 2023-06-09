using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private Transform player;
	[SerializeField] private SpriteRenderer _spriteRenderer;

	private bool _allowCameraX = true;
	private bool _isCameraMovingForward = true;
	public bool IsCameraMovingForward => _isCameraMovingForward;
	[SerializeField]
	private Camera _camera;
	private Vector2 screenBounds;
	private float objectWidth;

	private Vector2 _offset;
	private float preframeX;

	private void Start()
	{
		preframeX = player.transform.position.x;
		//_camera = GetComponentInChildren<Camera>();
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		objectWidth = _spriteRenderer.bounds.size.x / 2;
		_offset = new Vector2(transform.localPosition.x, transform.localPosition.y);
		transform.position = new Vector3(player.position.x + _offset.x, player.position.y + _offset.y, transform.position.z);
	}


	void Update()
	{
		//Y axis clamping
		float newX = transform.position.x;
		float newY = transform.position.y;
		float newZ = transform.position.z;

		//Add effects in vector3 if needed
		if (_allowCameraX)
		{
			newX = player.position.x + _offset.x;
		}
		if (player.position.y > _offset.y + 1f)
		{
			newY = player.position.y - 1f;
		}

		Vector3 updatePos = new Vector3(newX, newY, newZ);
		float incrementatedX = transform.position.x;
		transform.position = updatePos;
		incrementatedX = transform.position.x - incrementatedX;
		GameManager.Get().IncrementRunningDistance(incrementatedX);
	}

	private void FixedUpdate()
	{
		if (player.transform.position.x - preframeX > 0 && _allowCameraX)
		{
			_isCameraMovingForward = true;
		}
		else
		{
			_isCameraMovingForward = false;

		}
		preframeX = player.transform.position.x;
	}

	void LateUpdate()
	{
		//X axis clamping
		if (!_allowCameraX)
		{
			//Clamp player movement when camera is not moving
			screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));

			Vector3 viewPos = player.position;
			viewPos.x = Mathf.Clamp(viewPos.x, transform.position.x - (screenBounds.x - transform.position.x) + 0.46f, screenBounds.x - objectWidth);
			player.position = viewPos;
		}

	}

	private Vector3 tempPlayerPos;

	public void AllowCameraX(bool val)
	{
		if (!val && _allowCameraX)
		{
			tempPlayerPos = player.position;
			_allowCameraX = false;
		}

		if (val && !_allowCameraX)
		{
			if (player.position.x > tempPlayerPos.x)
			{
				tempPlayerPos.y = player.position.y;
				player.position = tempPlayerPos;
				_allowCameraX = true;
			}
		}

	}
}