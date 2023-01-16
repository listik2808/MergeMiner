using Assets.Source.UI.Leaderboards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderbordeClouseButton : MonoBehaviour
{
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private LeaderbordButton _bordButton;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ButtonClouseBackground);
    }

    private void OnDisable()
    {
        _button?.onClick.RemoveListener(ButtonClouseBackground);
    }

    private void ButtonClouseBackground()
    {
        _leaderboardView.gameObject.SetActive(false);
        _bordButton.OnDisappeared();
    }
}
