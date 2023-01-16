#if APP_METRICA
using System.Collections.Generic;

namespace Source.Scripts.Analytics
{
    public class AppMetricaAnalytic : IAnalytic
    {
        public void OnGameInitialize(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.GameStart, dataObjects);
        }

        public void OnLevelStart(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.LevelStart, dataObjects);
        }

        public void OnLevelComplete(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.LevelComplete, dataObjects);
        }

        public void OnLevelFail(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.Fail, dataObjects);
        }

        public void OnLevelRestart(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.Restart, dataObjects);
        }

        public void OnSoftSpent(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.SoftSpent, dataObjects);
        }

        public void OnRegistrationDayIs(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.RegistrationDay, dataObjects);
        }

        public void OnSessionCountIs(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.SessionCount, dataObjects);
        }

        public void OnDaysInGameIs(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.DaysInGame, dataObjects);
        }

        public void OnCurrentSoftHave(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.CurrentSoft, dataObjects);
        }

        public void OnContentIsOver(Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(AnalyticNames.ContentIsOver, dataObjects);
        }

        public void OnEvent(string eventName, Dictionary<string, object> dataObjects)
        {
            AppMetrica.Instance.ReportEvent(eventName, dataObjects);
        }

        public void OnEvent(string eventName)
        {
            AppMetrica.Instance.ReportEvent(eventName);
        }
    }
}
#endif