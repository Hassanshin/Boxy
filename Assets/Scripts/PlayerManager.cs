using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> myMonsters = new List<GameObject>();

    [SerializeField]
    private Transform[] slots;

    public void MoveMonstersToField()
    {
        for (int i = 0; i < myMonsters.Count; i++)
        {
            myMonsters[i].transform.position = slots[i].position;
        }
    }
}
