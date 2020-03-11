using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class InteractRuleTile : RuleTile
{
    public string Tag;
}

[CreateAssetMenu]
public class SoilTile : InteractRuleTile
{
    private float _saturation;
    public float Saturation
    {
        get { return _saturation; }
        set
        { 
            _saturation = Mathf.Clamp(value, 0, MaxSaturation);
            if (_saturation >= MaxSaturation)
            {
                FlipSprite();
            }
            
        }
    }
    public float MaxSaturation = 10f;

    void FlipSprite()
    {
    }
}


