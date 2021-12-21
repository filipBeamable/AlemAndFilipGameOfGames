using System;
using UnityEngine;


[Serializable]
public class PlayerInfo
{
    public PlayerInfo() { }
    public PlayerInfo(Guid id, Guid ownerId)
    {
        Position = new Vector3(0, 0, 0);
        Rotation = new Quaternion(0, 0, 0, 0);
        OwnerId = ownerId;
    }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Guid OwnerId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
}

