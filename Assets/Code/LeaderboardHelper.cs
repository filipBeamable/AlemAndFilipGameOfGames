using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardHelper : MonoBehaviour
{
    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
