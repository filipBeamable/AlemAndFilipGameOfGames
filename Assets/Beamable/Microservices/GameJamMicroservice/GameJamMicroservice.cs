using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beamable.Server
{
    [Microservice("GameJamMicroservice")]
   public class GameJamMicroservice : Microservice
   {
        static List<PlayerInfo> players = new List<PlayerInfo>();

        [ClientCallable]
        public void JoinGame(Guid ownerId, string name)
        {
            players.AddRange(Enumerable.Range(1,3).Select(p => new PlayerInfo
            {
                 Id = Guid.NewGuid(),
                 OwnerId = ownerId,
                 Name = name
            }));
        }

        [ClientCallable]
        public string RefreshGameState(string clientPlayersJson)
        {
            PlayerInfo[] clientPlayers = JsonUtility.FromJson<PlayerInfo[]>(clientPlayersJson);
            var ownerId = clientPlayers.First().OwnerId;
            foreach (var clientPlayer in clientPlayers)
            {
                var serverPlayer = players.Single(p => p.Id == clientPlayer.Id);
                serverPlayer.Position = clientPlayer.Position;
                serverPlayer.Rotation = clientPlayer.Rotation;
            }
            var result = players
                .Where(p => p.OwnerId != ownerId)
                    .Select(x =>
                    new PlayerInfo
                    {
                        Name = x.Name,
                        Id = x.Id,
                        OwnerId =
                        x.OwnerId,
                        Rotation =
                        x.Rotation,
                        Position =
                        x.Position
                    })
                   .ToArray();
            return JsonUtility.ToJson(result);
        }
    }

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
}