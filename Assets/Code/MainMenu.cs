using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static MainMenu Instance { get; set; }

    public GameObject startPanel;
    public GameObject roomsPanel;
    public GameObject joiningRoomPanel;
    public GameObject waitingForOtherPlayerPanel;

    [Space]
    public Transform roomsParent;
    public GameObject roomsButtonPrefab;
    public TMP_InputField roomNameInputField;
    public TextMeshProUGUI joiningRoomName;

    private void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Join()
    {
        startPanel.SetActive(false);

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1";
        }
        else
        {
            roomsPanel.SetActive(true);
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

    public void OnCreateRoomClicked()
    {
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
        roomsPanel.SetActive(true);
        //PhotonNetwork.JoinRandomRoom();
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
