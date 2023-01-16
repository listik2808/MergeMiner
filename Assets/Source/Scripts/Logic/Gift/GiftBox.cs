using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Source.Scripts.Logic.Gift
{
    [SelectionBase]
    public class GiftBox : MonoBehaviour
    {
        [SerializeField] [Min(1001)] private int _giftId = 1001;

        private GridBehaviour.Grid _grid;

        private void Awake() =>
            _grid = AllServices.Container.Single<IGameFactory>().Grid;
        
        public void Open()
        {
            _grid.AddContent(_giftId);
            gameObject.SetActive(false);
        }
    }
}