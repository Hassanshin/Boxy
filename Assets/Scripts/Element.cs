using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField]
    private ElementType m_ElementType;

    [SerializeField]
    private int m_Tier;

    [SerializeField]
    private int m_Emotion;

    public void Start()
    {
        
    }
}

public enum ElementType
{
    fire, water, air, earth
}

