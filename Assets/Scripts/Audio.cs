using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        _audio.Play();
        Pause();
    }

    public void UnPause()
    {
        _audio.UnPause();
        IsPlaying = true;
    }

    public void Pause()
    {
        _audio.Pause();
        IsPlaying = false;
    }
}