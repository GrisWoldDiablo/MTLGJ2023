using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMissleBehaviour : MonoBehaviour
{

    public float speed = 5.0f;
    private void Start()
    {
       
    }

    private void Update()
    {
        //move left
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 10;

        if (transform.position.x < screenLeft)
        {
            Destroy(gameObject);
        }
    }
    
}
