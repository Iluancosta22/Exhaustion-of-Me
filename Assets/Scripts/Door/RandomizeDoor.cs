using UnityEngine;

public class RandomizeDoor : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    public void Randomize(bool hasWall)
    {
        wall.SetActive(hasWall);
    }
}