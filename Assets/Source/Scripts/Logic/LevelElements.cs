using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Logic.GoalChecker;
using UnityEngine;

namespace Source.Scripts.Logic
{
    public class LevelElements : MonoBehaviour
    {
        [SerializeField] private GoalPlace _goalPlace;
        [SerializeField] private List<Cube> _cubes;

        public GoalPlace GoalPlace => _goalPlace;
        
        public void Reactivate()
        {
            foreach (var cube in _cubes)
                cube.ActivateComponents();
        }

        public void RiseHealth( int count )
        {
            foreach (var cube in _cubes)
                cube.RaiseHealth(count);
        }

        public void HealthDecreases()
        {
            foreach (var cube in _cubes)
                cube.HealthDecreases();
        }

        public void EnableCubesColliders()
        {
            foreach (var cube in _cubes)
                cube.EnableCollider();
        }

        public void DisableCubesColliders()
        {
            foreach (var cube in _cubes)
                cube.DisableCollider();
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            CashCubes();
            _goalPlace = GetComponentInChildren<GoalPlace>();
        }

        private void CashCubes()
        {
            _cubes.Clear();
            _cubes = GetComponentsInChildren<Cube>().ToList();
        }
#endif
    }
}