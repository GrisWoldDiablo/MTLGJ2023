using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

//Slice of the environment
public class EnvironmentAsset : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public float GetEnvironmentLength()
    {
        Sprite sprite = GetEnvironmentSprite();
        return sprite.textureRect.width / sprite.pixelsPerUnit;
    }

    public Sprite GetEnvironmentSprite()
    {
        return spriteRenderer.sprite;
    }
}
