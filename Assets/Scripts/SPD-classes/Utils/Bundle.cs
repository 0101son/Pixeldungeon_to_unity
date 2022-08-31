using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

public class Bundle
{
    private static readonly string TYPE_NAME = "__typeName";

    private JObject data;

    public Bundle()
    {
        data = new JObject();
    }


    public override string ToString()
    {
        return data.ToString();
    }

    private Bundle(JObject data)
    {
        this.data = data;
    }

    public bool IsNull()
    {
        return data == null;
    }

    public bool Contains(string key)
    {
        return data.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return data.Remove(key);
    }

    public List<string> GetKeys()
    {
        Dictionary<string,string> dictOBJ = data.ToObject<Dictionary<string, string>>();
        return new List<string>(dictOBJ.Keys);
    }

    public T Get<T>(string key)
    {
        return data[key].ToObject<T>();
    }

    public Vector2Int GetVector2Int(string key)
    {
        Bundle bundle = new Bundle((JObject)data[key]);
        return new Vector2Int(bundle.Get<int>(X), bundle.Get<int>(Y));
    }

    public Type GetType(string key)
    {
        return data[key].GetType();

    }

    public Bundle GetBundle(string key)
    {
        return new Bundle((JObject)data[key]);
    }

    private IBundlable GetBundlable()
    {
        if (data == null) return null;

        Type objType = Type.GetType(Get<string>(TYPE_NAME));
        Debug.Log("GetBundlable :" + objType);
        IBundlable obj = (IBundlable)Activator.CreateInstance(objType);
        obj.RestoreFromBundle(this);
        return obj;
        
    }


    public IBundlable GetBundlable(string key)
    {
        return GetBundle(key).GetBundlable();
    }

    /*
    public int[,] GetIntArrayDim2(string key)
    {
        JObject jobject = (JObject)data[key];
        int lenX = (int)jobject[X];
        int lenY = (int)jobject[Y];
        int[,] array = new int[lenX, lenY];

        JArray jArrayY = (JArray)jobject[ARRAY];
        
        for(int y = 0; y<lenY; y++)
        {
            JArray jArrayX = (JArray)jArrayY[y];
            for (int x = 0; x < lenX; x++)
                array[x, y] = (int)jArrayX[x];
        }
        return array;
    }

    public bool[,] GetBoolArrayDim2(string key)
    {
        JObject jobject = (JObject)data[key];
        int lenX = (int)jobject[X];
        int lenY = (int)jobject[Y];
        bool[,] array = new bool[lenX, lenY];

        JArray jArrayY = (JArray)jobject[ARRAY];

        for (int y = 0; y < lenY; y++)
        {
            JArray jArrayX = (JArray)jArrayY[y];
            for (int x = 0; x < lenX; x++)
                array[x, y] = (bool)jArrayX[x];
        }
        return array;
    }
    */

    public T[,] Get2DArray<T>(string key)
    {

        JObject jobject = (JObject)data[key];
        int lenX = (int)jobject[X];
        int lenY = (int)jobject[Y];
        T[,] array = new T[lenX, lenY];

        JArray jArrayY = (JArray)jobject[ARRAY];

        for (int y = 0; y < lenY; y++)
        {
            JArray jArrayX = (JArray)jArrayY[y];
            for (int x = 0; x < lenX; x++)
                array[x, y] = jArrayX[x].ToObject<T>();
        }
        return array;

    }
    public ICollection<IBundlable> GetCollection(string key)
    {
        List<IBundlable> list = new();

        JArray array = (JArray)data[key];
        for(int i=0; i< array.Count; i++)
        {
            IBundlable obj = new Bundle((JObject)array[i]).GetBundlable();
            list.Add(obj);
        }

        return list;
    }

    public void Put(string key, JToken value)
    {
        data.Add(key, value);
    }

    private static readonly string X = "X";
    private static readonly string Y = "Y";

    public void Put(string key, Vector2Int vector)
    {
        Bundle bundle = new Bundle();
        bundle.Put(X, vector.x);
        bundle.Put(Y, vector.y);
        data.Add(key, bundle.data);
    }

    public void Put(string key, Type value)
    {
        data.Add(key, value.ToString());
    }

    public void Put(string key, Bundle bundle)
    {
        data.Add(key, bundle.data);
    }

    public void Put(string key, IBundlable obj)
    {
        if(obj != null)
        {
            Bundle bundle = new Bundle();
            bundle.Put(TYPE_NAME, obj.GetType().ToString());
            obj.StoreInBundle(bundle);
            data.Add(key, bundle.data);
        }
    }

    private static readonly string ARRAY = "array";

    /*
    public void Put(string key, int[,] array)
    {
        int lenX = array.GetLength(0);
        int lenY = array.GetLength(1);
        JObject jobject = new JObject();
        jobject.Add(X, lenX);
        jobject.Add(Y, lenY);

        JArray jArrayY = new JArray();
        for (int y = 0; y < lenY; y++)
        {
            JArray jArrayX = new JArray();
            for(int x = 0; x< lenX; x++)
                jArrayX.Add(array[y,x]);
            jArrayY.Add(jArrayX);
        }

        jobject.Add(ARRAY, jArrayY);
        data.Add(key, jobject);
    }

    public void Put(string key, bool[,] array)
    {
        int lenX = array.GetLength(0);
        int lenY = array.GetLength(1);
        JObject jobject = new JObject();
        jobject.Add(X, lenX);
        jobject.Add(Y, lenY);

        JArray jArrayY = new JArray();
        for (int y = 0; y < lenY; y++)
        {
            JArray jArrayX = new JArray();
            for (int x = 0; x < lenX; x++)
                jArrayX.Add(array[y, x]);
            jArrayY.Add(jArrayX);
        }

        jobject.Add(ARRAY, jArrayY);
        data.Add(key, jobject);
    }
    */
    public void Put<T>(string key, T[,] array)
    {
        int lenX = array.GetLength(0);
        int lenY = array.GetLength(1);
        JObject jobject = new JObject();
        jobject.Add(X, lenX);
        jobject.Add(Y, lenY);

        JArray jArrayY = new JArray();
        for (int y = 0; y < lenY; y++)
        {
            JArray jArrayX = new JArray();
            for (int x = 0; x < lenX; x++)
                jArrayX.Add(array[x, y]);
            jArrayY.Add(jArrayX);
        }

        jobject.Add(ARRAY, jArrayY);
        data.Add(key, jobject);
    }

    //public T getArrayList<T>(ArrayList<T> arr) where T: someBaseClass
    public void Put<T>(string key, ICollection<T> collection) where T: IBundlable
    {
        JArray array = new();
        foreach(T obj in collection)
        {
            if(obj != null)
            {
                Bundle bundle = new Bundle();
                bundle.Put(TYPE_NAME, obj.GetType());
                obj.StoreInBundle(bundle);
                array.Add(bundle.data);
            }
        }
        data.Add(key, array);
    }

    public static void BundleToFile(string fileName, Bundle bundle)
    {
        string json = bundle.data.ToString();
        Debug.Log(fileName);
        File.WriteAllText(fileName, json);
            
    }

    public static Bundle BundleFormFile(string fileName)
    {
        string json = File.ReadAllText(fileName);
        return new Bundle(JObject.Parse(json));

    }
}
