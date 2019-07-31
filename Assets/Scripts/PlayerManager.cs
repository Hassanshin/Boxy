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

    public int[] myRank_Line = new int[3];

    [SerializeField]
    private Text bottomText;

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

    private void Start()
    {
        cam.worldCamera = Camera.main;

        PUi = GetComponent<PlayerUI>();
    }

    public void ChangeEmoticonPlayer(int _index)
    {
        v_EmotToChange.sprite = v_EmotPlayer[_index];
    }

    public void ChangeBottomText(string _text)
    {
        bottomText.text = _text;
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

        ChangeBottomText("done");
    }

    public void RemoveMonstersFromField()
    {
        for (int i = 0; i < myMonsters.Count; i++)
        {
            myMonsters[i].SetActive(false);
        }

        myMonsters.Clear();
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
