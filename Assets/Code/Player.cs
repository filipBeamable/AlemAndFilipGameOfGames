using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float startingHealth = 100f;
    public Transform healthPosition;
    public MeshRenderer meshRenderer;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip jumpSfx;
    public AudioClip shootSfx;
    public AudioClip hurtSfx;
    public AudioClip diedSfx;

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
            PlayDiedSfx();
        }
        else
        {
            healthUI.UpdateSlider(Health / startingHealth);
            PlayHurtSfx();
        }

        //if (this is PlayerController playerController)
        //    PlayerManager.Instance.players.Remove(playerController);
        //if (this is OtherPlayer otherPlayer)
        //    PlayerManager.Instance.otherPlayers.Remove(otherPlayer);
    }

    protected void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
    public void PlayJumpSfx() => PlaySound(jumpSfx);
    public void PlayShootSfx() => PlaySound(shootSfx);
    public void PlayHurtSfx() => PlaySound(hurtSfx);
    public void PlayDiedSfx() => PlaySound(diedSfx);
}
