using System.Collections.Generic;
using Source.Scripts.Infrastructure.Services;

namespace Source.Scripts.Analytics
{
    public interface IAnalyticManager : IService
    {
        void AddAnalytic(IAnalytic analytic);
        void SendEventOnGameInitialize(int sessionCount);
        void SendEventOnLevelStart(int levelNumber);
        void SendEventOnLevelComplete(int levelNumber);
        void SendEventOnFail(int levelNumber);
        void SendEventOnLevelRestart(int levelNumber);
        void SendEventOnSoftSpent(string purchaseType, string storeName, int purchaseAmount, int purchasesCount);
        void SendEventOnGameExit(string registrationDate, int sessionCount, int daysInGame);
        void SendEventOnGameExit(string registrationDate, int sessionCount, int daysInGame, int currentSoft);
        void SendEventContentIsOver(int sessionCount, int daysInGame);
        void SendEvent(string eventName, Dictionary<string, object> dataObjects);
        void SendEvent(string eventName);
    }
}