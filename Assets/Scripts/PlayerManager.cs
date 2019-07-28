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

    public Canvas cam;
    private PlayerUI PUi;

    public int[] totalScore = new int[3];

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
            myMonsters[i].transform.SetParent(slots[i]);
            myMonsters[i].SetActive(true);
            myMonsters[i].transform.SetAsFirstSibling();
            myMonsters[i].transform.localPosition = Vector3.zero;
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
