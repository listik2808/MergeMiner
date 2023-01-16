using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private Image _imageStop;
    [SerializeField] private Image _imagePlay;
    [SerializeField] private Button _buttonClick;

    private IStorage _storage;
    private IGameFactory _factory;

    public void Initialize(IStorage storage, IGameFactory factory)
    {
        _storage = storage;
        _factory = factory;
    } 

    private void OnEnable() =>
        _buttonClick.onClick.AddListener(OnClicked);

    private void OnDisable() =>
        _buttonClick.onClick.RemoveListener(OnClicked);

    public void PlayAudio()
    {
        _factory.AudioObject.UnPause();
        _storage.SetInt(DataNames.IsMusicPlaying, true.ToInt());
        SetButtonView(true);
    }

    public void StopAudio()
    {
        _factory.AudioObject.Pause();
        _storage.SetInt(DataNames.IsMusicPlaying, false.ToInt());
        SetButtonView(false);
    }
    
    private void OnClicked()
    {
        if(_storage.GetInt(DataNames.IsMusicPlaying).ToBool())
            StopAudio();
        else
            PlayAudio();
    }

    private void SetButtonView(bool isPlaying)
    {
        _imagePlay.gameObject.SetActive(isPlaying);
        _imageStop.gameObject.SetActive(!isPlaying);
    }
}