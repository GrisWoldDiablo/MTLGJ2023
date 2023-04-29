using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
	[SerializeField] private SpritesSheetScriptable _spritesSheetScriptable;

	private void Awake()
	{
		var spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = _spritesSheetScriptable.GetRandomSprite();
	}
}