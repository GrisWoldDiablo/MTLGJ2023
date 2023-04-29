using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] private float _speed = 7f;

    [SerializeField] private float _jumpHeight = 14f;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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

        body.velocity = new Vector2(dirX * _speed, body.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = new Vector3(body.velocity.x, _jumpHeight, 0);
        }
    }
}
