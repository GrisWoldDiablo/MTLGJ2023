using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 _offset = new (2.0f,2.3f);
    void Update()
    {
        transform.position = new Vector3(player.position.x + _offset.x, player.position.y + _offset.y,transform.position.z);
    }
}
