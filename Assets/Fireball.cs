using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rb;
    
    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    int damage = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Character player = other.GetComponent<Character>();

        if(player != null)
        {
            player.ModifyHealth(-damage);
            Destroy(gameObject);
        }

    }
}
