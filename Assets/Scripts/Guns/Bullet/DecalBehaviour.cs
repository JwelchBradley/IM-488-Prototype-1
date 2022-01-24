using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalBehaviour : MonoBehaviour
{
    public void StartFadeOut(float lifeTime, float timeBeforeDespawnStart)
    {
        StartCoroutine(FadeOut(lifeTime, timeBeforeDespawnStart));
    }

    public IEnumerator FadeOut(float lifeTime, float timeBeforeDespawnStart)
    {
        yield return new WaitForSeconds(timeBeforeDespawnStart);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float t = lifeTime - timeBeforeDespawnStart;
        float originalT = t;

        while (t > 0)
        {
            float a = sr.color.a;
            t -= Time.fixedDeltaTime;
            a = Mathf.Lerp(0, 1, t / originalT);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
            yield return new WaitForFixedUpdate();
        }
    }
}
