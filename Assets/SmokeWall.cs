using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeWall : MonoBehaviour
{
    int[] numWallSections = new int[3];

    [SerializeField]
    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move wall towards the player at a constant rate
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }



}
