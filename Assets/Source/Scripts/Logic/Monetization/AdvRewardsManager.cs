using System.Collections;
using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.States;
using Source.Scripts.Logic.Cell;
using Source.Scripts.SaveSystem;
using Source.Scripts.UI;
using UnityEngine;
using Grid = Source.Scripts.Logic.GridBehaviour.Grid;
using Random = UnityEngine.Random;

namespace Source.Scripts.Logic.Monetization
{
    public class AdvRewardsManager : MonoBehaviour
    {
        [SerializeField] private SkipLevelForAdvButton _skipLevelForAdvButton;
        
        private IGameFactory _factory;
        private IStorage _storage;
        private Coroutine _advGiftCoroutine;
        private IGameStateMachine _stateMachine;
        private IAnalyticManager _analytic;

        private void Awake()
        {
            _factory = AllServices.Container.Single<IGameFactory>();
            _storage = AllServices.Container.Single<IStorage>();
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _analytic = AllServices.Container.Single<IAnalyticManager>();
            _skipLevelForAdvButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _skipLevelForAdvButton.Button.onClick.AddListener(OnButtonClick);
            if(_storage.GetInt(DataNames.IsTutorialComplete).ToBool())
                _advGiftCoroutine = StartCoroutine(SpawnAdvGift());

            if (_storage.GetInt(DataNames.BadAttemptsNumber) >= 2)
            {
                _skipLevelForAdvButton.gameObject.SetActive(true);
                _analytic.SendEvent(AnalyticNames.SkipLevelForAd + AnalyticNames.Offer);
            }
        }

        private void OnDisable()
        {
            _skipLevelForAdvButton.Button.onClick.RemoveListener(OnButtonClick);
            if(_advGiftCoroutine != null)
                StopCoroutine(_advGiftCoroutine);
        }

        private void OnButtonClick()
        {
#if UNITY_EDITOR
            GoToLoadNextLevelState();
#endif
#if YANDEX_GAMES && !UNITY_EDITOR
            _storage.SetInt(DataNames.IsVideoAdShown, true.ToInt());
            VideoAd.Show(
                SetPause,
                null, 
                () =>
                {
                    UnPause();
                    GoToLoadNextLevelState();
                },
                (a) =>
                {
                    UnPause();
                });
#endif
        }
        
        private void GoToLoadNextLevelState()
        {
            _storage.SetInt(DataNames.BadAttemptsNumber, 0);
            _analytic.SendEvent(AnalyticNames.SkipLevelForAd + AnalyticNames.Click);
            _stateMachine.Enter<LoadNextLevelState>();
        }
        
        private void SetPause()
        {
            PlayerPrefs.SetInt(DataNames.IsAdShowing, true.ToInt());
            Time.timeScale = 0;
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }
        
        private void UnPause()
        {
            PlayerPrefs.SetInt(DataNames.IsAdShowing, false.ToInt());
            Time.timeScale = 1;
            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }

        private IEnumerator SpawnAdvGift()
        {
            yield return new WaitForSeconds(15f);
            CellContent cellContent = _factory.Grid.AddContent(1100);
            
            if (cellContent is AdvGift advGift)
            {
                int id = Random.Range(-3, 0);
                id += _storage.GetInt(DataNames.MaxMergedShovelLevel);
                id = Mathf.Clamp(id, 1, _storage.GetInt(DataNames.MaxMergedShovelLevel));
                advGift.SetContent(id);
            }
        }
    }
}
