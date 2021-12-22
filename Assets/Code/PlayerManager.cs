using Beamable;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private IBeamableAPI _beamableApi;

    public static PlayerManager Instance { get; private set; }
    public static float MouseSensitivity = -1;

    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }
    public bool ShouldSwitchCharacter { get; private set; }

    public GameObject playerPrefab;
    public GameObject explosionPrefab;
    public float rifleDamage;
    public float mouseSensitivity = 200f;
    public float maxTimeBeforeSwitch = 7f;
    public List<Transform> masterPlayerPositions;
    public List<Transform> otherPlayerPositions;

    public List<PlayerController> players;
    public List<PlayerController> otherPlayers;
    public AudioSource audioSource;

    [HideInInspector] public int currentActiveIndex = -1;
    public PlayerController CurrentActivePlayer => players[currentActiveIndex];

    private float timer;

    private void Awake()
    {
        Instance = this;
        if (MouseSensitivity < 0)
            MouseSensitivity = mouseSensitivity;
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

        UIController.Instance.mouseSensitivitySlider.value = MouseSensitivity;
        UIController.Instance.mouseSensitivitySlider.onValueChanged.AddListener(MouseSensitivityChanged);

        ResetTimer();
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

        if (!IsPaused && !ShouldSwitchCharacter && players.Count(p => p.gameObject.activeSelf) > 1)
        {
            timer += Time.deltaTime;
            if (timer >= maxTimeBeforeSwitch)
            {
                ShouldSwitchCharacter = true;
                UIController.Instance.switchCharacterInfo.SetActive(true);
                UIController.Instance.timer.text = "0";
            }
            else
            {
                int time = (int)((maxTimeBeforeSwitch - timer) + 0.9999f);
                UIController.Instance.timer.text = time.ToString();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetMainPlayer(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetMainPlayer(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetMainPlayer(2);
    }

    public void ResetTimer()
    {
        timer = 0f;
        ShouldSwitchCharacter = false;
        UIController.Instance.timer.text = ((int)(maxTimeBeforeSwitch + 0.5f)).ToString();
        UIController.Instance.switchCharacterInfo.SetActive(false);
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
        ResetTimer();
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
        MouseSensitivity = newValue;
    }

    public void IncrementScore(int score)
    {
        _beamableApi.LeaderboardService.IncrementScore("leaderboards.lc", score);
    }
}
