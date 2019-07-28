using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> myMonsters = new List<GameObject>();

    [SerializeField]
    private Transform[] slots;

    [SerializeField]
    private ScoreCounter[] counter;

    private Transform[] laneBot = new Transform[5];
    private Transform[] laneMid = new Transform[5];
    private Transform[] laneTop = new Transform[3];

    private int[] totalScore = new int[3];
    
    private int[] eFire = new int[3], eAir = new int[3], eWater = new int[3], eEarth = new int[3];

    private int AcePoint = 10;
    private int Pair3Point = 2;
    private int AllElementPoint = 10;

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
        // Clear Monster from triggering previous game
        foreach (ScoreCounter _counter in counter)
        {
            _counter.ClearList();
        }
        
        // Move Monsters to Players Field
        for (int i = 0; i < myMonsters.Count; i++)
        {
            //myMonsters[i].transform.SetParent(slots[i]);
            myMonsters[i].SetActive(true);
            //myMonsters[i].transform.SetAsFirstSibling();
            myMonsters[i].transform.position = slots[i].position;
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

    public void CountScore()
    {
        CountCounterList();


    }

    private void CountCounterList()
    {
        foreach (ScoreCounter _counter in counter)
        {
            _counter.Counting();
        }
    }

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
