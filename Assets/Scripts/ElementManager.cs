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
            players[i].RemoveMonstersFromField();
        }


        register();
    }

    public void CountingScore()
    {
        StartCoroutine(counting());
    }

    IEnumerator counting()
    {
        int[] maxScore = new int[players.Length];

        // Ready Pressed

        Debug.Log("Start Counting");

        yield return new WaitForSeconds(0.5f);

        // Updating Last Set

        for (int i = 0; i < players.Length; i++)
        {
            players[i].UpdateTotalFrom3Lane();
        }

        yield return new WaitForSeconds(2f);

        // Finding Maximum Each Lane
        for (int i = 0; i < 3; i++)
        {
            List<int> scoring = new List<int>();

            scoring.Add(players[0].totalScore[i]);
            scoring.Add(players[1].totalScore[i]);
            scoring.Add(players[2].totalScore[i]);
            scoring.Add(players[3].totalScore[i]);

            scoring.Sort();
            scoring.Reverse();

            yield return new WaitForSeconds(1f);

            // Finding Winner Each Lane
            

            Debug.Log(" List:" + scoring[0]);
            scoring.Clear();
        }
        
        


        Debug.Log("Done Counting " );
    }

   

    private void copyMonsters()
    {
        masterPoolList.AddRange(ReadytoShuffleList);
    }

    void masterToMonsterPool()
    {

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
        shuffledList = ShuffleList(ReadytoShuffleList);
        
    }

    private void register()
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
