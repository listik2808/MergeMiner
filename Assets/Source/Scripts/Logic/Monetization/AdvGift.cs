using System;
using System.Collections;
//using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Logic.Cell;
using Source.Scripts.Logic.GridBehaviour;
using Source.Scripts.MergedObjects;
using Source.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Logic.Monetization
{
    public class AdvGift : CellContent
    {
        [SerializeField] private Button _button;
        [SerializeField] private AnimationCurve _appearScaleCurve;
        [SerializeField] private AnimationCurve _IdleScaleCurve;
        [SerializeField] [Min(0.01f)] private float _appearDuration = 0.2f;
        [SerializeField] private Transform _content;
        [SerializeField] private AdvSlider _advSlider;
        [SerializeField] [Min(0)] private float _advDuration = 8f;

        private int _contentId;
        private IStaticDataService _staticData;
        private UI.UI _ui;
        private IAnalyticManager _analytic;
        private Coroutine _advShowingAnimation;
        private Coroutine _disappearingAnimation;

        public event Action<GridCell, int> Activated;
        public event Action<GridCell> Disappear; 

        public void Construct(IStaticDataService staticData, UI.UI ui)
        {
            _staticData = staticData;
            _ui = ui;
            _analytic = AllServices.Container.Single<IAnalyticManager>();
            _button.onClick.AddListener(OnButtonClick);
            _ui.MergeMenu.AdvBoxButton.Button.onClick.AddListener(OnButtonClick);
        }
        
        public void Enable() =>
            enabled = true;
    
        public void Disable() =>
            enabled = false;
        
        public void SetContent(int id)
        {
            _contentId = id;
            CellContent cellContentPrefab = _staticData.ForCellContent(_contentId).CellContentPrefab;
            Instantiate(cellContentPrefab, _content);
            _ui.MergeMenu.AdvBoxButton.ExtraRewardIcon.SetIcon(_staticData.ForCellContent(id).CellContentIcon);
            _ui.MergeMenu.AdvBoxButton.Show();
            if (_staticData.ForCellContent(id).CellContentPrefab is MergebleObject)
                _ui.MergeMenu.AdvBoxButton.ExtraRewardIcon.SetShovelGradeText(id);
            
            _analytic.SendEvent(AnalyticNames.AdReward + AnalyticNames.Offer);
            StartCoroutine(Appear());
            _advShowingAnimation = StartCoroutine(AdvShowing());
        }

        private void OnButtonClick()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _ui.MergeMenu.AdvBoxButton.Button.onClick.RemoveListener(OnButtonClick);
            _analytic.SendEvent(AnalyticNames.AdReward + AnalyticNames.Click);
#if UNITY_EDITOR
            GetReward();
#endif
#if YANDEX_GAMES && !UNITY_EDITOR
            VideoAd.Show(
                SetPause,
                null, 
                () =>
            {
                GetReward();
                UnPause();
            },
                (_) =>
                {
                    UnPause();
                });
#endif
        }

        private IEnumerator AdvShowing()
        {
            for (float t = 1; t >= 0; t -= Time.deltaTime / _advDuration)
            {
                _advSlider.SetValue(t);
                _ui.MergeMenu.AdvBoxButton.AdvSlider.SetValue(t);
                _advSlider.transform.localScale = Vector3.one * _IdleScaleCurve.Evaluate(t);
                yield return null;
            }

            _disappearingAnimation = StartCoroutine(Disappearing(()=> Disappear?.Invoke(ParentCell)));
        }

        private IEnumerator Disappearing(Action callback)
        {
            _ui.MergeMenu.AdvBoxButton.Hide();
            for (float t = 1; t >= 0; t -= Time.deltaTime / _appearDuration)
            {
                _content.localScale = Vector3.one * _appearScaleCurve.Evaluate(t);
                yield return null;
            }
            callback?.Invoke();
        }

        private IEnumerator Appear()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / _appearDuration)
            {
                _content.localScale = Vector3.one * _appearScaleCurve.Evaluate(t);
                yield return null;
            }
            _content.localScale = Vector3.one;
        }

        private void GetReward()
        {
            if(_advShowingAnimation != null)
                StopCoroutine(_advShowingAnimation);
            if (_disappearingAnimation != null)
                StopCoroutine(_disappearingAnimation);
            StartCoroutine(Disappearing(() => Activated?.Invoke(ParentCell, _contentId)));
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
    }
}
