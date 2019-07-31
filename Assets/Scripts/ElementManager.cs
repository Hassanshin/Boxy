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

    

    bool isCounting = false;



    public bool IsCounting
    {
        get
        {
            return isCounting;
        }
    }

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

    public void ClearField()
    {
        shuffledList.Clear();
        ReadytoShuffleList.Clear();

        for (int _playerIndex = 0; _playerIndex < players.Length; _playerIndex++)
        {
            players[_playerIndex].ClearMonster();
        }
    }

    public void RestartGame()
    {
        
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
        isCounting = true;
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

            // Adding Player (0, 1, 2, 3) with score each lane (0, 1, 2)
            for (int a = 0; a < players.Length; a++)
            {
                if (!scoring.Contains(players[a].totalScore[i]))
                    scoring.Add(players[a].totalScore[i]);
            }

            // Sorting by score
            scoring.Sort();
            scoring.Reverse();

            yield return new WaitForSeconds(1f);

            // Finding Winner Each Lane

            for (int a = 0; a < players.Length; a++)
            {
                PlayerManager _player = players[a];

                _player.ChangeBottomText("");
                _player.isFocusedOn(i);
                _player.ChangeEmoticonPlayer(1);

                yield return new WaitForSeconds(2f);

                _player.myRank_Line[i] = rankPlayer(scoring, _player.totalScore[i]);
                _player.ChangeBottomText("Lane " + (i + 1) + ": rank " + (_player.myRank_Line[i] + 1));

                switch (_player.myRank_Line[i])
                {
                    case 0:
                        _player.Money += 25;
                        break;
                    case 1:
                        _player.Money += 10;
                        break;
                    case 2:
                        _player.Money += -5;
                        break;
                    case 3:
                        _player.Money += -10;
                        break;
                    default:
                        break;
                }

                // Changing sprites
                if(_player.myRank_Line[i] == 0)
                {
                    // If win, rank 1
                    _player.ChangeEmoticonPlayer(2);

                    
                }
                else
                {
                    // If lose
                    _player.ChangeEmoticonPlayer(3);
                }
            }

            yield return new WaitForSeconds(1f);

            scoring.Clear();
        }

        doneCounting();
    }

    private void doneCounting()
    {
        isCounting = false;
        Debug.Log("Done Counting ");

        for (int a = 0; a < players.Length; a++)
        {
            players[a].clearFocus();
            players[a].ChangeBottomText("");
            players[a].GetComponent<PlayerManager>().ChangeEmoticonPlayer(0);
        }
    }

    int rankPlayer(List<int> scoringList, int myScore)
    {
        int result = 9;

        for (int i = 0; i < players.Length; i++)
        {
            if(myScore == scoringList[i])
            {
                result = i;
                break;
            }
        }

        return result;
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
