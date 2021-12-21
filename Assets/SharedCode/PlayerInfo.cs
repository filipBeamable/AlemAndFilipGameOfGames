using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public PlayerInfo()
    {
        Position = new Vector3(0, 0, 0);
        Rotation = new Quaternion(0, 0, 0, 0);
    }
    public PlayerInfo(Guid ownerId) : this()
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
    }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Guid OwnerId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
}
