using System.Collections.Generic;
using System.Linq;
using Source.Scripts.MergedObjects;
using UnityEngine;

namespace Source.Scripts.Logic.MainCamera
{
    public class CameraTracking : MonoBehaviour
    {
        [SerializeField] private UI.UI _ui;
        [SerializeField] [Min(0)] private float _lerpSpeed = 4;
    
        private IReadOnlyList<MergebleObject> _mergeableObjects;

        private float _shovelYPosition = float.MaxValue;
        private int _currentShovelsCount;
        private bool _isTracking;

        private Transform _defaultPoint;
        private Transform _finishPoint;
        private Transform _shovelTrackPoint;
        
        private Transform _currentTrackPoint;

        public UI.UI UI => _ui;

        private void Awake()
        {
            _shovelTrackPoint = new GameObject("ShovelTrackPoint").transform;
            _currentTrackPoint = _shovelTrackPoint;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _currentTrackPoint.transform.position, _lerpSpeed + Time.deltaTime);

            if(_isTracking == false)
                return;
        
            var activeShovels = _mergeableObjects.Where(shovel => shovel.gameObject.activeSelf).ToList();

            if (activeShovels.Count == 0)
                return;

            _shovelTrackPoint.transform.position = new Vector3(0, FindPositionY(activeShovels), 0);
        }

        public void Initialize(Transform defaultPoint, Transform finishPoint)
        {
            _defaultPoint = defaultPoint;
            _finishPoint = finishPoint;
            _currentTrackPoint = _defaultPoint;
        } 

        public void StartTrack(IReadOnlyList<MergebleObject> mergeableObjects)
        {
            _mergeableObjects = mergeableObjects;
            _currentTrackPoint = _shovelTrackPoint;
            _isTracking = true;
        }

        public void MoveToDefaultPosition()
        {
            _isTracking = false;
            _currentTrackPoint = _defaultPoint;
            _shovelYPosition = float.MaxValue;
        }

        public void MoveToFinishPosition()
        {
            _isTracking = false;
            _currentTrackPoint = _finishPoint;
        }

        private float FindPositionY(IReadOnlyCollection<MergebleObject> activeShovels)
        {
            if (_currentShovelsCount != activeShovels.Count)
            {
                _currentShovelsCount = activeShovels.Count;
                _shovelYPosition = float.MaxValue;
            }
        
            foreach (var mergeableObject in activeShovels.Where(mergeableObject => mergeableObject.transform.position.y < _shovelYPosition))
                _shovelYPosition = mergeableObject.transform.position.y;

            return _shovelYPosition;
        }
    }
}
