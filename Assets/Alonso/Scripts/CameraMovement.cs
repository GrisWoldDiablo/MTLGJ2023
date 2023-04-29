using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 _offset = new (2.0f,2.3f);
    private bool _allowCameraMovement = true;
    private Camera _camera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;


    private void Start()
    {
        _camera = GetComponent<Camera>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = player.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void Update()
    {
        //Add effects in vector3 if needed
        if (_allowCameraMovement)
        {
            transform.position = new Vector3(player.position.x + _offset.x, player.position.y + _offset.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, player.position.y + _offset.y, transform.position.z);

        }
    }
    
    void LateUpdate(){
        if (!_allowCameraMovement)
        {
            //Clamp player movement when camera is not moving
            screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
            
            Vector3 viewPos = player.position;
            viewPos.x = Mathf.Clamp(viewPos.x, transform.position.x - (screenBounds.x- transform.position.x) + objectWidth, screenBounds.x - objectWidth);
            player.position = viewPos;
        }
    }

    private Vector3 tempPlayerPos;
    public void AllowCameraMovement(bool val)
    {
        if (!val && _allowCameraMovement)
        {
            tempPlayerPos = player.position;
            _allowCameraMovement = false;
        }

        if (val && !_allowCameraMovement)
        {
            if(player.position.x > tempPlayerPos.x)
            {
                tempPlayerPos.y = player.position.y;
                player.position = tempPlayerPos;
                _allowCameraMovement = true;
            }
        }

    }

}
