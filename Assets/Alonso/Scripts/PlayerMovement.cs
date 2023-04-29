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
	[SerializeField] private float _slidingDuration = 2f;
	[Header("Camera")] [SerializeField] private CameraMovement _camera;

	[Header("Collisions")] [SerializeField]
	private ContactFilter2D _contactFilter2D;
	private BoxCollider2D _boxCollider2D;
	private Vector2 _standingColliderSize;
	private Vector2 _standingColliderOffset;
	private Vector2 _slidingColliderSize;
	private Vector2 _slidingColliderOffset;
	
	private bool _isGrounded => _body.IsTouching(_contactFilter2D);
	private bool _isSliding = false;
	public bool IsGrounded => _isGrounded;
	public bool IsMovingForward => _dirX > 0.0f;

	public bool IsDead { get; set; } = false; // TODO Move dead logic to Player Class

	public PlayerState GetPlayerState()
	{
		if (!IsGrounded)
		{
			return PlayerState.Jump;
		}

		if (_isSliding)
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
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_standingColliderSize = _boxCollider2D.size;
		_standingColliderOffset = _boxCollider2D.offset;
		_slidingColliderSize = new Vector2(_standingColliderSize.x,_standingColliderSize.y / 2);
		_slidingColliderOffset = new Vector2(_boxCollider2D.offset.x,_boxCollider2D.offset.y * 2);

	}

	private float _forwardSpeed = 1.1f;
	private float _maxForwardSpeed = 1.1f;
	private float _time = 0;
	private float _slidingTime = 0;
	private float _dirX;
	private bool _isHittingRoof;
	private float _bufferSlidingTime = -1f;
	private float _maxSlidingBufferTime = 0.25f;

	void Update()
	{
		_isHittingRoof = IsHittingRoof();
		if (IsDead) // TODO Move dead logic to Player Class
		{
			_body.velocity = Vector2.zero;
			return;
		}

		MoveLeftRight();
		MoveUpDown();
	}

	private void MoveLeftRight()
	{
		if (_bufferSlidingTime > 0)
		{
			return;
		}
		
		_dirX = Input.GetAxisRaw("Horizontal");
		if (_dirX < 0f)
		{
			_dirX = -0.3f;
			_camera.AllowCameraX(false);
			_isSliding = false;

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
		
		if (!_isSliding)
		{
			ResetCollisions();
		}
	}


	private void MoveUpDown()
	{

		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && IsGrounded && _bufferSlidingTime <= 0)
		{
			_body.velocity = new Vector2(_body.velocity.x, _jumpHeight);
			_isSliding = false;
		}

		if (Input.GetButtonDown("Slide") && !_isSliding && IsGrounded && _dirX > 0)
		{
			_isSliding = true;
			_slidingTime = _slidingDuration;
			_boxCollider2D.size = _slidingColliderSize;
			_boxCollider2D.offset = _slidingColliderOffset;

		}

		if (_isSliding)
		{
			if (_isHittingRoof)
			{
				_slidingTime = Mathf.Max(_maxSlidingBufferTime,_slidingTime);
				_bufferSlidingTime = _maxSlidingBufferTime;
			}
			
			if (_slidingTime > 0 )
			{
				_slidingTime -= Time.deltaTime;
				_bufferSlidingTime -= Time.deltaTime;
			}
			else
			{
				_isSliding = false;
			}
		}
		else
		{
			_bufferSlidingTime = -1;
		}
	}

	private bool IsHittingRoof()
	{
		var hitPos1 = new Vector2(transform.position.x-0.01f,transform.position.y);
		var hitPos2 = new Vector2(transform.position.x+0.01f,transform.position.y);
		
		RaycastHit2D hit = Physics2D.Raycast(hitPos1, Vector2.up,1f);
		RaycastHit2D hit2 = Physics2D.Raycast(hitPos2, Vector2.up,1f);

		return hit && hit2;
	}

	private void ResetCollisions()
	{
		_boxCollider2D.size = _standingColliderSize;
		_boxCollider2D.offset = _standingColliderOffset;
	}

	/// <summary>
	/// Modifies speed by a ratio. If it's lower, it will return to normal after _timeOfSlowdown seconds, if it's higher it won't change until it is manually changed.
	/// </summary>
	public void ModifySpeed(float ratio)
	{
		_forwardSpeed = _maxForwardSpeed * 0.5f;
	}
}