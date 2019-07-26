using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElementManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> monstersPoolList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> shuffledList = new List<GameObject>();

    private void Start()
    {
        shuffledList = ShuffleList(monstersPoolList);
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
}
