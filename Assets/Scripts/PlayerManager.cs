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
    
    private int[] eFire = new int[3], eAir = new int[3], eWater = new int[3], eEarth = new int[3];

    private int AcePoint = 10;
    private int Pair3Point = 2; 

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
            myMonsters[i].SetActive(true);
            myMonsters[i].transform.SetAsFirstSibling();
            myMonsters[i].transform.localPosition = Vector3.zero;
        }

    }

    public void KillAllMonstersFromField()
    {
        //for (int i = 0; i < myMonsters.Count; i++)
        //{
        //    Destroy(myMonsters[i]);
        //}

        myMonsters.Clear();
    }

    public void CountScore()
    {
        updateEachLane();
        
        counting();

        PUi.UpdatePlayerUI();


    }

    #region Scoring method

    private void counting()
    {
        elementCounter(laneBot, 0);
        elementCounter(laneMid, 1);
        elementCounter(laneTop, 2);

        // Counting Score based on Lane
        totalScore[0] = TotalOn(laneBot, 0);
        totalScore[1] = TotalOn(laneMid, 1);
        totalScore[2] = TotalOn(laneTop, 2);

        
    }

    int TotalOn(Transform[] _lane, int _laneIndex)
    {
        int _result = 0;
        int _resultElement = 0;
        int _resultTier = 0;
        int _resultPair = 0;
        int _resultAce = 0;
        int _resultStrongElement = 0;

        for (int i = 0; i < _lane.Length; i++)
        {
            Element _monster = _lane[i].GetChild(0).GetComponent<Element>();

            
            // 5 Element pair with Strong type
            if (_monster.m_Emotion == EmotionType.Strong)
            {
                ElementType strongType = _monster.m_ElementType;

                if(checkAllSameElement(_lane, strongType))
                {
                    _resultStrongElement = 10;
                }


            }
            else
            {
                if (checkAllSameElement(_lane, ElementType.fire))
                {
                    _resultElement = 5;
                }
                else if (checkAllSameElement(_lane, ElementType.air))
                {
                    _resultElement = 5;
                }
                else if (checkAllSameElement(_lane, ElementType.earth))
                {
                    _resultElement = 5;
                }
                 else if (checkAllSameElement(_lane, ElementType.water))
                {
                    _resultElement = 5;
                }
            }

            // 3 Element pair on Bot and Mid lane
            if (_laneIndex < 2)
            {
                _resultPair = pair3and4(_laneIndex);
            }

            // Tier
            _resultTier += _monster.m_Tier;


            // Ace with other Element
            if (_monster.m_Emotion == EmotionType.Ace)
            {
                ElementType AceType = _monster.m_ElementType;

                if (checkAllSameElement(_lane, AceType))
                {
                    _resultAce += AcePoint;

                }
                else
                {
                    _resultAce -= AcePoint;

                }

                _resultPair = 0;
                _resultElement = 0;
            }

            //_result = _resultTier + _resultElement + _resultPair + _resultAce;
            _result = _resultElement + _resultPair + _resultAce + _resultStrongElement;

            
          
        }

        //Debug.Log("Tier:" + _resultTier + " Element:" + _resultElement + " Pair:" + _resultPair + " Ace:" + _resultAce);

        return _result;
    }

    int pair3and4(int _laneIndex)
    {
        int _result = 0;

        if (eFire[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eFire[_laneIndex] >= 4)
        {
            _result = Pair3Point + 2;
        }

        if (eAir[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eAir[_laneIndex] >= 4)
        {
            _result = Pair3Point + 2;
        }

        if (eEarth[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eEarth[_laneIndex] >= 4)
        {
            _result = Pair3Point + 2;
        }

        if (eWater[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eWater[_laneIndex] >= 4)
        {
            _result = Pair3Point +2;
        }

        return _result;
    }

    void elementCounter(Transform[] _lane, int _laneIndex)
    {
        eFire[_laneIndex] = 0;
        eEarth[_laneIndex] = 0;
        eAir[_laneIndex] = 0;
        eWater[_laneIndex] = 0;

        for (int i = 0; i < _lane.Length; i++)
        {
            Element _monster = _lane[i].GetChild(0).GetComponent<Element>();

            switch (_monster.m_ElementType)
            {
                case ElementType.fire:
                    eFire[_laneIndex]++;
                    break;
                case ElementType.water:
                    eWater[_laneIndex]++;
                    break;
                case ElementType.air:
                    eAir[_laneIndex]++;
                    break;
                case ElementType.earth:
                    eEarth[_laneIndex]++;
                    break;
                default:
                    break;
            }
        }
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
