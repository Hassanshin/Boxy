using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> myMonsters = new List<GameObject>();

    [SerializeField]
    private Transform[] slots;
    
    [SerializeField]
    private int[] totalScore = new int[3];

    public Canvas cam;

    private PlayerUI PUi;

    public int[] TotalScore
    {
        get
        {
            return totalScore;
        }
    }

    private void Start()
    {
        cam.worldCamera = Camera.main;

        PUi = GetComponent<PlayerUI>();
    }

    public void countScore()
    {
        countLine(slots);
        PUi.UpdatePlayerUI();

        Debug.Log("countScore PManager");
    }

    public void MoveMonstersToField()
    {
        for (int i = 0; i < myMonsters.Count; i++)
        {
            myMonsters[i].transform.SetParent(slots[i]);
            myMonsters[i].transform.localPosition = Vector3.zero;

            //myMonsters[i].transform.position = slots[i].position;
        }
        
    }

    private void countLine(Transform[] _slots)
    {
        for (int i = 0; i < totalScore.Length; i++)
        {
            totalScore[i] = 0;
        }
       
        for (int i = 0; i < _slots.Length; i++)
        {
            Element _monster = _slots[i].GetChild(0).GetComponent<Element>();

            if (i < 5)
            {
                totalScore[0] += _monster.m_Tier;
            }
            else if (i < 10)
            {
                totalScore[1] += _monster.m_Tier;
            }
            else
            {
                Debug.Log(_monster.m_Tier);
                totalScore[2] += _monster.m_Tier;
            }

            
            
        }
    }

    
    
}
