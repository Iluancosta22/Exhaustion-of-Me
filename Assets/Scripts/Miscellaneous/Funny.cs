using System.Collections;
using TMPro;
using UnityEngine;

public class Funny : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float speed = 1f;

    private Coroutine colorCoroutine;

    void OnEnable()
    {
        colorCoroutine = StartCoroutine(RainbowLoop());
    }

    void OnDisable()
    {
        if (colorCoroutine != null)
            StopCoroutine(colorCoroutine);

        text.color = Color.white;
    }

    IEnumerator RainbowLoop()
    {
        float hue = 0f;

        while (true)
        {
            hue = (hue + Time.deltaTime * speed) % 1f;
            text.color = Color.HSVToRGB(hue, 1f, 1f);
            yield return null;
        }
    }
}