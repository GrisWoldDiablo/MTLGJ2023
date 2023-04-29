using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlayerState
{
	Jump,
	Run,
	Slide
}

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D _body;
	[Header("Movement")] [SerializeField] private float _speed = 7f;
	[SerializeField] private float _jumpHeight = 14f;
	[SerializeField] private float _timeOfSlowdown = 3f;

	[Header("Camera")] [SerializeField] private CameraMovement _camera;

	[Header("Collisions")] [SerializeField]
	private ContactFilter2D _contactFilter2D;

	private bool _isGrounded => _body.IsTouching(_contactFilter2D);

	public bool IsGrounded => _isGrounded;
	public bool IsMovingForward => _dirX > 0.0f;

	public bool IsDead { get; set; } = false; // TODO Move dead logic to Player Class

	public PlayerState GetPlayerState()
	{
		if (!IsGrounded)
		{
			return PlayerState.Jump;
		}

		if (_dirY < 0.0f)
		{
			return PlayerState.Slide;
		}

		return PlayerState.Run;
	}

	public float Speed
	{
		get => _speed;
		set => _speed = value;
	}

	void Start()
	{
		_body = GetComponent<Rigidbody2D>();
	}

	private float _forwardSpeed = 1.1f;
	private float _maxForwardSpeed = 1.1f;
	private float _time = 0;
	private float _dirX;
	private float _dirY;

	void Update()
	{
		if (IsDead) // TODO Move dead logic to Player Class
		{
			_body.velocity = Vector2.zero;
			return;
		}

		_dirX = Input.GetAxisRaw("Horizontal");
		if (_dirX < 0f)
		{
			_dirX = -0.3f;
			_camera.AllowCameraX(false);
		}
		else if (_dirX >= 0f)
		{
			if (_forwardSpeed < _maxForwardSpeed)
			{
				_time += Time.deltaTime;
				if (_time > _timeOfSlowdown)
				{
					_forwardSpeed += Time.deltaTime * _maxForwardSpeed;
				}
			}
			else
			{
				_time = 0;
				_forwardSpeed = _maxForwardSpeed;
			}
			_dirX = _forwardSpeed;
			_camera.AllowCameraX(true);
		}

		_body.velocity = new Vector2(_dirX * _speed, _body.velocity.y);
		_dirY = Input.GetAxisRaw("Vertical");

		if ((Input.GetKeyDown(KeyCode.Space) || _dirY > 0) && _isGrounded)
		{
			_body.velocity = new Vector2(_body.velocity.x, _jumpHeight);
		}
	}

	/// <summary>
	/// Modifies speed by a ratio. If it's lower, it will return to normal after _timeOfSlowdown seconds, if it's higher it won't change until it is manually changed.
	/// </summary>
	public void ModifySpeed(float ratio)
	{
		_forwardSpeed = _maxForwardSpeed * 0.5f;
	}
}