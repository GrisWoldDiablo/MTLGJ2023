using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PingPongAnimation : MonoBehaviour
{

    public Sprite sprite1;
    public Sprite sprite2;
    public float animationSpeed = 0.5f;

    private Sprite startingSprite;
    private Sprite otherSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        int starting = Random.Range(0, 2);

        if(starting == 0)
        {
            startingSprite = sprite1;
            otherSprite = sprite2;
        }
        else
        {
            startingSprite = sprite2;
            otherSprite = sprite1; 
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Animate());


    }

    IEnumerator Animate()
    {
        while (true)
        {
            spriteRenderer.sprite = startingSprite;
            yield return new WaitForSeconds(animationSpeed);
            spriteRenderer.sprite = otherSprite;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
