using Source.Scripts.Logic.Cell;
using UnityEngine;

namespace Source.Scripts.Logic.GridBehaviour
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        public Collider Collider => _collider;
        public bool IsOccupied { get; private set; }
        public CellContent CellContent { get; private set; }

        public void AddObject(CellContent content)
        {
            CellContent = content;
            IsOccupied = true;
        }

        public void RemoveObject()
        {
            CellContent = null;
            IsOccupied = false;
        }
    }
}
