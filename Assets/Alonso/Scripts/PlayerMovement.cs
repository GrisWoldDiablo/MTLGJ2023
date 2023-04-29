using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _body;
    private BoxCollider2D _collider; 
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _jumpHeight = 14f;
    [SerializeField] private ContactFilter2D _contactFilter2D;
    private bool _isGrounded => _body.IsTouching(_contactFilter2D);

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        if (dirX < 0f)
        {
            dirX = 0.1f;
        }else if (dirX >= 0f)
        {
            dirX = 1f;
        }

        _body.velocity = new Vector2(dirX * _speed, _body.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _body.velocity = new Vector3(_body.velocity.x, _jumpHeight, 0);
        }
    }

}
