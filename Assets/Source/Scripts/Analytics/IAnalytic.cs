using System.Collections.Generic;

namespace Source.Scripts.Analytics
{
    public interface IAnalytic
    {
        void OnGameInitialize(Dictionary<string, object> dataObjects);
        void OnLevelStart(Dictionary<string, object> dataObjects);
        void OnLevelComplete(Dictionary<string, object> dataObjects);
        void OnLevelFail(Dictionary<string, object> dataObjects);
        void OnLevelRestart(Dictionary<string, object> dataObjects);
        void OnSoftSpent(Dictionary<string, object> dataObjects);
        void OnRegistrationDayIs(Dictionary<string, object> dataObjects);
        void OnSessionCountIs(Dictionary<string, object> dataObjects);
        void OnDaysInGameIs(Dictionary<string, object> dataObjects);
        void OnCurrentSoftHave(Dictionary<string, object> dataObjects);
        void OnContentIsOver(Dictionary<string, object> dataObjects);
        void OnEvent(string eventName, Dictionary<string, object> dataObjects);
        void OnEvent(string eventName);
    }
}