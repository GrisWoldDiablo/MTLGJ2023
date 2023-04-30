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

    public abstract void PickUp(Character character);
}
