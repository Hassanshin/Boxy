using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform[] charPosition;

    [SerializeField]
    private GameObject chosen;

    private void Start()
    {
        if(chosen != null)
            chosen.transform.position = charPosition[PlayerPrefs.GetInt("char")].position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void BtnChangeChar(int _index)
    {
        PlayerPrefs.SetInt("char", _index);

        chosen.transform.position = charPosition[_index].position;
    }

    public void BtnChangeScene(int _index)
    {
        SceneManager.LoadScene(_index);
    }

    public void BtnQuit()
    {
        Application.Quit();
    }
}
