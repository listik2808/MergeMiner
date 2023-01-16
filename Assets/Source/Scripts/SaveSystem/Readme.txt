Storage Asset.
--------------

Позволяет сохранять ряд данных для последующего использования в аналитке, 
а так же данные типов: int, float, string, Vector3 (Unity), Quaternion (Unity)
аналогично использованию Prefs.

Хранилище можно создать в двух модах.
По умолчанию хранилище создается в SaveMode.Delayed режиме. При этом после установки значений в Set...
методах данные будут только назначаться в Data. Для сохранения необходимо выполнить Save(),
например в конце уровня.
В режиме SaveMode.Immediately (new Storage(DataNames.GameName, SaveMode.Immediately)) после использования Set...
данные будут сохраняться автоматически, использовать Save() не обязательно.
----------------------------------

Методы Has возвращают bool, позволяют проверить содержит ли соответствующее хранилище
данные по ключу.

Методы Set записывают данные в соответствующее хранилище по ключу.

Методы Get по ключу позволяют получить данные из хранилща. Если ключ отсутствует в хранилище
метод вызовет ArgumentException();

ClearData - позволяет очистить хранилище.
----------------------------------

В данный момент ассет поддерживает удаленное сохранение только совместно с пакетом agava.yandexgames
https://github.com/forcepusher/com.agava.yandexgames
После установки agava.yandexgames необходимо так же в Unity перейти в PlayerSettings -> Other Settings и
в Sctipting Define Symbols добавить YANDEX_GAMES.
----------------------------------

Для платформы UNITY_WEBGL становятся доступны методы:
SaveRemote сохраняет данные локально и отправляет в удаленное хранилище.

SyncRemoteSave сравнивает локальные и удаленные данные по дате создания, сохраняет более новые.
Возвращает IEnumerator, необходимо дождаться выполнения yield return Storage.SyncRemoteSave();,
либо yield return Storage.SyncRemoteSave(DoSomethingAfterCallback);
Либо (в не ассинхронных методах) вызвать StartCoroutine(Storage.SyncRemoteSave(DoSomethingAfterCallback));

ClearDataRemote очищает локальные и удаленные данные.
Возвращает IEnumerator, необходимо дождаться выполнения yield return Storage.ClearDataRemote();,
либо yield return Storage.ClearDataRemote(DoSomethingAfterCallback);
Либо (в не ассинхронных методах) вызвать StartCoroutine(Storage.ClearDataRemote(DoSomethingAfterCallback));
----------------------------------

Примечание: по сути Storage Asset собирает Data и сохраняет в PlayerPrefs по ключу
private readonly string _dataName; после сериализации.
По умполчанию в конструкторе задается значение dataName = "DataName".
Рекомендуется для каждой игры задавать уникальное имя данных, наример new Storage("GameName");
В каждой игре возможно создать несколько хранилищ. При этом доступ к нужному будет доступен
по соответствующему ключу после инициализации.
