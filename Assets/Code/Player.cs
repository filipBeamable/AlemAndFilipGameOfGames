using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float startingHealth = 100f;
    public Transform healthPosition;
    public MeshRenderer meshRenderer;

    public float Health { get; private set; }

    protected HealthUI healthUI;

    private void Awake()
    {
        Health = startingHealth;
        healthUI = UIController.Instance.InstantiateHealthUI();
        healthUI.UpdateSlider(1f);
    }

    protected virtual void Update()
    {
        if (healthUI != null)
        {
            healthUI.gameObject.SetActive(meshRenderer.isVisible);
            if (healthUI.gameObject.activeSelf)
                healthUI.UpdatePosition(healthPosition.position);
        }
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
            Destroy(healthUI.gameObject);
        }
        else
        {
            healthUI.UpdateSlider(Health / startingHealth);
        }

        //if (this is PlayerController playerController)
        //    PlayerManager.Instance.players.Remove(playerController);
        //if (this is OtherPlayer otherPlayer)
        //    PlayerManager.Instance.otherPlayers.Remove(otherPlayer);
    }
}
