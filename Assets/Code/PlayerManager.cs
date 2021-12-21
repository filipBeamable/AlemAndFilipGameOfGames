using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<PlayerController> players;
    public List<OtherPlayer> otherPlayers;
    public AudioSource audioSource;

    [HideInInspector] public int currentActiveIndex = -1;
    public PlayerController CurrentActivePlayer => players[currentActiveIndex];

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentActiveIndex = 0;
        for (int i = 0; i < players.Count; i++)
            players[i].SetIsMain(i == 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        foreach (PlayerController player in players)
        {
            if (player.cameraController.IsAnimating)
                return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetMainPlayer(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetMainPlayer(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetMainPlayer(2);
    }

    private void SetMainPlayer(int index)
    {
        if (currentActiveIndex == index)
            return;

        PlayerController oldPlayer = CurrentActivePlayer;
        currentActiveIndex = index;
        oldPlayer.SetIsMain(false);
        CurrentActivePlayer.SetIsMain(true);
        CurrentActivePlayer.cameraController.LerpFromOldPlayer(oldPlayer);

        UpdateActivePlayerHealth();
    }

    public void UpdateActivePlayerHealth()
    {
        UIController.Instance.mainCharacterHealth.UpdateSlider(CurrentActivePlayer.Health / CurrentActivePlayer.startingHealth);
    }
}
