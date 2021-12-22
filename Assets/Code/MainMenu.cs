using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static MainMenu Instance { get; set; }

    public GameObject startPanel;
    public GameObject waitingToJoinPanel;
    public GameObject roomsPanel;
    public GameObject joiningRoomPanel;
    public GameObject waitingForOtherPlayerPanel;

    [Space]
    public Transform roomsParent;
    public GameObject roomsButtonPrefab;
    public TMP_InputField roomNameInputField;
    public TextMeshProUGUI joiningRoomName;
    public TMP_Dropdown regionDropdown;

    private string regionToken;

    private void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true;

        regionDropdown.onValueChanged.AddListener(OnRegionChanged);
    }

    private void OnRegionChanged(int index)
    {
        string region = regionDropdown.options[index].text;
        if (region == "USA East")
            regionToken = "us";
        if (region == "USA West")
            regionToken = "usw";
        if (region == "Europe")
            regionToken = "eu";
        if (region == "Australia")
            regionToken = "au";
        if (region == "Canada")
            regionToken = "cae";
        if (region == "Russia")
            regionToken = "ru";
        if (region == "South America")
            regionToken = "sa";
        if (region == "Turkey")
            regionToken = "tr";
        else
            regionToken = "";
    }

    public void Join()
    {
        startPanel.SetActive(false);
        waitingToJoinPanel.SetActive(true);

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1";
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("Score2");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Level");
    }

    public void GoBack()
    {
        PhotonNetwork.LeaveLobby();
        roomsPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomsPanel.SetActive(true);
        waitingForOtherPlayerPanel.SetActive(false);
    }

    public void OnCreateRoomClicked()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text) || string.IsNullOrWhiteSpace(roomNameInputField.text))
            return;

        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 2 });
        GoToJoiningRoom(roomNameInputField.text);
        roomNameInputField.text = "";
    }

    public void OnClickedRoomButton(string name)
    {
        PhotonNetwork.JoinRoom(name);
        GoToJoiningRoom(name);
    }

    private void GoToJoiningRoom(string roomName)
    {
        joiningRoomPanel.SetActive(true);
        roomsPanel.SetActive(false);
        joiningRoomName.text = roomName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        waitingToJoinPanel.SetActive(false);
        roomsPanel.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Pun room list update callback");

        for (int i = roomsParent.childCount - 1; i >= 0; i--)
            Destroy(roomsParent.GetChild(i).gameObject);

        foreach (RoomInfo roomInfo in roomList)
        {
            if (!roomInfo.IsOpen || roomInfo.PlayerCount >= roomInfo.MaxPlayers)
                continue;

            Instantiate(roomsButtonPrefab, roomsParent).GetComponent<RoomButton>().Init(roomInfo.Name);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (PhotonNetwork.IsMasterClient)
        {
            joiningRoomPanel.SetActive(false);
            waitingForOtherPlayerPanel.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Other Player joined room, loading level");
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Level");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Created room failed");
        joiningRoomPanel.SetActive(false);
        roomsPanel.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
}
