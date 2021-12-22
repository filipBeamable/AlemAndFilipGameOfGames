using Beamable;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private IBeamableAPI _beamableApi;

    public static PlayerManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }

    public GameObject playerPrefab;
    public GameObject explosionPrefab;
    public float rifleDamage;
    public float mouseSensitivity = 200f;
    public List<Transform> masterPlayerPositions;
    public List<Transform> otherPlayerPositions;

    public List<PlayerController> players;
    public List<PlayerController> otherPlayers;
    public AudioSource audioSource;

    [HideInInspector] public int currentActiveIndex = -1;
    public PlayerController CurrentActivePlayer => players[currentActiveIndex];

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        _beamableApi = await API.Instance;
        List<Transform> spawnPoints = PhotonNetwork.IsMasterClient ? masterPlayerPositions : otherPlayerPositions;

        foreach (Transform spawnPoint in spawnPoints)
            players.Add(PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation).GetComponent<PlayerController>());

        currentActiveIndex = 0;
        for (int i = 0; i < players.Count; i++)
            players[i].SetIsMain(i == 0);

        UIController.Instance.mouseSensitivitySlider.onValueChanged.AddListener(MouseSensitivityChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
            UIController.Instance.pauseMenu.SetActive(IsPaused);
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

    public void Exit()
    {
        GoToLeaderboard();
    }

    private bool SetMainPlayer(int index)
    {
        if (currentActiveIndex == index || !players[index].gameObject.activeSelf)
            return false;

        PlayerController oldPlayer = CurrentActivePlayer;
        currentActiveIndex = index;
        oldPlayer.SetIsMain(false);
        CurrentActivePlayer.SetIsMain(true);
        CurrentActivePlayer.cameraController.LerpFromOldPlayer(oldPlayer);

        UpdateActivePlayerHealth();
        return true;
    }

    public void UpdateActivePlayerHealth()
    {
        UIController.Instance.mainCharacterHealth.UpdateSlider(CurrentActivePlayer.Health / CurrentActivePlayer.startingHealth);
    }

    public void OnPlayerDied(Player deadPlayer)
    {
        List<PlayerController> playersList = deadPlayer.photonView.IsMine ? players : otherPlayers;
        List<CharacterUI> characterUIs = deadPlayer.photonView.IsMine ? UIController.Instance.myCharacters : UIController.Instance.enemyCharacters;
        characterUIs[playersList.IndexOf(deadPlayer as PlayerController)].PlayAnimAndChangeColor();

        bool allDead = true;
        foreach (Player player in players)
        {
            if (player.gameObject.activeSelf)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            OtherPlayerWon();
            return;
        }
        else
        {
            if (deadPlayer == CurrentActivePlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (SetMainPlayer(i))
                        break;
                }
            }
        }

        allDead = true;
        foreach (Player player in otherPlayers)
        {
            if (player.gameObject.activeSelf)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            LocalPlayerWon();
            return;
        }
    }

    private void LocalPlayerWon()
    {
        UIController.Instance.GameOver("YOU WON :)");
        IsGameOver = true;
    }
    private void OtherPlayerWon()
    {
        UIController.Instance.GameOver("YOU LOST :(");
        IsGameOver = true;
    }

    public void GoToLeaderboard()
    {
        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Score2");
    }

    public void MouseSensitivityChanged(float newValue)
    {
        mouseSensitivity = newValue;
    }

    public void IncrementScore(int score)
    {
        _beamableApi.LeaderboardService.IncrementScore("leaderboards.lc", score);
    }
}
