using System.Collections;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    public Light myLight;

    [Header("Emission Material")]
    public Renderer emissionRenderer;
    public Color emissionColor = Color.white; // defina direto aqui a cor que quer
    
    [Tooltip("Deixe vazio para usar '_EmissionColor' padrão")]
    public string emissionProperty = "_EmissionColor";
    private Color _baseEmissionColor;

    [Header("Intensidade")]
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;

    [Header("Timing")]
    public float minOnTime  = 0.05f;
    public float maxOnTime  = 0.4f;
    public float minOffTime = 0.02f;
    public float maxOffTime = 0.25f;

    [Header("Burst")]
    [Range(0f, 1f)]
    public float burstChance = 0.3f;
    public int   burstMin    = 2;
    public int   burstMax    = 5;

    void OnEnable()  => StartCoroutine(Flicker());
    void OnDisable() => StopAllCoroutines();

    void Start()
    {
        if (emissionRenderer != null)
            emissionRenderer.material.EnableKeyword("_EMISSION");
    }

    void SetIntensity(float lightIntensity, float emissionT)
    {
        myLight.intensity = lightIntensity;

        if (emissionRenderer != null)
        {
            // No URP o valor HDR é controlado multiplicando a cor pela intensidade
            emissionRenderer.material.SetColor("_EmissionColor", emissionColor * emissionT);
        }
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            if (Random.value < burstChance)
            {
                int count = Random.Range(burstMin, burstMax + 1);
                for (int i = 0; i < count; i++)
                {
                    float t = Random.Range(0f, 0.6f);
                    SetIntensity(Random.Range(minIntensity, maxIntensity * 0.6f), t);
                    yield return new WaitForSeconds(Random.Range(minOnTime * 0.3f, maxOnTime * 0.5f));

                    SetIntensity(0f, 0f);
                    yield return new WaitForSeconds(Random.Range(minOffTime, minOffTime * 3f));
                }
            }

            float intensity = Random.Range(minIntensity, maxIntensity);
            // Normaliza o t de 0–1 baseado na intensidade relativa ao máximo
            float emissionNormalized = Mathf.InverseLerp(minIntensity, maxIntensity, intensity);
            SetIntensity(intensity, emissionNormalized);
            yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));

            SetIntensity(0f, 0f);
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));
        }
    }
}