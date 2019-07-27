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
            return;

        i_ElementManager.Ready();
        isPlaying = true;

        humanPlayer.GetComponent<PlayerManager>().countScore();
    }
}
