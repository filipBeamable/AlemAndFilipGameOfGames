using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject joinPanel;
    public GameObject waitingPanel;

    public void Join()
    {
        joinPanel.SetActive(false);
        waitingPanel.SetActive(true);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Level");
    }
}
