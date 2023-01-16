using System.Collections;
using DG.Tweening;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Logic.Cell;
using Source.Scripts.MergedObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class RewardMenu : MenuBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Header _header;
        [SerializeField] private Button _backToPlayButton;
        [SerializeField] private Button _rewardButton;
        [SerializeField] [Min(0)] private float _appearDuration = 0.05f;
        [SerializeField] private Transform _newToolPlace;
        [SerializeField] private Transform _extraRewardContainer;
        [SerializeField] private ExtraRewardIcon _extraRewardIconPrefab;
        
        private Coroutine _rotateRoutine;
        private CellContent _newTool;
        private Sequence _sequence;
        private IStaticDataService _staticData;
        
        public Button BackToPlayButton => _backToPlayButton;
        public Button RewardButton => _rewardButton;
        public Header Header => _header;

        private void Awake() => 
            base.Hide();

        public void Initialize(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public override void Show(int id)
        {
            base.Show(id);
            _canvasGroup.alpha = 0;

            _sequence = DOTween.Sequence();

            _sequence.Append(_canvasGroup.DOFade(1, _appearDuration));
            _newTool = Instantiate(_staticData.ForCellContent(id).CellContentPrefab, GetPosition(), Quaternion.identity);
            _newTool.transform.localScale = Vector3.one * 2;
            _newTool.GetComponent<Collider>().enabled = false;
            _newTool.GetComponent<MergebleObject>()?.StopInteractable();
            SetLayer(_newTool.gameObject);
            _rotateRoutine = StartCoroutine(Rotate(_newTool.transform));
            ClearContainer();
            ShowExtraReward(id);
        }

        public override void Hide()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_canvasGroup.DOFade(0, _appearDuration)).OnComplete(base.Hide);
            StopCoroutine(_rotateRoutine);
            Destroy(_newTool.gameObject);
        }

        private void SetLayer(GameObject newToolGO)
        {
            int uILayer = LayerMask.NameToLayer("UI");
            
            foreach (var gOTransform in newToolGO.GetComponentsInChildren<Transform>())
                gOTransform.gameObject.layer = uILayer;
        }

        private Vector3 GetPosition()
        {
            float zOffset = -0.5f;
            return new Vector3(_newToolPlace.position.x, _newToolPlace.position.y, _newToolPlace.position.z + zOffset);
        }

        private IEnumerator Rotate(Transform transform)
        {
            float speed = 30;
            
            while (true)
            {
                transform.Rotate(new Vector3(0, speed,0) * Time.deltaTime);
                yield return null;
            }
        }

        private void ClearContainer()
        {
            if (_extraRewardContainer.transform.childCount > 0)
                foreach (Transform child in _extraRewardContainer.transform)
                    Destroy(child.gameObject);
        }

        private void ShowExtraReward(int rewardId)
        {
            int[] rewardIds = _staticData.ForShovelReward(rewardId).СellContentIds;

            foreach (int id in rewardIds)
            {
                ExtraRewardIcon icon = Instantiate(_extraRewardIconPrefab, _extraRewardContainer);
                icon.SetIcon(_staticData.ForCellContent(id).CellContentIcon);
                
                if(_staticData.ForCellContent(id).CellContentPrefab is MergebleObject)
                    icon.SetShovelGradeText(id);
            }
        }
    }
}