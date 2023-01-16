using System;
using System.Collections;
using UnityEngine;
#if YANDEX_GAMES && !UNITY_EDITOR
#endif
using Agava.YandexGames;

namespace Source.Scripts.SaveSystem
{
    public class Storage : IStorage
    {
        private readonly string _dataName;

        private readonly SaveMode _mode;
        private Data _data;

        public event Action Changed;

        public Storage(string dataName = "DataName", SaveMode mode = SaveMode.Delayed)
        {
            _dataName = dataName;
            _mode = mode;
            _data = Load();
        }

        public void SetFloat(string key, float value)
        {
            if (_data.Floats.ContainsKey(key))
                _data.Floats[key] = value;
            else
                _data.Floats.Add(key, value);
            Changed?.Invoke();
            if (_mode == SaveMode.Immediately) Save();
        }

        public float GetFloat(string key)
        {
            return _data.Floats.ContainsKey(key)
                ? _data.Floats[key]
                : throw new ArgumentException($"Floats doesn't contain Key: {key}");
        }

        public bool HasKeyFloat(string key)
        {
            return _data.Floats.ContainsKey(key);
        }

        public void SetInt(string key, int value)
        {
            if (_data.Ints.ContainsKey(key))
                _data.Ints[key] = value;
            else
                _data.Ints.Add(key, value);
            Changed?.Invoke();
            if (_mode == SaveMode.Immediately) Save();
        }

        public int GetInt(string key)
        {
            return _data.Ints.ContainsKey(key) 
                ? _data.Ints[key] 
                : throw new ArgumentException($"Ints doesn't contain Key: {key}");
        }

        public bool HasKeyInt(string key)
        {
            return _data.Ints.ContainsKey(key);
        }

        public void SetString(string key, string value)
        {
            if (_data.Strings.ContainsKey(key))
                _data.Strings[key] = value;
            else
                _data.Strings.Add(key, value);
            Changed?.Invoke();
            if (_mode == SaveMode.Immediately) Save();
        }

        public string GetString(string key)
        {
            return _data.Strings.ContainsKey(key) 
                ? _data.Strings[key] 
                : throw new ArgumentException($"Strings doesn't contain Key: {key}");
        }

        public bool HasKeyString(string key)
        {
            return _data.Strings.ContainsKey(key);
        }

        public void SetVector3(string key, Vector3 value)
        {
            if (_data.Vectors.ContainsKey(key))
                _data.Vectors[key] = value.AsVectorData();
            else
                _data.Vectors.Add(key, value.AsVectorData());
            Changed?.Invoke();
            if (_mode == SaveMode.Immediately) Save();
        }

        public Vector3 GetVector3(string key)
        {
            return _data.Vectors.ContainsKey(key)
                ? _data.Vectors[key].AsUnityVector()
                : throw new ArgumentException($"Vectors doesn't contain Key: {key}");
        }

        public bool HasKeyVector3(string key)
        {
            return _data.Vectors.ContainsKey(key);
        }

        public void SetQuaternion(string key, Quaternion value)
        {
            if (_data.Quaternions.ContainsKey(key))
                _data.Quaternions[key] = value.AsQuaternionData();
            else
                _data.Quaternions.Add(key, value.AsQuaternionData());
            Changed?.Invoke();
            if (_mode == SaveMode.Immediately) Save();
        }

        public Quaternion GetQuaternion(string key)
        {
            return _data.Quaternions.ContainsKey(key)
                ? _data.Quaternions[key].AsUnityQuaternion()
                : throw new ArgumentException($"Quaternions doesn't contain Key: {key}");
        }

        public bool HasKeyQuaternion(string key)
        {
            return _data.Quaternions.ContainsKey(key);
        }

        public void AddDisplayedLevelNumber()
        {
            _data.DisplayedLevelNumber++;
            if (_mode == SaveMode.Immediately) Save();
        }

        public int GetDisplayedLevelNumber()
        {
            return _data.DisplayedLevelNumber;
        }

        public void AddSession()
        {
            _data.SessionCount++;
            if (_mode == SaveMode.Immediately) Save();
        }

        public int GetSessionCount()
        {
            return _data.SessionCount;
        }

        public DateTime GetRegistrationDate()
        {
            return DateTime.Parse(_data.RegistrationDate);
        }

        public void SetLastLoginDate()
        {
            _data.LastLoginDate = DateTime.Now.ToString();
            if (_mode == SaveMode.Immediately) Save();
        }

        public DateTime GetLastLoginDate()
        {
            return DateTime.Parse(_data.LastLoginDate);
        }

        public void SetTimeSinceLastLogin()
        {
            _data.TimeSinceLastLogin = (DateTime.Now - GetLastLoginDate()).ToString();
            if(_mode == SaveMode.Immediately) Save();
        }

        public TimeSpan GetTimeSinceLastLogin()
        {
            return TimeSpan.Parse(_data.TimeSinceLastLogin);
        }

        public void SetLastRewardTakingTime()
        {
            _data.LastRewardTakingTime = DateTime.Now.ToString();
            if(_mode == SaveMode.Immediately) Save();
        }

        public DateTime GetLastRewardTakingTime()
        {
            return DateTime.Parse(_data.LastRewardTakingTime);
        }

        public int GetNumberDaysAfterRegistration()
        {
            return GetLastLoginDate().Day - GetRegistrationDate().Day;
        }

        public void SetSoft(int value)
        {
            _data.Soft = value;
            Changed?.Invoke();
            if (_mode == SaveMode.Immediately) Save();
        }

        public int GetSoft()
        {
            return _data.Soft;
        }

        public DateTime GetSaveTime()
        {
            return DateTime.Parse(_data.SaveTime);
        }

        public void SetLevel(int index)
        {
            _data.LevelNumber = index;
            if (_mode == SaveMode.Immediately) Save();
        }

        public int GetLevel()
        {
            return _data.LevelNumber;
        }
        
        public void Save()
        {
            _data.SaveTime = DateTime.Now.ToString();
            PlayerPrefs.SetString(_dataName, _data.ToJson());
        }

        public void ClearData()
        {
            _data = new Data();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Data cleared");
        }

#if UNITY_WEBGL
        public void SaveRemote()
        {
            Save();
#if YANDEX_GAMES && !UNITY_EDITOR
            if(PlayerAccount.IsAuthorized)
                PlayerAccount.SetPlayerData(_data.ToJson());
#endif
            Debug.Log("Saved to remote storage");
        }

        public IEnumerator SyncRemoteSave(Action onDataIsSynchronizedCallback = null)
        {
            var isRemoteDataLoaded = false;
            LoadRemote(remoteData =>
            {
                remoteData ??= new Data();

                var localDataSaveTime = DateTime.Parse(_data.SaveTime);
                var remoteDataSaveTime = DateTime.Parse(remoteData.SaveTime);

                if (remoteDataSaveTime > localDataSaveTime)
                {
                    _data = remoteData;
                    Save();
                }
                else
                {
                    SaveRemote();
                }

                isRemoteDataLoaded = true;
                onDataIsSynchronizedCallback?.Invoke();
            });

            while (isRemoteDataLoaded == false)
                yield return null;
        }
        
        public IEnumerator ClearDataRemote(Action onRemoteDataCleared = null)
        {
            ClearData();
#if !UNITY_WEBGL || UNITY_EDITOR
            onRemoteDataCleared?.Invoke();
            yield return true;
#endif          
#if YANDEX_GAMES && !UNITY_EDITOR

            if (!PlayerAccount.IsAuthorized)
            {
                onRemoteDataCleared?.Invoke();
                yield return null;
            }
            else
            {
                var isRemoteDateCleared = false;

                PlayerAccount.SetPlayerData("", () =>
                {
                    isRemoteDateCleared = true;
                    onRemoteDataCleared?.Invoke();
                });

                while (isRemoteDateCleared == false)
                    yield return null;
            }
#endif
        }

        private void LoadRemote(Action<Data> onDataLoadedCallback)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("Loaded from remote storage");
            onDataLoadedCallback?.Invoke(null);
#endif
#if YANDEX_GAMES && !UNITY_EDITOR
            if(PlayerAccount.IsAuthorized)
                PlayerAccount.GetPlayerData(data =>
                {
                    onDataLoadedCallback?.Invoke(data == "" ? null : data.ToDeserialized<Data>());
                });
            else
                onDataLoadedCallback?.Invoke(null);
#endif
        }
#endif
        private Data Load()
        {
            return PlayerPrefs.GetString(_dataName)?.ToDeserialized<Data>() ?? new Data();
        }
    }
}