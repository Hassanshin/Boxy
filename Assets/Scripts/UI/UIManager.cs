using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool isPlaying;

    private ElementManager i_ElementManager;

    public GameObject humanPlayer;

    private void Awake()
    {
        
        i_ElementManager = ElementManager._instance;
    }

    private void Start()
    {
        humanPlayer.GetComponent<DragHandler>().enabled = true;

        
    }

    public void BtnReady()
    {
        if (isPlaying)
        {
            BtnShuffle();
        }
        else
        {
            
            i_ElementManager.Register();
            isPlaying = true;

            humanPlayer.GetComponent<PlayerManager>().CountScore();
        }

    }

    public void BtnShuffle()
    {
        if (!isPlaying)
            return;

        i_ElementManager.RestartGame();
        i_ElementManager.Register();

        humanPlayer.GetComponent<PlayerManager>().CountScore();
    }
}
