using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 _offset = new (2.0f,2.3f);
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private bool _allowCameraX = true;

    private Camera _camera; 
    private Vector2 screenBounds;
    private float objectWidth;

    private float centeredY;
    private void Start()
    {
        _camera = GetComponent<Camera>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = _spriteRenderer.bounds.size.x / 2;
        centeredY = transform.position.y;
    }

    void Update()
    {
        float newX = transform.position.x;
        float newY = centeredY;
        float newZ = transform.position.z;


        
        //Add effects in vector3 if needed
        if (_allowCameraX)
        {
            newX = player.position.x + _offset.x;
        }
        if (player.position.y > centeredY + 0.2f)
        {
            newY = player.position.y;
        }

        transform.position = new Vector3(newX, newY, newZ);
    }
    
    void LateUpdate(){
        if (!_allowCameraX)
        {
            //Clamp player movement when camera is not moving
            screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
            
            Vector3 viewPos = player.position;
            viewPos.x = Mathf.Clamp(viewPos.x, transform.position.x - (screenBounds.x- transform.position.x) + objectWidth, screenBounds.x - objectWidth);
            player.position = viewPos;
        }
    }

    private Vector3 tempPlayerPos;
    public void AllowCameraX(bool val)
    {
        if (!val && _allowCameraX)
        {
            tempPlayerPos = player.position;
            _allowCameraX = false;
        }

        if (val && !_allowCameraX)
        {
            if(player.position.x > tempPlayerPos.x)
            {
                tempPlayerPos.y = player.position.y;
                player.position = tempPlayerPos;
                _allowCameraX = true;
            }
        }

    }
    
}
