using UnityEngine;

[CreateAssetMenu(fileName = "SpriteSheet", menuName = "Custom/SpriteSheet")]
public class SpritesSheetScriptable : ScriptableObject
{
	[SerializeField] private Sprite[] _sprites;

	public Sprite GetRandomSprite()
	{
		return _sprites[Random.Range(0, _sprites.Length)];
	}
}