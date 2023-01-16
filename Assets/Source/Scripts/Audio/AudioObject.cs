using UnityEngine;

public class AudioObject : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    private void Awake()
    {
        _audio.Play();
        _audio.Pause();
    }

    private void Start() => 
        DontDestroyOnLoad(this);

    public void Pause() => 
        _audio.Pause();

    public void UnPause() =>
        _audio.UnPause();
}