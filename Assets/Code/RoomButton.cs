using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomButton : MonoBehaviour
{
    public TextMeshProUGUI roomName;

    public void Init(string roomName)
    {
        this.roomName.text = roomName;
    }

    public void OnClicked()
    {
        MainMenu.Instance.OnClickedRoomButton(roomName.text);
    }
}
