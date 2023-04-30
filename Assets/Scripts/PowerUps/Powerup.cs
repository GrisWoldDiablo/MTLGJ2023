using System;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			PickUp(collision.gameObject.GetComponentInParent<Character>());
		}
	}

    void Update()
    {
	    float y = Mathf.PingPong(Time.time, 0.5f);
	    gameObject.transform.localPosition = new Vector3(0, 
													 y,
													0);
    }

    public abstract void PickUp(Character character);
}
