//using Agava.WebUtility;
using Source.Scripts.SaveSystem;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    public void OnInBackgroundChange(bool inBackground)
    {
        if(PlayerPrefs.HasKey(DataNames.IsAdShowing)) 
            if(PlayerPrefs.GetInt(DataNames.IsAdShowing).ToBool())
                return;
        
        Time.timeScale = inBackground ? 0.0f : 1.0f;
        AudioListener.pause = inBackground;
        AudioListener.volume = inBackground ? 0f : 1f;
    }
}
