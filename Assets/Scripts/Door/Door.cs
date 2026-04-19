using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteract
{
    public Transform door;
    public Transform player;
    public float targetRotation = 120f;
    public float animationDuration = 2f;

    public AudioSource doorAudio;

    private bool open;
    private float closedAngle;
    private float openAngle;
    private Coroutine currentAnimation;

    void OnEnable() => RandomizeManager.OnRandomizeDoors += CloseDoor;
    void OnDisable() => RandomizeManager.OnRandomizeDoors -= CloseDoor;
    
    void Start()
    {
        closedAngle = door.localEulerAngles.y;
        player = FindAnyObjectByType<Player>().transform;
    }

    public void Interact()
    {
        if (door == null) return;

        // Só recalcula a direção ao abrir, não ao fechar
        if (!open)
            openAngle = CalculateOpenAngle();

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(DoorAnimation(!open));
    }

    float CalculateOpenAngle()
    {
        // Direção do player em relação à porta
        Vector3 directionToPlayer = player.position - door.position;

        // Dot positivo = player na frente da porta, negativo = player atrás
        float dot = Vector3.Dot(-door.forward, directionToPlayer);
        bool playerInFront = dot >= 0f;

        return closedAngle + (playerInFront ? targetRotation : -targetRotation);
    }

    public void CloseDoor()
    {
        if(open) StartCoroutine(DoorAnimation(false));
    }

    IEnumerator DoorAnimation(bool opening)
    {
        open = opening;

        float startY = door.localEulerAngles.y;
        float endY = opening ? openAngle : closedAngle;
        float elapsed = 0f;

        if (doorAudio != null)
            doorAudio.Play();

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animationDuration);

            float angle = Mathf.LerpAngle(startY, endY, t);
            door.localEulerAngles = new Vector3(door.localEulerAngles.x, angle, door.localEulerAngles.z);

            yield return null;
        }

        door.localEulerAngles = new Vector3(door.localEulerAngles.x, endY, door.localEulerAngles.z);
        currentAnimation = null;
    }
}