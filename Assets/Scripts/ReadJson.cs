using UnityEngine;
using System.IO;

[System.Serializable]
public class LevelData
{
    public int[] level_data;
}

public class ReadJson : MonoBehaviour
{
    private string Kelias
    {
        get { return $"{Application.dataPath}/Scripts/level_data.json"; }
    }

    private string JsonString
    {
        get { return File.ReadAllText(Kelias); }
    }

    public LevelData[] LevelData
    {
        get { return JsonHelper.FromJson<LevelData>(JsonString); }
    }

}

internal static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.levels;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.levels = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.levels = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] levels;
    }
}