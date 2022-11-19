using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TriadicPalette", menuName = "ScriptableObjects/TriadicPalette", order = 1)]
public class TriadicPaletteSO : ScriptableObject
{
    private float _minS = 0f, _maxS = 1f, _minV = 0f, _maxV = 1f;


    [ShowInInspector, PropertyRange(0f, 1f)]
    public float minS
    {
        get { return _minS; }
        set
        {
            _minS = value;
            color1 = _color1;
        }
    }

    [ShowInInspector, PropertyRange(0f, 1f)]
    public float maxS
    {
        get { return _maxS; }
        set
        {
            _maxS = value;
            color1 = _color1;
        }
    }

    [ShowInInspector, PropertyRange(0f, 1f)]
    public float minV
    {
        get { return _minV; }
        set
        {
            _minV = value;
            color1 = _color1;
        }
    }

    [ShowInInspector, PropertyRange(0f, 1f)]
    public float maxV
    {
        get { return _maxV; }
        set
        {
            _maxV = value;
            color1 = _color1;
        }
    }

    [ShowInInspector]
    public Color color1
    {
        get { return _color1; }
        set
        {

            Color.RGBToHSV(value, out float H, out float S, out float V);
            _color1 = Color.HSVToRGB(H, Mathf.Clamp(S, minS, maxS), Mathf.Clamp(V, minV, maxV));
            CreatePalette();
        }
    }
    [ShowInInspector]
    [ReadOnly]
    public Color color2
    {
        get; private set;
    }
    [ShowInInspector]
    [ReadOnly]
    public Color color3
    {
        get; private set;
    }
    private Color _color1;

    private void CreatePalette()
    {
        Color.RGBToHSV(color1, out float H, out float S, out float V);
        color2 = Color.HSVToRGB((H + 0.33f) % 1f, S, V);
        color3 = Color.HSVToRGB((H + 0.66f) % 1f, S, V);
        Debug.Log("Creating palette");
    }

    public void RandomizePallete()
    {
        color1 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

}