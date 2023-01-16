#if YANDEX_METRICA
using System.Collections.Generic;
using Agava.YandexMetrica;
using UnityEngine;

namespace Source.Scripts.Analytics
{
    public class YandexMetricaAnalytic : IAnalytic
    {
        public void OnGameInitialize(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.GameStart, dataObjects);
        }

        public void OnLevelStart(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.LevelStart, dataObjects);
        }

        public void OnLevelComplete(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send($"level{dataObjects[AnalyticNames.Level]}Complete");
        }

        public void OnLevelFail(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send($"level{dataObjects[AnalyticNames.Level]}Fail");
        }

        public void OnLevelRestart(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.RegistrationDay, dataObjects);
        }

        public void OnSoftSpent(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.SoftSpent, dataObjects);
        }

        public void OnRegistrationDayIs(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.RegistrationDay, dataObjects);
        }

        public void OnSessionCountIs(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.SessionCount, dataObjects);
        }

        public void OnDaysInGameIs(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.DaysInGame, dataObjects);
        }

        public void OnCurrentSoftHave(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.CurrentSoft, dataObjects);
        }

        public void OnContentIsOver(Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(AnalyticNames.ContentIsOver, dataObjects);
        }

        public void OnEvent(string eventName, Dictionary<string, object> dataObjects)
        {
            YandexMetrica.Send(eventName, dataObjects);
        }

        public void OnEvent(string eventName)
        {
            YandexMetrica.Send(eventName);
        }
    }
}
#endif