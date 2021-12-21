using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float startingHealth = 100f;

    public float Health { get; private set; }

    private void Awake()
    {
        Health = startingHealth;
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        if (Health <= 0)
            Destroy(gameObject);

        if (this is PlayerController playerController)
            PlayerManager.Instance.players.Remove(playerController);
        if (this is OtherPlayer otherPlayer)
            PlayerManager.Instance.otherPlayers.Remove(otherPlayer);
    }
}
