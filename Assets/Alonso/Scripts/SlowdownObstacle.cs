using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownObstacle : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float _slowdownRatio = 0.5f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().ModifySpeed(_slowdownRatio, 2f);
            Destroy(gameObject);
        }
    }
}
