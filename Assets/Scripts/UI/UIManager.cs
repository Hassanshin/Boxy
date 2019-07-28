using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public gameState gameStateNow = gameState.idle;

    private ElementManager i_ElementManager;
    public GameObject humanPlayer;

    [SerializeField]
    private Button mainButton;

    private void Awake()
    {
        
        i_ElementManager = ElementManager._instance;

    }

    private void Start()
    {
        humanPlayer.GetComponent<DragHandler>().enabled = true;
        mainButton.GetComponentInChildren<Text>().text = "Start";

        
    }

    public void BtnReady()
    {
        switch (gameStateNow)
        {
            case gameState.playing:
                
                countingScore();
                break;

            case gameState.counting:
                
                resetAFterCounting();
                break;

            case gameState.idle:
                
                beginGameAfterIdling();
                break;

            default:
                break;
        }
    }

    void beginGameAfterIdling()
    {
        i_ElementManager.RestartGame();
        humanPlayer.GetComponent<PlayerManager>().CountScore();
        humanPlayer.GetComponent<DragHandler>().enabled = true;

        gameStateNow = gameState.playing;

        mainButton.GetComponentInChildren<Text>().text = "Ready";
    }

    void resetAFterCounting()
    {
        //i_ElementManager.RestartGame();
        //humanPlayer.GetComponent<PlayerManager>().CountScore();
        

        gameStateNow = gameState.idle;

        mainButton.GetComponentInChildren<Text>().text = "Start";
    }

    void countingScore()
    {
        i_ElementManager.CountingScore();
        humanPlayer.GetComponent<DragHandler>().enabled = false;

        gameStateNow = gameState.counting;

        mainButton.GetComponentInChildren<Text>().text = "Restart";
    }
}

public enum gameState
{
    playing, counting, idle
}
