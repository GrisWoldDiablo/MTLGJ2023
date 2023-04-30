using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    BoxCollider2D col;

    bool bHasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character player = collision.GetComponentInParent<Character>();
        if (player != null && !bHasCollided)
        {
            bHasCollided = true;
            //damage the player
            player.ModifyHealth(-1);
        }
    }
}
