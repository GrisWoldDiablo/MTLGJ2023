using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D _body;
    [Header("Movement")]
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _jumpHeight = 14f;
    [SerializeField] private float _timeToReachMaxSpeed = 1f;
        
    [Header("Camera")]
    [SerializeField] private CameraMovement _camera; 
    
    [Header("Collisions")]
    [SerializeField] private ContactFilter2D _contactFilter2D;
    private bool _isGrounded => _body.IsTouching(_contactFilter2D);

	public float Speed { get => _speed; set => _speed = value; }
  
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private float _forwardSpeed = 1.1f;
    private float _maxForwardSpeed = 1.1f;
    private float _time = 0;

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        if (dirX < 0f)
        {
            dirX = -0.3f;
            _camera.AllowCameraMovement(false);
        }else if (dirX >= 0f)
        {
            if (_forwardSpeed < _maxForwardSpeed)
            {
                _time += Time.deltaTime;
                if (_time > 3f)
                {
                    _forwardSpeed += Time.deltaTime * (_maxForwardSpeed / _timeToReachMaxSpeed);
                }
            }
            else
            {
                _time = 0;
                _forwardSpeed = _maxForwardSpeed;
            }
            Debug.Log("Speed: " + _forwardSpeed);
            dirX = _forwardSpeed;
            _camera.AllowCameraMovement(true);
        }

        _body.velocity = new Vector2(dirX * _speed, _body.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _body.velocity = new Vector2(_body.velocity.x, _jumpHeight);
        }
    }

    public void SlowDown()
    {
        _forwardSpeed = _maxForwardSpeed / 2;
    }
}
