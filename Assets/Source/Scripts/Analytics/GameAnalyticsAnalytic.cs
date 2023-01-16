#if GAME_ANALYTICS
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

namespace Source.Scripts.Analytics
{
    public class GameAnalyticsAnalytic : IAnalytic
    {
        public GameAnalyticsAnalytic()
        {
            GameAnalytics.Initialize();
        }

        public void OnGameInitialize(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, AnalyticNames.GameStart + dataObjects[AnalyticNames.Count]);
        }

        public void OnLevelStart(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, AnalyticNames.Level + dataObjects[AnalyticNames.Level]);
        }

        public void OnLevelComplete(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, AnalyticNames.Level + dataObjects[AnalyticNames.Level], dataObjects);
        }

        public void OnLevelFail(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, AnalyticNames.Fail + dataObjects[AnalyticNames.Level], dataObjects);
        }

        public void OnLevelRestart(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, AnalyticNames.Restart + AnalyticNames.Level + dataObjects[AnalyticNames.Level]);
        }

        public void OnSoftSpent(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, AnalyticNames.SoftSpent, dataObjects);
        }

        public void OnRegistrationDayIs(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, AnalyticNames.RegistrationDay,
                dataObjects);
        }

        public void OnSessionCountIs(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, AnalyticNames.SessionCount,
                dataObjects);
        }

        public void OnDaysInGameIs(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, AnalyticNames.DaysInGame,
                dataObjects);
        }

        public void OnCurrentSoftHave(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, AnalyticNames.CurrentSoft,
                dataObjects);
        }

        public void OnContentIsOver(Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, AnalyticNames.ContentIsOver, 
                dataObjects);
        }

        public void OnEvent(string eventName, Dictionary<string, object> dataObjects)
        {
            GameAnalytics.NewDesignEvent(eventName, dataObjects);
        }

        public void OnEvent(string eventName)
        {
            GameAnalytics.NewDesignEvent(eventName);
        }
    }
}
#endif
