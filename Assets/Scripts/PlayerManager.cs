using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> myMonsters = new List<GameObject>();

    [SerializeField]
    private Transform[] slots;

    private Transform[] laneBot = new Transform[5];
    private Transform[] laneMid = new Transform[5];
    private Transform[] laneTop = new Transform[3];
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

    public void MoveMonstersToField()
    {
        for (int i = 0; i < myMonsters.Count; i++)
        {
            myMonsters[i].transform.SetParent(slots[i]);
            myMonsters[i].transform.SetAsFirstSibling();
            myMonsters[i].transform.localPosition = Vector3.zero;
        }

    }

    public void CountScore()
    {
        updateEachLane();

        counting();

        PUi.UpdatePlayerUI();

        Debug.Log("countScore PManager");
    }

    #region Scoring method

    private void counting()
    {
        // Counting Score based on Lane

        totalScore[0] = TotalOn(laneBot);
        totalScore[1] = TotalOn(laneMid);
        totalScore[2] = TotalOn(laneTop);

        checkElement(laneBot);
        checkElement(laneMid);
        checkElement(laneTop);
    }

    private void checkElement(Transform[] _lane)
    {
        for (int i = 0; i < _lane.Length; i++)
        {
            Element _monster = _lane[i].GetChild(0).GetComponent<Element>();

            
        }

        
    }

    int TotalOn(Transform[] _lane)
    {
        int _result = 0;
        int _resultElement = 0;
        int _resultTier = 0;

        for (int i = 0; i < _lane.Length; i++)
        {
            Element _monster = _lane[i].GetChild(0).GetComponent<Element>();

            // Tier
            _resultTier += _monster.m_Tier;

            // All Element with Strong type
            if (_monster.m_Emotion == EmotionType.Strong)
            {
                ElementType strongType = _monster.m_ElementType;
                Debug.Log(strongType);

                if(checkAllSameElement(_lane, strongType))
                {
                    _resultElement += 2;
                }
            }

            _result = _resultElement;

        }
        return _result;
    }

    bool checkAllSameElement(Transform[] _lane, ElementType _type)
    {
        for (int i = 0; i < _lane.Length; i++)
        {
            Element _monster = _lane[i].GetChild(0).GetComponent<Element>();

            if(_monster.m_ElementType != _type)
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    private void updateEachLane()
    {

        int a = 0, b = 0, c = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            Element _monster = slots[i].GetChild(0).GetComponent<Element>();

            if (i < 5)
            {
                
                laneBot[a] = slots[i];
                a++;
                //totalScore[0] += _monster.m_Tier;
            }
            else if (i < 10)
            {
                
                laneMid[b] = slots[i];
                b++;
                //totalScore[1] += _monster.m_Tier;
            }
            else
            {
                
                laneTop[c] = slots[i];
                c++;
                //totalScore[2] += _monster.m_Tier;
            }

            
            
        }


    }

    
    
}
