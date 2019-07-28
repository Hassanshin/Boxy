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
    [Header("UI")]
    private int HighestTier;

    [SerializeField]
    private int[] tier = new int[5];

    [SerializeField]
    private int eFire, eEarth, eAir, eWater;

    [SerializeField]
    private int eAce, eStrong, eNeutral;

    

    [Header("UI")]

    [SerializeField]
    private Text v_Score;

    [SerializeField]
    private Text v_Name;

    public void ClearCount()
    {
        eFire = 0;
        eEarth = 0;
        eAir = 0;
        eWater = 0;

        eAce = 0;
        eStrong = 0;
        eNeutral = 0;

        v_Name.gameObject.SetActive(false);
        v_Score.gameObject.SetActive(false);
    }

    public void ClearList()
    {
        monster.Clear();

        
    }

    public void Counting()
    {
        ClearCount();
        elementStats();

        StartCoroutine( Scoring() );
    }

    #region Scoring method

    IEnumerator Scoring()
    {
        int _result = 0;
        int _resultStrongElement = 0;
        int _resultAceElement = 0;
        int _resultElement = 0;
        int _AllElementPoint = 0;

        v_Name.gameObject.SetActive(false);
        v_Score.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        

        // == PAIR COMBO

        //All element and No Ace and no strong
        if (allElement() && eAce <= 0 && eStrong <= 0)
        {
            _resultElement = 12;
            _result = _resultElement;                           // ScoreCombo

            UpdateUI(_result, "All Element");
        }
        else if(eFire > 2 || eEarth > 2 || eWater > 2 || eAir > 2)
        {
            //3 and  4 Pair
            UpdateUI(pair3and4(), "Squad");
        }
        else
        {
            // Use Highest Tier if All combo fails
            UpdateUI(HighestTier, "Highest Tier");
        }

        // == EMOTION COMBO

        if (eAce > 0)
        {
            foreach (Element _monster in monster)
            {
                // find the Ace elements
                if (_monster.m_Emotion == EmotionType.Ace)
                {
                    ElementType AceType = _monster.m_ElementType;

                    if (checkAllSameElement(AceType))
                    {
                        _resultAceElement = 10 + countSameElement(AceType, EmotionType.Strong);
                    }
                    else
                    {
                        _resultAceElement = -5 + countSameElement(AceType, EmotionType.Strong);
                    }

                    _result = _resultAceElement;    // ScoreCombo + Strong emot that same element
                    UpdateUI(_result, "Ace " + AceType + " Element");
                }
            }
        }

        else if(eStrong <= 0)
        {
            // 1 lane same element with no Strong type
            if (checkAllSameElement(ElementType.fire) ||
                checkAllSameElement(ElementType.air) ||
                checkAllSameElement(ElementType.earth) ||
                checkAllSameElement(ElementType.water))
            {
                _AllElementPoint = 5;
                _result = _AllElementPoint;        // ScoreCombo

                UpdateUI(_result, "Perfect Element");
            }
        }

        else
        {
            // 5 Element pair with Strong type
            foreach (Element _monster in monster)
            {
                // find the Strong elements
                if (_monster.m_Emotion == EmotionType.Strong)
                {
                    ElementType strongType = _monster.m_ElementType;

                    if (checkAllSameElement(strongType))
                    {
                        _resultStrongElement = 5;
                        _result = _resultStrongElement + _AllElementPoint + eStrong;   // ScoreCombo + 1 lane Combo + Every Strong types

                        UpdateUI(_result, "Strong " + strongType + " Element");
                    }
                }
            }
        }

        

    }

    int pair3and4()
    {
        int _result = 0;
        int point = 3;

        if (eFire == 3)
        {
            _result = point;
        }
        else if (eFire == 4)
        {
            _result = point + 2;
        }

        if (eAir == 3)
        {
            _result = point;
        }
        else if (eAir == 4)
        {
            _result = point + 2;
        }

        if (eEarth == 3)
        {
            _result = point;
        }
        else if (eEarth == 4)
        {
            _result = point + 2;
        }

        if (eWater == 3)
        {
            _result = point;
        }
        else if (eWater == 4)
        {
            _result = point + 2;
        }

        return _result;
    }

    bool allElement()
    {
        if (eFire > 0 && eAir > 0 && eEarth > 0 && eWater > 0)
        {
            return true;
        }

        return false;
    }

    bool checkAllSameElement(ElementType _type)
    {
        foreach (Element _monster in monster)
        {
            if (_monster.m_ElementType != _type)
                return false;
            
        }

        return true;
    }

    int countSameElement(ElementType _type, EmotionType _emot)
    {
        int _result = 0;

        foreach (Element _monster in monster)
        {
            if (_monster.m_ElementType == _type && 
                _monster.m_Emotion == _emot)
            {
                _result++;
            }
        }

        return _result;
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

            switch (monster[i].m_Emotion)
            {
                case EmotionType.Neutral:
                    eNeutral++;
                    break;
                case EmotionType.Strong:
                    eStrong++;
                    break;
                case EmotionType.Ace:
                    eAce++;
                    break;
                default:
                    break;
            }

            tier[monster[i].m_Tier - 1]++;

            HighestTier = highest();
        }

    }

    int highest()
    {
        int high = 0;

        for (int i = 0; i < monster.Count; i++)
        {
            if(monster[i].m_Tier > high)
            {
                high = monster[i].m_Tier;
            }
        }

        return high;
    }
    #endregion

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

    #region UI

    void UpdateUI(int _score, string _comboName)
    {
        Debug.Log(_score + "   " + _comboName);

        StopCoroutine(updateUInum(_score, _comboName));
        StartCoroutine(updateUInum(_score, _comboName));
    }

    IEnumerator updateUInum(int _score, string _comboName)
    {

        yield return new WaitForSeconds(0.5f);

        v_Name.gameObject.SetActive(true);
        v_Score.gameObject.SetActive(true);

        v_Score.text = _score + "";
        v_Name.text = _comboName;
    }

    #endregion


}
