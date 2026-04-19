using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeManager : MonoBehaviour
{
    [Header("Randomização")]
    [SerializeField] private int seed = 42;
    [Tooltip("Se verdadeiro, gera um seed aleatório a cada execução")]
    [SerializeField] private bool randomSeed = false;
    [Tooltip("Porcentagem de portas que ficarão com parede (0 = nenhuma, 100 = todas)")]
    [SerializeField, Range(0, 100)] private int wallChance = 30;

    public float timeToRandomize = 60f;
    public float t = 0;

    [Header("Audio")]
    [SerializeField] private AudioSource randomizeWarning;
    private bool inicialized;

    [SerializeField] public RandomizeDoor[] doors;
    public static event Action OnRandomizeDoors;

    void Start()
    {
        StartCoroutine(StartRandomize());
    }

    void Update()
    {
        t += Time.deltaTime;

        if (t >= timeToRandomize)
        {
            StartCoroutine(StartRandomize());
            t = 0f;
        }
    }

    IEnumerator StartRandomize()
    {
        OnRandomizeDoors?.Invoke();
        yield return new WaitForSeconds(2f);
        Randomize();
        inicialized = true;
    }

    public void Randomize()
    {
        if(randomizeWarning != null && inicialized)
        {
            randomizeWarning.Play();
        }

        // Coleta todos os RandomizeDoor da cena
        doors = FindObjectsByType<RandomizeDoor>(FindObjectsSortMode.None);

        if (doors.Length == 0) return;

        // Embaralha com Fisher-Yates usando o seed controlado
        List<RandomizeDoor> shuffled = Shuffle(doors);

        // Metade exata com wall ativa, metade sem
        int half = Mathf.RoundToInt(shuffled.Count * (wallChance / 100f));

        for (int i = 0; i < shuffled.Count; i++)
            shuffled[i].Randomize(i < half);
    }

    List<RandomizeDoor> Shuffle(RandomizeDoor[] array)
    {
        int usedSeed = randomSeed ? UnityEngine.Random.Range(int.MinValue, int.MaxValue) : seed;
        UnityEngine.Random.State previousState = UnityEngine.Random.state;
        UnityEngine.Random.InitState(usedSeed);

        List<RandomizeDoor> list = new List<RandomizeDoor>(array);

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        // Restaura o estado anterior para não interferir em outros sistemas
        UnityEngine.Random.state = previousState;

        return list;
    }
}