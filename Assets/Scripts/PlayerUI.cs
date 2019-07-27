using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Text[] v_Score;

    [SerializeField]
    private Text v_TotalScore;

    private PlayerManager PManager;

    private void Start()
    {
        PManager = GetComponent<PlayerManager>();
    }

    public void UpdatePlayerUI()
    {
        scoreUpdate();
    }

    private void scoreUpdate()
    {
        for (int i = 0; i < PManager.TotalScore.Length; i++)
        {
            v_Score[i].text = PManager.TotalScore[i] + "";
        }

        int AllLaneScore = PManager.TotalScore[0] + PManager.TotalScore[1] + PManager.TotalScore[2];

        v_TotalScore.text = "Total Score = " + AllLaneScore;
    }
}
