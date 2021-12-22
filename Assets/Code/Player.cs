using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    public float startingHealth = 100f;
    public Transform healthPosition;
    public MeshRenderer meshRenderer;
    public Material localPlayerMat;
    public Material otherPlayerMat;

    [Header("Sounds")]
    public AudioSource jumpSfx;
    public AudioSource shootSfx;
    public AudioSource hurtSfx;
    public GameObject diedAudioPrefab;

    public float Health { get; set; }

    protected HealthUI healthUI = null;

    protected virtual void Awake()
    {
        Health = startingHealth;
        meshRenderer.material = photonView.IsMine ? localPlayerMat : otherPlayerMat;
    }

    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            healthUI = UIController.Instance.InstantiateHealthUI();
            healthUI.index.text = (PlayerManager.Instance.players.IndexOf(this as PlayerController) + 1).ToString();
            healthUI.UpdateSlider(1f);
        }
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

    public void SetHealthSynced(float healthValue)
    {
        photonView.RPC("SetHealth", RpcTarget.All, healthValue);
    }
    [PunRPC]
    public void SetHealth(float healthValue)
    {
        Health = healthValue;
        if (healthUI != null)
            healthUI.UpdateSlider(Health / startingHealth);
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        PlayerManager.Instance.UpdateActivePlayerHealth();
        if (Health <= 0)
        {
            if (!photonView.IsMine)
                PlayerManager.Instance.IncrementScore(1);

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


    public void PlayShootSfx() => shootSfx.Play();
    public void PlayHurtSfx() => hurtSfx.Play();
    public void PlayDiedSfx() => Instantiate(diedAudioPrefab).transform.position = transform.position;
}
