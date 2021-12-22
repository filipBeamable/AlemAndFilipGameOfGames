using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    public float startingHealth = 100f;
    public Transform healthPosition;
    public MeshRenderer meshRenderer;
    public Material localPlayerMat;
    public Material otherPlayerMat;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip jumpSfx;
    public AudioClip shootSfx;
    public AudioClip hurtSfx;
    public AudioClip diedSfx;

    public float Health { get; private set; }

    protected HealthUI healthUI = null;

    protected virtual void Awake()
    {
        Health = startingHealth;
        if (photonView.IsMine)
        {
            healthUI = UIController.Instance.InstantiateHealthUI();
            healthUI.UpdateSlider(1f);
        }

        meshRenderer.material = photonView.IsMine ? localPlayerMat : otherPlayerMat;
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
        PlayerManager.Instance.UpdateActivePlayerHealth();
        if (Health <= 0)
        {
            gameObject.SetActive(false);
            if (healthUI != null)
                Destroy(healthUI.gameObject);
            PlayDiedSfx();
            PlayerManager.Instance.OnPlayerDied(this);
        }
        else
        {
            if (healthUI != null)
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
    public void PlayDiedSfx() => PlayerManager.Instance.audioSource.PlayOneShot(diedSfx);
}
