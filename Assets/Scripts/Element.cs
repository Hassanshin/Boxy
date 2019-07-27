using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    //[SerializeField]
    public ElementType m_ElementType;

    //[SerializeField]
    public int m_Tier;

    //[SerializeField]
    public EmotionType m_Emotion;

    [Header("Color")]
    public Color colorFire;
    public Color colorAir;
    public Color colorEarth;
    public Color colorWater;

    [Header("Model3d")]
    public Mesh[] modelTier;

    public MeshFilter FilterToChange;

    public void Start()
    {
        changeModel();
    }

    private void changeModel()
    {
        c_tier();
        c_element();
        c_emotion();
    }

    private void c_emotion()
    {
        

        switch (m_Emotion)
        {
            case EmotionType.Neutral:
                break;
            case EmotionType.Strong:
                transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                break;
            case EmotionType.Ace:
                transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void c_element()
    {
        Material model = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;

        switch (m_ElementType)
        {
            case ElementType.fire:
                model.color = colorFire;
                break;
            case ElementType.water:
                model.color = colorWater;
                break;
            case ElementType.air:
                model.color = colorAir;
                break;
            case ElementType.earth:
                model.color = colorEarth;
                break;
            default:
                break;
        }
    }

    void c_tier()
    {
        float _scale = 1f;

        FilterToChange.mesh = modelTier[m_Tier-1];

        switch (m_Tier)
        {
            case 1:
                _scale = 0.7f;
                break;
            case 2:
                _scale = 0.8f;
                break;
            case 3:
                _scale = 0.9f;
                break;
            case 4:
                _scale = 1f;
                break;
            case 5:
                _scale = 1.1f;
                break;
            default:
                break;
        }

        transform.localScale = new Vector3(_scale, _scale, _scale);
    }
}

public enum ElementType
{
    fire, water, air, earth
}

public enum EmotionType
{
    Neutral, Strong, Ace
}

