using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMissleBehaviour : MonoBehaviour
{

    public float speed = 5.0f;
    private void Start()
    {
        //set y random from camera
        float screenHeight = Camera.main.orthographicSize * 2f; 
        float screenWidth = screenHeight * Screen.width / Screen.height;

        float edgeX = transform.position.x + screenWidth / 2f;

        float screenHeightWithoutTopThird = screenHeight * (2f / 3f); // get the height of the bottom 2/3rd of the screen

        float minY = -screenHeightWithoutTopThird / 2f; // get the minimum Y position
        float maxY = screenHeightWithoutTopThird / 2f; // get the maximum Y position

        float randomY = Random.Range(minY, maxY); // get a random Y position within the bottom 2/3rd of the screen

        transform.position = new Vector3(edgeX, randomY, transform.position.z); // set the new position of the object
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
