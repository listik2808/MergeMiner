using Agava.YandexGames;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.UI.Leaderboards
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private ChallengerView _template;
        [SerializeField] private GameObject _container;
        [SerializeField] private AutorizationRequest _request;
        [SerializeField][Range(0, 10)] private float _scrollTime;

        private const string UnknownPlayerNameRu = "Неизвестный игрок";
        private const string UnknownPlayerNameEn = "Unknown player";
        private const string UnknownPlayerNameTr = "Bilinmeyen oyuncu";
        private const float ResetDelay = 0.4f;
        private ScrollRect _scrollRect;
        private bool _canScroll = true;
        private bool _isAnimationStopped;
        private float _verticalLastPosition = 1f;
        private float _verticalStartPosition = 0f;
        private string _playerID;
        private string _playerName;

        private void Awake()
        {
            _scrollRect = GetComponentInChildren<ScrollRect>();
            ResetScrollRectPosition();
        }

        private void OnEnable()
        {
            ClearEntry();
            TryShowChallengers();
            StartCoroutine(ResetCoroutine());
        }

        private void OnDisable()
        {
            _canScroll = true;
            ResetScrollRectPosition();
        }

        private void Update()
        {
            if (_isAnimationStopped)
                StartCoroutine(SlideCoroutine(_scrollTime));
        }

        private IEnumerator SlideCoroutine(float time)
        {
            float Timer = 0;

            if (_canScroll)
                while (_scrollRect.verticalNormalizedPosition < _verticalLastPosition)
                {
                    _scrollRect.verticalNormalizedPosition = Mathf.Lerp(_verticalStartPosition, _verticalLastPosition, Timer / time);
                    yield return null;
                    Timer += Time.deltaTime;
                }

            _canScroll = false;
            Debug.Log(_canScroll);

        }

        private IEnumerator ResetCoroutine()
        {
            yield return new WaitForSeconds(ResetDelay);
            ResetScrollRectPosition();
        }

        private void GetPlayerInfo()
        {
            PlayerAccount.GetProfileData((result) =>
            {
                _playerID = result.uniqueID;
                _playerName = result.publicName;
            });
        }

        private void ResetScrollRectPosition()
        {
            _scrollRect.verticalNormalizedPosition = _verticalStartPosition;
        }

        public void AddChallengers()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            GetPlayerInfo();

            Leaderboard.GetEntries(LeaderboardName.Name, (result) =>
            {
                foreach (var entry in result.entries)
                {
                    var view = Instantiate(_template, _container.transform);

                    if (string.IsNullOrEmpty(entry.player.publicName))
                    {
                        //view.SetName(UnknownPlayerNameRu);
                        view.SetName(TranslateIntoLocalLanguage());
                    }
                    else
                        view.SetName(entry.player.publicName);

                    view.SetScore(entry.score);
                }
            });
#endif
        }

        public void ClearEntry()
        {
            if (_container.transform.childCount > 0)
                foreach (Transform child in _container.transform)
                    Destroy(child.gameObject);
        }

        public void OnStartAnimation()
        {
            _isAnimationStopped = false;
        }

        public void OnEndAnimation()
        {
            _isAnimationStopped = true;
        }

        public void TryShowChallengers()
        {
#if (!UNITY_EDITOR && UNITY_WEBGL)
            if (PlayerAccount.IsAuthorized)
            {
                _request.gameObject.SetActive(false);
                AddChallengers();
            }
            else
            {
                _request.gameObject.SetActive(true);
            }
#endif
        }
        private string TranslateIntoLocalLanguage()
        {
            if (YandexGamesSdk.Environment.i18n.lang == "tr")
            {
                return UnknownPlayerNameTr;
            }
            else if (YandexGamesSdk.Environment.i18n.lang == "ru")
            {
                return UnknownPlayerNameRu;
            }
            else if(YandexGamesSdk.Environment.i18n.lang == "en")
            {
                return UnknownPlayerNameEn;
            }

            return null;
        }
    }
}
