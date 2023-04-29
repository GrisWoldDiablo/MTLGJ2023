using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeWall : MonoBehaviour
{
    [SerializeField]
    private Character player;

    private ParticleSystem ps;

    int[] numWallSections = new int[3];

    [SerializeField]
    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && player.IsDead) {

            // Get the main module of the particle system
            return;
        }
        //move wall towards the player at a constant rate
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Character player = other.GetComponent<Character>();

        if (player != null)
        {
            player.ModifyHealth(-10);

        }
    }



}
