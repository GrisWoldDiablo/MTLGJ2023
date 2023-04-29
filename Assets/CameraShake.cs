using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraShake : MonoBehaviour
{
    bool bIsShaking = false;
    public void DoCameraShake(float shakeDuration, float shakeMagnitude)
    {
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }
    Vector3 shakeTransform = new Vector3();
    private IEnumerator Shake(float shakeDuration, float shakeMagnitude)
    {
        Vector3 initialPosition = transform.localPosition;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

            shakeTransform = new Vector3(x, y, 0f);
            elapsedTime += Time.deltaTime;
            bIsShaking = true;
            transform.localPosition = shakeTransform;
            yield return null;
        }

        bIsShaking = false;
        transform.localPosition = initialPosition;
    }
}
