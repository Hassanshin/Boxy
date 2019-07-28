using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    
    #region Collider

    private const string MONSTER_TAG = "monster";
    public List<Element> monster = new List<Element>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != MONSTER_TAG)
            return;

        Element _otherElement = other.GetComponent<Element>();

        if (monster.Contains(_otherElement))
            return;

        monster.Add(_otherElement);
        Counting();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != MONSTER_TAG)
            return;

        Element _otherElement = other.GetComponent<Element>();

        monster.Remove(_otherElement);
    }

    #endregion

    [SerializeField]
    private int eFire, eEarth, eAir, eWater;

    #region Scoring method

    private Text v_Score;
    private Text v_Name;

    public void ClearCount()
    {
        eFire = 0;
        eEarth = 0;
        eAir = 0;
        eWater = 0;
    }

    public void ClearList()
    {
        monster.Clear();
    }

    public void Counting()
    {
        ClearCount();
        elementStats();
        
        //// Counting Score based on Lane
        //totalScore[0] = TotalOn(laneBot, 0);
        //totalScore[1] = TotalOn(laneMid, 1);
        //totalScore[2] = TotalOn(laneTop, 2);


    }

    void elementStats()
    {
        for (int i = 0; i < monster.Count; i++)
        {
            switch (monster[i].m_ElementType)
            {
                case ElementType.fire:
                    eFire++;
                    break;
                case ElementType.water:
                    eWater++;
                    break;
                case ElementType.air:
                    eAir++;
                    break;
                case ElementType.earth:
                    eEarth++;
                    break;
                default:
                    break;
            }
        }
    }


    /*

    

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

                if (checkAllSameElement(_lane, strongType))
                {
                    _resultStrongElement = 5;
                }


            }

            // 1 lane same element with no Strong type
            if (checkAllSameElement(_lane, ElementType.fire))
            {
                _resultElement = AllElementPoint;
            }
            else if (checkAllSameElement(_lane, ElementType.air))
            {
                _resultElement = AllElementPoint;
            }
            else if (checkAllSameElement(_lane, ElementType.earth))
            {
                _resultElement = AllElementPoint;
            }
            else if (checkAllSameElement(_lane, ElementType.water))
            {
                _resultElement = AllElementPoint;
            }

            //All element
            if (allElement(_laneIndex))
            {
                _resultElement = AllElementPoint;
            }

            // Tier
            _resultTier += _monster.m_Tier;




            //_result = _resultTier + _resultElement + _resultPair + _resultAce;




        }

        for (int c = 0; c < _lane.Length; c++)
        {
            Element _monster = _lane[c].GetChild(0).GetComponent<Element>();
            // Ace with other Element
            if (_monster.m_Emotion == EmotionType.Ace)
            {
                ElementType AceType = _monster.m_ElementType;

                if (checkAllSameElement(_lane, AceType))
                {
                    _resultAce = AcePoint;

                }
                else
                {
                    _resultAce = -AcePoint / 2;

                }


                _resultPair = 0;
                _resultElement = 0;


            }
            // No ace no Strong, so using pairing
            else if (_monster.m_Emotion != EmotionType.Strong)
            {
                // 3 Element pair on Bot and Mid lane
                if (_laneIndex < 2)
                {
                    _resultPair = pair3and4(_laneIndex);

                }
            }
        }

        _result = _resultElement + _resultPair + _resultAce + _resultStrongElement;


        if (_laneIndex == 1)
            Debug.Log("Tier:" + _resultTier + "  Element:" + _resultElement + "  Pair:" + _resultPair + "  Strong:" + _resultStrongElement + "  Ace:" + _resultAce);

        return _result;
    }

    bool allElement(int _laneIndex)
    {
        if (eFire[_laneIndex] > 0 && eAir[_laneIndex] > 0 && eEarth[_laneIndex] > 0 && eWater[_laneIndex] > 0)
        {
            return true;
        }

        return false;
    }

    int pair3and4(int _laneIndex)
    {
        int _result = 0;

        if (eFire[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eFire[_laneIndex] == 4)
        {
            _result = Pair3Point + 2;
        }

        if (eAir[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eAir[_laneIndex] == 4)
        {
            _result = Pair3Point + 2;
        }

        if (eEarth[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eEarth[_laneIndex] == 4)
        {
            _result = Pair3Point + 2;
        }

        if (eWater[_laneIndex] == 3)
        {
            _result = Pair3Point;
        }
        else if (eWater[_laneIndex] == 4)
        {
            _result = Pair3Point + 2;
        }

        return _result;
    }

    

    bool checkAllSameElement(Transform[] _lane, ElementType _type)
    {
        for (int i = 0; i < _lane.Length; i++)
        {
            Element _monster = _lane[i].GetChild(0).GetComponent<Element>();

            if (_monster.m_ElementType != _type)
            {
                return false;
            }
        }

        return true;
    }

    */

    #endregion



}
