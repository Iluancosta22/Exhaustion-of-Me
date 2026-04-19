using System.Collections;
using UnityEngine;

public class JumpScareAudio : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private AudioClip[] scareClips;

    [Header("Timing (segundos)")]
    [SerializeField] private float minTime = 10f;
    [SerializeField] private float maxTime = 30f;

    [Header("Distância")]
    [SerializeField] private float minDistance = 10;
    [SerializeField] private float maxDistance = 50;

    private AudioSource activeSource;

    void OnEnable()  => StartCoroutine(ScareLoop());
    void OnDisable() => StopAllCoroutines();

    IEnumerator ScareLoop()
    {
        while (true)
        {
            // Espera o clip atual terminar antes de qualquer coisa
            if (activeSource != null)
                yield return new WaitUntil(() => !activeSource.isPlaying);

            // Só então sorteia o tempo de espera
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            if (scareClips.Length == 0 || audioSourcePrefab == null) continue;

            SpawnSound();
        }
    }

    void SpawnSound()
    {
        AudioClip clip = scareClips[Random.Range(0, scareClips.Length)];
        Vector3 pos    = GetRandomPosition();

        GameObject obj = Instantiate(audioSourcePrefab, pos, Quaternion.identity);
        activeSource   = obj.GetComponent<AudioSource>();

        activeSource.clip = clip;
        activeSource.Play();

        Destroy(obj, clip.length + 0.5f);
    }

    Vector3 GetRandomPosition()
    {
        float angle    = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(minDistance, maxDistance);

        return new Vector3(
            transform.position.x + Mathf.Cos(angle) * distance,
            transform.position.y,
            transform.position.z + Mathf.Sin(angle) * distance
        );
    }
}