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
    [SerializeField] private float _slidingDuration = 2f;
    [Header("Camera")] [SerializeField] private CameraMovement _camera;

    [Header("Collisions")] [SerializeField]
    private ContactFilter2D _contactFilter2D;
    private BoxCollider2D _boxCollider2D;
    private Vector2 _standingColliderSize;
    private Vector2 _standingColliderOffset;
    private Vector2 _slidingColliderSize;
    private Vector2 _slidingColliderOffset;
	
    public bool IsGrounded => _body.IsTouching(_contactFilter2D);
    private bool _isSliding = false;
    private bool _isHit = false;
    public bool IsMovingForward => _forwardSpeed > 0.0f;
    public Vector2 Velocity => _body.velocity;

    public bool CanMove = true;
        
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
        _contactFilter2D.useNormalAngle = true;
        _contactFilter2D.minNormalAngle = 89;
        _contactFilter2D.maxNormalAngle = 110;
        _standingColliderSize = _boxCollider2D.size;
        _standingColliderOffset = _boxCollider2D.offset;
        _slidingColliderSize = new Vector2(_standingColliderSize.x,_standingColliderSize.y / 2);
        _slidingColliderOffset = new Vector2(_boxCollider2D.offset.x,_boxCollider2D.offset.y * 2);

    }

    //Speed and time variables
    private float _forwardSpeed = 1.1f;
    private float _maxForwardSpeed = 1.1f;
    private float _time = 0;
    private float _slidingTime = 0;
    private bool _isHittingRoof;
    private bool _isSlidingUnderRoof = false; 
    private float _buffTimer = 3f;

    void Update()
    {
        _isHittingRoof = IsHittingRoof();
        if (!CanMove) // TODO Move dead logic to Player Class
        {
            _body.velocity = Vector2.zero;
            return;
        }

        MoveLeftRight();
        MoveUpDown();
    }

    private void MoveLeftRight()
    {
        if (_isSlidingUnderRoof)
        {
            return;
        }

        float _dirX = GameManager.Get().CanReceiveInput ? Input.GetAxisRaw("Horizontal") : 0.0f;
        if (_dirX < 0f)
        {
            if (_forwardSpeed > -0.5f)
            {
                _forwardSpeed += Time.deltaTime * -5f;
            }
            else
            {
                _forwardSpeed = -0.5f;
            }
            if (_forwardSpeed < 0)
            {
                _camera.AllowCameraX(false);
            }
            _isSliding = false;

        }
        else
        {
            
                if (_forwardSpeed < _maxForwardSpeed)
                {
                    _forwardSpeed += Time.deltaTime * _maxForwardSpeed * 5f;
                }
                else if(_forwardSpeed > _maxForwardSpeed)
                {
                    _forwardSpeed -= Time.deltaTime * _maxForwardSpeed * 5f;
                }
                if(Mathf.Abs(_forwardSpeed - _maxForwardSpeed) > 0.01f)
                {
                    _forwardSpeed = _maxForwardSpeed;
                }

                
                _camera.AllowCameraX(true);
            
        }
        
        if (_isHit)
        {
            _time += Time.deltaTime;
            if (_time > _buffTimer)
            {
                _isHit = false;
                _time = 0;
                _maxForwardSpeed = 1.1f;
            }
        }
        _body.velocity = new Vector2(_forwardSpeed * _speed, _body.velocity.y);
        if (!_isSliding)
        {
            ResetCollisions();
        }
    }


    private void MoveUpDown()
    {
        if (GameManager.Get().CanReceiveInput)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && IsGrounded && !_isSlidingUnderRoof)
            {
                _body.velocity = new Vector2(_body.velocity.x, _jumpHeight);
                _isSliding = false;
                CharacterSFXManager.Get().PlayJumpSFX();
            }

            if (Input.GetButtonDown("Slide") && !_isSliding && IsGrounded && _forwardSpeed > 0)
            {
                _isSliding = true;
                _slidingTime = _slidingDuration;
                _boxCollider2D.size = _slidingColliderSize;
                _boxCollider2D.offset = _slidingColliderOffset;
                CharacterSFXManager.Get().PlaySlideSFX();
            }
        }

        if (_isSliding)
        {
            if (_isHittingRoof)
            {
                _slidingTime -= Time.deltaTime;
                _isSlidingUnderRoof = true;
                return;
            }
			
            if (_slidingTime > 0 )
            {
                _slidingTime -= Time.deltaTime;
                _isSlidingUnderRoof = false;
            }
            else
            {
                _isSliding = false;
                _isSlidingUnderRoof = false;
            }
        }

    }
    private bool IsHittingRoof()
    {
        var hitPos1 = new Vector2(transform.position.x-0.1f,transform.position.y- 0.7f);
        var hitPos2 = new Vector2(transform.position.x+0.1f,transform.position.y- 0.7f);
        Debug.DrawLine(hitPos1,hitPos1+ Vector2.up* 0.5f,Color.red);
        Debug.DrawLine(hitPos2,hitPos2+ Vector2.up* 0.5f,Color.red);

        RaycastHit2D hit = Physics2D.Raycast(hitPos1, Vector2.up,0.5f);
        RaycastHit2D hit2 = Physics2D.Raycast(hitPos2, Vector2.up,0.5f);

        return hit || hit2;
    }
	
    private void ResetCollisions()
    {
        _boxCollider2D.size = _standingColliderSize;
        _boxCollider2D.offset = _standingColliderOffset;
    }

    /// <summary>
    /// Modifies speed by a ratio. If it's lower, it will return to normal after _timeOfSlowdown seconds, if it's higher it won't change until it is manually changed.
    /// </summary>
    public void ModifySpeed(float ratio, float timer)
    {
        _maxForwardSpeed = 1.1f * ratio;
        _isHit = true;
        _buffTimer = timer;
    }
}
