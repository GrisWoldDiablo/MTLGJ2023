using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool _leftAllowed = false;

    private Rigidbody2D _body;
    [Header("Movement")]
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _jumpHeight = 14f;
    
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

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        if (dirX < 0f)
        {
            if (_leftAllowed)
            {
                dirX = -0.3f;
                _camera.AllowCameraMovement(false);
            }
            else
            {
                dirX = 0.1f;

            }
        }else if (dirX >= 0f)
        {
            dirX = 1.1f;
            _camera.AllowCameraMovement(true);
        }

        _body.velocity = new Vector2(dirX * _speed, _body.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _body.velocity = new Vector2(_body.velocity.x, _jumpHeight);
        }
    }

}
