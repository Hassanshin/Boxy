using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElementManager : MonoBehaviour
{
    private const string MONSTER_TAG = "monster";

    private List<GameObject> ReadytoShuffleList = new List<GameObject>();
    private List<GameObject> masterPoolList = new List<GameObject>();

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
        players = fieldPlayers.GetComponentsInChildren<PlayerManager>();

        registerTypeMonsters();
        shuffleMonsters();

    }

    public void RestartGame()
    {
        shuffledList.Clear();
        ReadytoShuffleList.Clear();
        masterToMonsterPool();

        for (int i = 0; i < players.Length; i++)
        {
            players[i].KillAllMonstersFromField();
        }


        Register();
    }

    private void copyMonsters()
    {
        masterPoolList.AddRange(ReadytoShuffleList);

        //for (int i = 0; i < ReadytoShuffleList.Count; i++)
        //{
        //    GameObject copy = Instantiate(ReadytoShuffleList[i]);
        //    masterPoolList.Add(Instantiate(ReadytoShuffleList[i]));
            
        //}
    }

    void masterToMonsterPool()
    {
        Debug.Log("master to Monster Pool " + masterPoolList.Count);
        ReadytoShuffleList.AddRange(masterPoolList);
        shuffledList = ShuffleList(ReadytoShuffleList);
    }

    private void registerTypeMonsters()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).tag == MONSTER_TAG)
                ReadytoShuffleList.Add(transform.GetChild(i).gameObject);
        }

        players = fieldPlayers.GetComponentsInChildren<PlayerManager>();
        copyMonsters();
    }
    
    void shuffleMonsters()
    {
        Debug.Log("Shuffle monsters " + ReadytoShuffleList.Count);
        shuffledList = ShuffleList(ReadytoShuffleList);
        
    }

    public void Register()
    {
        for (int i = 0; i < players.Length; i++)
        {
            registerMonstersToPlayer(i);
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

    void registerMonstersToPlayer(int _playerIndex)
    {
        // memberi setiap player sebnayak 13 monster
        Debug.Log("Register to Player " + shuffledList.Count);
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
