using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject joinPanel;
    public GameObject waitingPanel;

    public void Join()
    {
        joinPanel.SetActive(false);
        waitingPanel.SetActive(true);
    }
}
