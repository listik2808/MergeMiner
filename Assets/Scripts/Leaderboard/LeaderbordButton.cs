using Assets.Source.UI.Leaderboards;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class LeaderbordButton : MonoBehaviour
{
    [SerializeField] private LeaderboardView _leaderboardView;

    private Button _button;
    private bool _canUpdate = true;


    private void Awake()
    {
        _button = GetComponent<Button>();
        if(AllServices.Container.Single<IStorage>().GetInt(DataNames.IsTutorialComplete).ToBool() == false)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    public void OnDisappeared()
    {
        SwitchButtonInteractable(true);
        Time.timeScale = 1.0f;
    }

    private void OnAppeared()
    {
        SwitchButtonInteractable(true);
        Time.timeScale = 0f;
    }

    private void OnCompleted()
    {
        ChangeUpdateState(true);
    }

    private void OnStarted()
    {
        ChangeUpdateState(false);
    }

    private void OnButtonClick()
    {
        SwitchButtonInteractable(false);
        OnAppeared();
        _leaderboardView.gameObject.SetActive(true);
        // _leaderboardView.ClearEntry();
        // _leaderboardView.TryShowChallengers();
        // _leaderboardView.AddChallengers();
    }

    private void ChangeUpdateState(bool value)
    {
        _canUpdate = value;
    }

    private void SwitchButtonInteractable(bool value)
    {
        _button.interactable = value;
    }
}
