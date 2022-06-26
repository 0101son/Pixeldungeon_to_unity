using System.Collections;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace Gpm.CacheStorage
{
    using Common;

    [Serializable]
    public class CachePackage : ISerializationCallbackReceiver
    {
        public const string PACKAGE_NAME = "CacheStoragePackage";

        [SerializeField]
        public List<CacheInfo> cacheStorage = new List<CacheInfo>();

        [SerializeField]
        internal int lastIndex = 0;

        [SerializeField]
        internal long cachedSize = 0;

        [SerializeField]
        internal int requestTime = 0;

        [SerializeField]
        private List<int> spaceIdx = new List<int>();

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            foreach (var info in cacheStorage)
            {
                info.storage = this;
            }
        }

        internal CacheInfo GetCacheInfo(string url)
        {
            foreach (CacheInfo cachInfo in cacheStorage)
            {
                if (cachInfo.url.Equals(url))
                {
                    return cachInfo;
                }
            }

            return null;
        }

        List<CacheInfo> requestCache = new List<CacheInfo>();

        public CacheInfo RequestLocal(string url, System.Action<GpmCacheStorage.Result> onResult)
        {
            CacheInfo info = GetCacheInfo(url);

            GpmCacheStorage.Result result = new GpmCacheStorage.Result();
            result.success = false;

            if (info != null)
            {
                result.info = info;
                result.data = GetCacheData(info);
                if (result.data != null)
                {
                    result.success = true;
                }
            }
            onResult(result);

            return info;
        }
        public CacheInfo Request(string url, System.Action<GpmCacheStorage.Result> onResult)
        {
            foreach (var rq in requestCache)
            {
                if (rq.url.Equals(url) == true)
                {
                    return rq;
                }
            }

            CacheInfo info = GetCacheInfo(url);
            if (info == null)
            {
                info = new CacheInfo(this, url);
            }

            System.Action<byte[]> OnData = (datas) =>
            {
                info.lastAccess = DateTime.UtcNow.Ticks;

                if (datas == null)
                {
                    GpmCacheStorage.Result result = new GpmCacheStorage.Result();
                    result.success = false;
                    result.info = info;
                    result.data = null;

                    onResult(result);
                }
                else
                {
                    GpmCacheStorage.Result result = new GpmCacheStorage.Result();
                    result.success = true;
                    result.info = info;
                    result.data = datas;

                    onResult(result);
                }
            };

            // no Internet
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                byte[] datas = GetCacheData(info);
                OnData(datas);
                return info;
            }

            bool isForce = false;
            if (info.received > 0 &&
                requestTime > 0)
            {
                if (DateTime.UtcNow.Ticks - info.received > requestTime)
                {
                    byte[] datas = GetCacheData(info);

                    if (datas != null)
                    {
                        OnData(datas);
                        return info;
                    }
                    else
                    {
                        isForce = true;
                    }
                }
            }

            requestCache.Add(info);

            GpmWebRequest request = new GpmWebRequest();
            if (isForce == false)
            {
                if (string.IsNullOrEmpty(info.eTag) == false)
                {
                    request.SetRequestHeader("If-None-Match", info.eTag);
                }
                if (string.IsNullOrEmpty(info.lastModified) == false)
                {
                    request.SetRequestHeader("If-Modified-Since", info.lastModified);
                }
            }

            request.Get(url, (requestResult) =>
            {
                requestCache.Remove(info);
                if (requestResult.isSuccess == true)
                {
                    if (requestResult.responseCode == (long)HttpStatusCode.NotModified)
                    {
                        byte[] datas = GetCacheData(info);
                        if (datas != null)
                        {
                            OnData(datas);
                        }
                        else
                        {
                            // 데이타가 없다면 재요청
                            info.eTag = string.Empty;
                            Request(url, onResult);
                        }
                    }
                    else if (requestResult.responseCode == (long)HttpStatusCode.OK)
                    {
                        info.eTag = requestResult.request.GetResponseHeader("ETag");
                        info.expires = requestResult.request.GetResponseHeader("Expires");
                        info.lastModified = requestResult.request.GetResponseHeader("Last-Modified");

                        info.age = requestResult.request.GetResponseHeader("Age");
                        info.date = requestResult.request.GetResponseHeader("Date");

                        info.received = DateTime.UtcNow.Ticks;

                        byte[] datas = requestResult.request.downloadHandler.data;

                        if (datas != null)
                        {
                            info.contentLength = datas.LongLength;

                            AddCacheData(info, datas);
                        }

                        OnData(datas);
                    }
                }
                else
                {
                    OnData(null);
                }
            });
            return info;
        }

        

        internal string GetCacheDataPath(CacheInfo info)
        {
            if (info.index > 0)
            {
                return Path.Combine(GpmCacheStorage.GetCachePath(), info.index.ToString());
            }

            return "";
        }

        public void SaveCacheData(CacheInfo info, byte[] data)
        {
            if (Directory.Exists(GpmCacheStorage.GetCachePath()) == false)
            {
                Directory.CreateDirectory(GpmCacheStorage.GetCachePath());
            }
            string filePath = GetCacheDataPath(info);

            File.WriteAllBytes(filePath, data);
        }

        public byte[] GetCacheData(CacheInfo info)
        {
            string filePath = GetCacheDataPath(info);

            return File.ReadAllBytes(filePath);
        }

        public void AddCacheData(CacheInfo info, byte[] datas)
        {
            if (spaceIdx.Count > 0)
            {
                info.index = spaceIdx[0];
                spaceIdx.RemoveAt(0);
            }
            else
            {
                info.index = ++lastIndex;
            }

            cachedSize += info.contentLength;

            
            SaveCacheData(info, datas);

            cacheStorage.Add(info);

            GpmCacheStorage.SavePackage();
        }

        public void SecuringStorage(long maxSize, long addSize = 0)
        {
            if (addSize > maxSize)
            {
                return;
            }

            while  (cacheStorage.Count > 0 &&
                    cachedSize + addSize > maxSize)
            {
                if (RemoveCacheData(cacheStorage[0]) == false)
                {
                    break;
                }
            }
        }

        public bool RemoveCacheData(CacheInfo info)
        {
            if (cacheStorage.Remove(info) == true)
            {
                try
                {
                    string filePath = GetCacheDataPath(info);
                    File.Delete(filePath);

                    spaceIdx.Add(info.index);
                    spaceIdx.Sort();

                    while (spaceIdx[spaceIdx.Count - 1] >= lastIndex)
                    {
                        spaceIdx.RemoveAt(spaceIdx.Count - 1);
                        lastIndex--;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }

                cachedSize -= info.contentLength;

                GpmCacheStorage.SavePackage();

                return true;
            }
            else
            {
                return false;
            }
        }


        public void Remove()
        {
            Directory.Delete(GpmCacheStorage.GetCachePath());

            lastIndex = 0;
            cachedSize = 0;
            cacheStorage.Clear();
            spaceIdx.Clear();
        }

        public void RemoveAll()
        {
            foreach (CacheInfo info in cacheStorage)
            {
                try
                {
                    string filePath = GetCacheDataPath(info);
                    File.Delete(filePath);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }

            lastIndex = 0;
            cachedSize = 0;
            cacheStorage.Clear();
            spaceIdx.Clear();

            GpmCacheStorage.SavePackage();
        }
        private static string PackagePath()
        {
            return Path.Combine(GpmCacheStorage.GetCachePath(), PACKAGE_NAME);
        }

        public static CachePackage Load()
        {
            CachePackage cachePackage = null;

            string path = PackagePath();
            if (File.Exists(path) == true)
            {
                try
                {
                    cachePackage = JsonUtility.FromJson<CachePackage>(File.ReadAllText(path));
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }

            return cachePackage;
        }

        public void Save()
        {
            if(Directory.Exists(GpmCacheStorage.GetCachePath()) == false)
            {
                Directory.CreateDirectory(GpmCacheStorage.GetCachePath());
            }
            
            File.WriteAllText(PackagePath(), JsonUtility.ToJson(this));
        }
    }
}