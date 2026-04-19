using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("FootSteps Audio")]
    [SerializeField] private AudioSource Footsteps;
    [SerializeField] private AudioClip walkAudio;
    [SerializeField] private AudioClip runAudio;

    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (player.Walking && !player.Running) WalkAudio();
        else if (player.Walking && player.Running) RunAudio();
        else StopFootsteps();
    }

    #region FootSteps
    public void StopFootsteps()
    {
        Footsteps.Pause();
    }

    public void WalkAudio()
    {
        SetFootstepClip(walkAudio, 1f);
    }

    public void RunAudio()
    {
        SetFootstepClip(runAudio, 1.5f);
    }

    private void SetFootstepClip(AudioClip clip, float pitch)
    {
        if (Footsteps.clip != clip)
        {
            Footsteps.clip = clip;
            Footsteps.Play(); // troca de clip sempre exige Play()
        }

        Footsteps.pitch = pitch;

        if (!Footsteps.isPlaying)
            Footsteps.Play();
    }
    #endregion
}