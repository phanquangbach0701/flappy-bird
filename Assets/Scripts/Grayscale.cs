using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grayscale : MonoBehaviour, IOnDie
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
    
    IEnumerator GrayscleRoutine(float duration)
    {
        float time = 0;
        while (duration > time)
        {
            float durationFrame = Time.deltaTime;
            float ratio = time / duration;
            float grayAmount = ratio;
            SetGraysacle(grayAmount);
            time += durationFrame;
            yield return null;
        }
        SetGraysacle(1);
    }

    private void SetGraysacle(float grayAmount)
    {
        spriteRenderer.material.SetFloat("_GrayscaleAmount", grayAmount);
    }

    public void OnDie()
    {
        StartCoroutine(GrayscleRoutine(1));
    }
}
