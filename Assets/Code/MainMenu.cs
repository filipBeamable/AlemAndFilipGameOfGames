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

    public GameObject playButton;
    public GameObject roomsPanel;

    [Space]
    public Transform roomsParent;
    public GameObject roomsButtonPrefab;
    public TMP_InputField roomNameInputField;

    private void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Join()
    {
        playButton.SetActive(false);
        roomsPanel.SetActive(true);

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1";
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Level");
    }

    public void OnCreateRoomClicked()
    {
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 2 });
        roomNameInputField.text = "";
    }

    public void OnClickedRoomButton(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = roomsParent.childCount - 1; i >= 0; i--)
            Destroy(roomsParent.GetChild(i).gameObject);

        foreach (RoomInfo roomInfo in roomList)
        {
            if (!roomInfo.IsOpen)
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
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
}
