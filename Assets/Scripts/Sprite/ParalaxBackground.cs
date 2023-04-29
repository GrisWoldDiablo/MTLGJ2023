using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
	[SerializeField] private float _speed = 1.0f;
	[SerializeField] private int numberOfParalax;

	private readonly Vector3 direction = new Vector3(1.0f, 0.0f, 0.0f);
	private CameraMovement _cameraMovement;
	private PlayerMovement _playerMovement;
	private SpriteRenderer _spriteRenderer;
	private float _spriteSize = 0.0f;
	private bool wasSeen = false;

	private void Awake()
	{
		_cameraMovement = FindObjectOfType<CameraMovement>();
		_playerMovement = FindObjectOfType<PlayerMovement>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_spriteSize = _spriteRenderer.sprite.bounds.size.x;
	}

	private void Update()
	{
		if (_spriteRenderer.isVisible && !wasSeen)
		{
			wasSeen = true;
		}
		
		if (!_cameraMovement.IsCameraMovingForward || Time.timeScale <= 0.0f)
		{
			return;
		}

		var transformPosition = direction * (_speed * Time.deltaTime * _playerMovement.Speed);
		gameObject.transform.position += transformPosition;
		
		if (!_spriteRenderer.isVisible && wasSeen)
		{
			wasSeen = false;
			gameObject.transform.position += direction * (_spriteSize * numberOfParalax);
		}
	}
}