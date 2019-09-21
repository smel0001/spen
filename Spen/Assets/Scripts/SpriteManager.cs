using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct NamedSprite
{
    public string name;
    public Sprite sprite;
}


public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance
    {
        get { return s_Instance; }
    }
    protected static SpriteManager s_Instance;

    public NamedSprite[] _Sprites;

    private Dictionary<string, Sprite> SpriteMap;

    void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }

        //Manually create dict
        SpriteMap = new Dictionary<string, Sprite>();

        foreach (NamedSprite value in _Sprites)
        {
            SpriteMap.Add(value.name, value.sprite);
        }
        
    }

    public Sprite GetSprite(string name)
    {
        if (SpriteMap.ContainsKey(name))
        {
            return SpriteMap[name];
        }
        else
        {
            //temp return default
            return SpriteMap["Grass"];
        }
        
    }
}
