using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteract
{
    public Transform door;
    public bool reverseDirection;
    public float targetRotation;
    public float animationDuration = 2f;

    public AudioSource doorAudio;

    private bool open;
    private float closedAngle;
    private float openAngle;
    private Coroutine currentAnimation;

    void Start()
    {
        closedAngle = door.localEulerAngles.y;
        openAngle = closedAngle + (reverseDirection ? -targetRotation : targetRotation);
    }

    public void Interact()
    {
        if (door == null) return;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(DoorAnimation(!open));
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