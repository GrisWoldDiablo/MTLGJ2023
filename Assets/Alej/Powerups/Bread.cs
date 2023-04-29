public class Bread : Powerup
{
	public override void PickUp(Character character)
	{
		character.ModifyHealth(1);
	}
}
