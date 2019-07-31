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

    public int totalScore;

    [Header("UI")]

    [SerializeField]
    private Text v_Score;

    [SerializeField]
    private Text v_Name;

    [SerializeField]
    private GameObject v_isCounting;

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

    public void isCountingThisLane(bool _state)
    {
        v_isCounting.SetActive(_state);
    }
    

    public void Counting()
    {
        ClearCount();
        elementStats();

        transform.parent.GetComponent<PlayerManager>().UpdateTotalFrom3Lane();

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
            _resultElement = 10;
            _result = _resultElement;                           // ScoreCombo

            UpdateUI(_result, "All Element");
        }
        else if(eFire == 3 || eEarth == 3 || eWater == 3 || eAir == 3)
        {
            //3 and  4 Pair
            UpdateUI(pair3and4(), "Tri");                     // ScoreCombo
        }
        else if (eFire == 4 || eEarth == 4 || eWater == 4 || eAir == 4)
        {
            //3 and  4 Pair
            UpdateUI(pair3and4(), "Quadra");                     // ScoreCombo
        }
        else
        {
            // Use Highest Tier if All combo fails
            UpdateUI(HighestTier - 1, "Highest Tier");          // ScoreCombo - 1
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
                        _resultAceElement = -2 + countSameElement(AceType, EmotionType.Strong);
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
                _AllElementPoint = 7;
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
                        _resultStrongElement = 7;
                        _result = _resultStrongElement + eStrong;   // ScoreCombo + 1 lane Combo + Every Strong types

                        UpdateUI(_result, "Strong " + strongType + " Element");
                    }
                }
            }
        }

        

    }

    int pair3and4()
    {
        int _result = 0;
        int point = 4;

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

    #region UI

    void UpdateUI(int _score, string _comboName)
    {
        //Debug.Log(_score + "   " + _comboName);

        StopCoroutine(updateUInum(_score, _comboName));
        StartCoroutine(updateUInum(_score, _comboName));
    }

    IEnumerator updateUInum(int _score, string _comboName)
    {
        // Final score shown in UI 

        yield return new WaitForSeconds(0.5f);

        v_Name.gameObject.SetActive(true);
        v_Score.gameObject.SetActive(true);

        v_Score.text = _score + "";
        v_Name.text = _comboName;

        totalScore = _score;
    }

    #endregion


}
