using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private int _health;

	private bool isDead = false;

	public int Health { get => _health; set => _health = value; }
    public bool IsDead { get => isDead; }

	public PlayerMovement PlayerMovement { get => GetComponent<PlayerMovement>(); }

    void Die()
	{
        //game manager end game
        isDead = true;
    }

	public void ModifyHealth(int damage)
	{
		_health += damage;
		if (_health <= 0)
		{
			Die();
		}
	}

	public void ModifySpeed(float modification)
	{
		PlayerMovement.Speed += modification;
	}
}
