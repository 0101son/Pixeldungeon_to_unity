using System.Collections;
using UnityEngine;
using System;
using System.IO;

namespace Gpm.CacheStorage
{
    [Serializable]
    public class CacheStorageConfig
    {
        public const string CONFIG_NAME = "CacheStorageConfig";

        [SerializeField]
        private string cachePath;

        public string GetCachePath()
        {
            if (string.IsNullOrEmpty(cachePath) == true)
            {
                SetCachePath(Application.temporaryCachePath);
            }

            return cachePath;
        }

        public void SetCachePath(string path)
        {
            cachePath = Path.Combine(path, GpmCacheStorage.NAME);
            Save();
        }

        private static string ConfigPath()
        {
            return Path.Combine(Application.persistentDataPath, CONFIG_NAME);
        }

        public static CacheStorageConfig Load()
        {
            CacheStorageConfig config = null;
            try
            {
                config = JsonUtility.FromJson<CacheStorageConfig>(File.ReadAllText(ConfigPath()));
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            return config;
        }

        public void Save()
        {
            File.WriteAllText(ConfigPath(), JsonUtility.ToJson(this));
        }
        
    }
} 