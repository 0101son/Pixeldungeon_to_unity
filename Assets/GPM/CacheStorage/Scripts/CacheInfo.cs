using System.Net;
using UnityEngine;
using System;


namespace Gpm.CacheStorage
{
    using Common;

    [Serializable]
    public class CacheInfo
    {
        [Serializable]
        public struct CacheControl
        {
            public long maxAge;

            public bool mustRevalidate;
        }
        
        [NonSerialized]
        internal CachePackage storage;

        [SerializeField]
        public string url;

        [SerializeField]
        public string eTag;

        [SerializeField]
        public string lastModified;

        [SerializeField]
        public long lastAccess;

        [SerializeField]
        public string expires;

        [SerializeField]
        public long received;

        [SerializeField]
        public string age;

        [SerializeField]
        public string date;

        [SerializeField]
        public CacheControl cacheControl;

        [SerializeField]
        public long contentLength;

        [SerializeField]
        internal int index;

        public CacheInfo(CachePackage storage, string url)
        {
            this.storage = storage;
            this.url = url;
        }

    }
}