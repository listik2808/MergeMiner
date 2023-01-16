using Source.Scripts.Logic.MainCamera;
using UnityEngine;

namespace Source.Scripts.Logic
{
    public class CloudMover : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _speed;

        private void Start()
        {
            Transform targetParent = FindObjectOfType<CameraTracking>().transform;
            var _point = _camera.ViewportToScreenPoint(_camera.transform.position);
            _startPoint.transform.position = new Vector3(_point.x - 30, _startPoint.transform.position.y, _startPoint.transform.position.z);
            _endPoint.transform.position = new Vector3(_point.x + 30, _startPoint.transform.position.y, _startPoint.transform.position.z);
            _startPoint.transform.parent = targetParent;
            _endPoint.transform.parent = targetParent;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPoint.position, _speed * Time.deltaTime);

            if (transform.position == _endPoint.transform.position)
                transform.position = _startPoint.transform.position;
        }
    }
}
