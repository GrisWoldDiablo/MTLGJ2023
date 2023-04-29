using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			PickUp(collision.gameObject.GetComponent<Character>());
		}
	}

    public abstract void PickUp(Character character);
}
