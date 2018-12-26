using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceHelper : Singleton<ResourceHelper>
{

    public readonly string TestAtlasPath = "Test/Atlas";

    public readonly string TestPrefabPath = "Test/PreFabs";

    public readonly string PrefabPath = "PreFabs";
    
    public ResourceHelper ()
    {
       
    }
    
    public T LoadResouce<T>(string baseUrl, string path) where T : Object
    {
        string fullPath = Path.Combine(baseUrl, path);
        Log.Deb(String.Format("LoadResouce path : {0}",fullPath));
        return Resources.Load<T>(Path.Combine(baseUrl, path));
    }
    
    public T[] LoadAllResouce<T>(string baseUrl, string path) where T : Object
    {
        string fullPath = Path.Combine(baseUrl, path);
        Log.Deb(String.Format("LoadAllResouce path : {0}",fullPath));
        return Resources.LoadAll<T>(Path.Combine(baseUrl, path));
    }
    
    public GameObject LoadPrefab(string path, bool test = false)
    {
        string baseUrl = test ? TestPrefabPath : PrefabPath;
        Log.Deb(String.Format("LoadPrefab :{0} {1}",baseUrl, path));
        return LoadResouce<GameObject>(baseUrl, path);
    }
    
    public  Sprite[] LoadAtalas(string path)
    {
        string baseUrl = TestAtlasPath;
        Log.Deb(String.Format("LoadSprite :{0} {1}",baseUrl, path));
        return LoadAllResouce<Sprite>(TestAtlasPath, path);
    }

    public GameObject LoadFX(string path, bool test = false)
    {
        string baseUrl = "FX/" + path;
        return LoadPrefab(baseUrl, test);
    }
}
