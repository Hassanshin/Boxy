using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElementManager : MonoBehaviour
{
    private const string MONSTER_TAG = "monster";

    private List<GameObject> monstersPoolList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> shuffledList = new List<GameObject>();

    [SerializeField]
    private Transform fieldPlayers;

    [SerializeField]
    private PlayerManager[] players;
    

    #region Singleton

    public static ElementManager _instance = null;

    private void Awake()
    {
        _instance = this;
    }

    #endregion

    private void Start()
    { 
        registerTypeMonsters();
        players = fieldPlayers.GetComponentsInChildren<PlayerManager>();
        shuffleMonsters();

        
        
    }

    private void disableDragHandler()
    {
        
    }

    private void registerTypeMonsters()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).tag == MONSTER_TAG)
                monstersPoolList.Add(transform.GetChild(i).gameObject);
        }

        players = fieldPlayers.GetComponentsInChildren<PlayerManager>();

    }
    
    void shuffleMonsters()
    {
        shuffledList = ShuffleList(monstersPoolList);
    }

    public void Ready()
    {
        for (int i = 0; i < players.Length; i++)
        {
            RegisterMonstersToPlayer(i);
        }
    }

    public List<GameObject> ShuffleList<GameObject>(List<GameObject> inputList)
    {
        List<GameObject> randomList = new List<GameObject>();

        //Random r = new Random();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = Random.Range(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

    void RegisterMonstersToPlayer(int _playerIndex)
    {
        for (int i = 0; i < 13; i++)
        {
            int randomNum = Random.Range(0, shuffledList.Count);
            if(shuffledList[randomNum] != null)
            {
                players[_playerIndex].myMonsters.Add(shuffledList[randomNum]);
                shuffledList.RemoveAt(randomNum);
            }
        }

        players[_playerIndex].MoveMonstersToField();
    }
}
