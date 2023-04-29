using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

//Slice of the environment
public class EnvironmentAsset : MonoBehaviour
{
	//split the sprite up into positions that can be selected randomly to spawn obstacles at
	private Transform[] obstaclePositions;

	private SpriteRenderer spriteRenderer;

	private ProceduralEnvGenerator envGenerator;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		envGenerator = ProceduralEnvGenerator.Get();
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

	void Update()
	{
		// Check if any tiles have moved off the left side of the screen
		float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 10;

		if (transform.position.x < screenLeft)
		{
			envGenerator.IncrementExpired();
			Destroy(gameObject);
		}

	}
}