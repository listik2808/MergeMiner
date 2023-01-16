using Source.Scripts.Logic.GridBehaviour;
using UnityEngine;

namespace Source.Scripts.Logic.Cell
{
    public abstract class CellContent : MonoBehaviour
    {
        [SerializeField] private int _id;
        
        protected GridCell ParentCell;
        
        public int ID => _id;
        
        public void SetParentCell(GridCell gridCell)
        {
            ParentCell = gridCell;
        }
    }
}
