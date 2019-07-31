using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> myMonsters = new List<GameObject>();

    [SerializeField]
    private Transform[] slots;

    [SerializeField]
    private ScoreCounter[] counter;

    public Canvas cam;
    private PlayerUI PUi;

    public int[] totalScore = new int[3];

    private int money;

    public int Money
    {
        get
        {
            return money;
        }

        set
        {
            int DefMoney = money;
            int MoneyChange = value - DefMoney;

            money = value;

            v_Money.text = money + "";

            if (MoneyChange < 0)
                v_MoneyChange.text = MoneyChange + "";
            else
                v_MoneyChange.text = "+" + MoneyChange + "";

            StartCoroutine( hideChanges() );

            if (HumanControl)
            {
                PlayerPrefs.SetInt("money", money);
            }
        }
    }

    IEnumerator hideChanges()
    {
        yield return new WaitForSeconds(1f);
        v_MoneyChange.text = "";
    }

    public int[] myRank_Line = new int[3];

    [SerializeField]
    private Text v_Money;

    [SerializeField]
    private Text v_MoneyChange;

    [SerializeField]
    private Text v_BottomText;

    [SerializeField]
    private Image v_CharToChange;

    [SerializeField]
    private Color[] c_ColorChar;

    [SerializeField]
    private Image v_EmotToChange;

    [SerializeField]
    private Sprite[] v_EmotPlayer;

    /*
     * 0 idle
     * 1 counting
     * 2 win
     * 3 lose
     * 
     * 
     */

    private int charIndex;

    public bool HumanControl;

    

    private void Start()
    {
        cam.worldCamera = Camera.main;

        PUi = GetComponent<PlayerUI>();

        

        if (HumanControl)
        {
            charIndex = PlayerPrefs.GetInt("char");

            changeCharacter(charIndex);
            v_Money.text = PlayerPrefs.GetInt("money") + "";
        }
    }

    public void ChangeEmoticonPlayer(int _index)
    {
        v_EmotToChange.sprite = v_EmotPlayer[_index];
    }

    public void ChangeBottomText(string _text)
    {
        v_BottomText.text = _text;
    }



    public void ClearMonster()
    {
        for (int i = 0; i < myMonsters.Count; i++)
        {
            myMonsters[i].SetActive(false);
        }
    }

    public void MoveMonstersToField()
    {
        // Clear Monster from triggering previous game
        foreach (ScoreCounter _counter in counter)
        {
            _counter.ClearList();
        }
        
        // Move Monsters to Players Field
        for (int i = 0; i < myMonsters.Count; i++)
        {
            myMonsters[i].transform.SetParent(slots[i]);
            myMonsters[i].SetActive(true);
            myMonsters[i].transform.SetAsFirstSibling();
            myMonsters[i].transform.localPosition = Vector3.zero;
        }

    }

    public void isFocusedOn(int _index)
    {
        for (int i = 0; i < counter.Length; i++)
        {
            counter[i].isCountingThisLane(false);
        }

        counter[_index].isCountingThisLane(true);
    }

    public void clearFocus()
    {
        for (int i = 0; i < counter.Length; i++)
        {
            counter[i].isCountingThisLane(false);
        }

        
    }

    public void RemoveMonstersFromField()
    {
        for (int i = 0; i < myMonsters.Count; i++)
        {
            myMonsters[i].SetActive(false);
        }

        myMonsters.Clear();
    }

    private void changeCharacter(int _index)
    {
        charIndex = _index;

        v_CharToChange.color = c_ColorChar[_index];

    }

    public void CountScore()
    {
        // Count the Score 
        CountCounterList();

        
    }

    private void CountCounterList()
    {
        for (int i = 0; i < counter.Length; i++)
        {
            counter[i].Counting();
            
        }
        
    }

    public void UpdateTotalFrom3Lane()
    {
        for (int i = 0; i < counter.Length; i++)
        {
            totalScore[i] = counter[i].totalScore;
        }
    }
    
}
