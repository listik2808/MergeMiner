using Source.Scripts.Analytics;
using Source.Scripts.SaveSystem;
using UnityEngine;

namespace Source.Scripts.Infrastructure.States
{
    public class ApplicationPauseState : IState
    {
        private readonly IStorage _storage;
        private readonly IAnalyticManager _analytic;

        public ApplicationPauseState(IStorage storage, IAnalyticManager analytic)
        {
            _storage = storage;
            _analytic = analytic;
        }

        public void Enter()
        {
            Debug.Log("Pause");
            
            SaveAndSendAnalytic();
        }

        public void Exit()
        {
        }

        private void SaveAndSendAnalytic()
        {
#if UNITY_WEBGL
            _storage.SaveRemote();
#else
            Storage.Save();
#endif
            _analytic.SendEventOnGameExit(
                _storage.GetRegistrationDate().ToString(),
                _storage.GetSessionCount(),
                _storage.GetNumberDaysAfterRegistration(),
                _storage.GetSoft());
        }
    }
}